using McAlister.Study.CoreSample1.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using df = McAlister.Study.CoreSample1.Definitions;
using dfe = McAlister.Study.CoreSample1.Definitions.Entities;
using McAlister.Study.CoreSample1.DAL;
using System.Linq.Expressions;
using McAlister.Study.CoreSample1.Definitions;

namespace McAlister.Study.CoreSample1.Services
{
    ///This is the place to put any complex linq queries.  No where else!
    ///Entity Container Name must match line below, and there must be a connection string of same name in ProdAdd even though we want to overwrite connection string.
    ///Do not return IQueryable ! https://blog.ploeh.dk/2012/03/26/IQueryableTisTightCoupling/
    ///Note .AsNoTracking().ToList() quicker if readonly.
    public class Repository : GenericRepository<WideWorldImportersContext>, df.IRepository, IDisposable
    {
        public String ConnStr
        {
            get
            {
                return Context.Database.GetDbConnection().ConnectionString;
            }
        }

        public DbContext Context
        {
            get
            {
                return base.DataContext;
            }
        }

        public SQLDb GetSQLDbObj()
        {
            var obj = new SQLDb(ConnStr);
            return obj;
        }

        public Repository() : base()
        {
        }

        public ICollection<dfe.Orders> GetOrdersNoEF()
        {
            var sql = $"Select * from Orders";
            var sqldb = GetSQLDbObj();
            var dt = sqldb.GetData(sql);
            var results = Utility.ConvertDataTableToList<dfe.Orders>(dt);
            return results;
        }

        public DataTable GetOrdersNoEFDT()
        {
            var sql = $"Select * from Orders";
            var sqldb = GetSQLDbObj();
            var dt = sqldb.GetData(sql);
            return dt;
        }

        ICollection<T> df.IRepository.GetList<T>(Expression<Func<T, bool>> predicate)
        {
            return base.GetList<T>(predicate).ToList();
        }

        ICollection<T> df.IRepository.GetList<T, TKey>(Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> orderBy)
        {
            return base.GetList<T, TKey>(predicate, orderBy).ToList();
        }

        ICollection<T> df.IRepository.GetList<T, TKey>(Expression<Func<T, TKey>> orderBy)
        {
            return base.GetList<T, TKey>(orderBy).ToList();
        }

        ICollection<T> df.IRepository.GetList<T>()
        {
            return base.GetList<T>().ToList();
        }

        void IDisposable.Dispose()
        {
            //TODO Consider implemneting dispose pattern
            this.Database.CloseConnection();
            base.Dispose();
        }
    }

}
