using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using df = McAlister

namespace McAlister.Study.CoreSample1.Tests.Controllers
{
    [TestClass]
    public abstract class BaseTest
    {
        protected LifetimeResetter Resetter { get; set; }

        protected BaseTest()
        {
            Resetter = new LifetimeResetter();
            UC = UnityConfig.Container;
            Utility.SetConnectionMode("Development");
            Repo = UC.Resolve<df.IRepository>(new ParameterOverride(typeof(String), "connStr", null));
        }

        [TestInitialize]
        public void OnTestSetup()
        {
            Resetter.Reset();
        }

        protected void RegisterResettableType<T>(params InjectionMember[] injectionMembers)
        {
            UC.RegisterType<T>(new ResettableLifetimeManager(Resetter), injectionMembers);
        }

        protected class LifetimeResetter
        {
            public event EventHandler<EventArgs> OnReset;

            public void Reset()
            {
                OnReset?.Invoke(this, EventArgs.Empty);
            }
        }

        protected class ResettableLifetimeManager : LifetimeManager, ITypeLifetimeManager
        {
            public ResettableLifetimeManager(LifetimeResetter lifetimeResetter)
            {
                lifetimeResetter.OnReset += (o, args) => instance = null;
            }

            private object instance;

            public override object GetValue(ILifetimeContainer container = null)
            {
                return base.GetValue(container);
            }

            public override void SetValue(object newValue, ILifetimeContainer container = null)
            {
                base.SetValue(newValue, container);
            }

            public override void RemoveValue(ILifetimeContainer container = null)
            {
                base.RemoveValue(container);
            }

            protected override LifetimeManager OnCreateLifetimeManager()
            {
                throw new NotImplementedException();
            }

        }

        protected String GetJsonFile(String filename)
        {
            var assPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            String basePath = assPath.Substring(0, assPath.IndexOf("bin")) + "JsonTestFiles";
            String pathAndName = basePath + $"\\{filename}" + ".json";
            var result = Utility.ReadFileToString(pathAndName);
            return result;
        }
    }
}
