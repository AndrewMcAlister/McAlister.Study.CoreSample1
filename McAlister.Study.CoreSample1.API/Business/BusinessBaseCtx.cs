using McAlister.Study.CoreSample1.DAL;
using McAlister.Study.CoreSample1.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace McAlister.Study.CoreSample1.Business
{
    public abstract class BusinessBaseCtx<T> where T : class
    {
        protected WideWorldImportersContext Repo { get; set; }

        public abstract Boolean IsValid(T entity, ref String msg);

        public abstract T FindExact(T entity);

        public BusinessBaseCtx(WideWorldImportersContext repo)
        {
            Repo = repo;
        }

        public virtual T Insert(T entity)
        {
            throw new NotImplementedException();
        }

        public virtual T Update(T entity)
        {
            throw new NotImplementedException();
        }

        public void CopyValues<T>(T modifiedEntity, T existingEntity) where T : class
        {
            throw new NotImplementedException();
        }

        public virtual T Get(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public virtual List<T> GetList(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        //df.IRepository.GetList<T, TKey>(Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> orderBy, int pageSize, int page)
        public virtual List<T> GetList<TKey>(Expression<Func<T,bool>> predicate, Expression<Func<T,TKey>>orderBy, bool isDescending, int pageSize, int page)
        {
            throw new NotImplementedException();
        }

        public virtual List<T> GetList()
        {
            throw new NotImplementedException();
        }

        public virtual void Delete<T>(T entity) where T : class
        {
            throw new NotImplementedException();
        }

        public virtual void DeleteRange<T>(IList<T> lst) where T : class
        {
            throw new NotImplementedException();
        }

        public virtual void SaveChanges()
        {
            throw new NotImplementedException();
        }

        public virtual async void SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
