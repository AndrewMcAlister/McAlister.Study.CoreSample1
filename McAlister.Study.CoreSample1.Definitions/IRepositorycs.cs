using McAlister.Study.CoreSample1.Definitions.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace McAlister.Study.CoreSample1.Definitions
{
    public interface IRepository
    {
        #region Generic
        String ConnStr { get;}
        DbContext Context { get; } // I want the context to be available so a business class can control a transaction
        T Get<T>(Expression<Func<T, bool>> predicate) where T : class;
        ICollection<T> GetList<T>(Expression<Func<T, bool>> predicate) where T : class;
        ICollection<T> GetList<T, TKey>(Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> orderBy) where T : class;
        ICollection<T> GetList<T, TKey>(Expression<Func<T, TKey>> orderBy) where T : class;
        ICollection<T> GetList<T>() where T : class;
        OperationStatus ExecuteStoreCommand(string cmdText, params object[] parameters);
        T Insert<T>(T entity) where T : class;
        T Update<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        void Delete<T>(Expression<Func<T, bool>> predicate) where T : class;
        ICollection<Orders> GetOrdersNoEF();
        DataTable GetOrdersNoEFDT();
        void DeleteRange<T>(IEnumerable<T> lst) where T : class;
        void CopyValues<T>(T modifiedEntity, T existingEntity) where T : class;
        T GetById<T>(object[] ids) where T : class;
        int SubmitChanges();
        Task<int> SubmitChangesAsync();

        #endregion

    }
}
