﻿using McAlister.Study.CoreSample1.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace McAlister.Study.CoreSample1.Business
{
    public abstract class BusinessBase<T> where T : class
    {
        protected IRepository Repo { get; set; }

        public abstract Boolean IsValid(T entity, ref String msg);

        public abstract T FindExact(T entity);

        public BusinessBase(IRepository repo)
        {
            Repo = repo;
        }

        public virtual T Insert(T entity)
        {
            T result = null;
            String msg = null;
            if (IsValid(entity, ref msg))
            {
                if (FindExact(entity) == null)
                {
                    result = Repo.Insert<T>(entity);
                }
            }
            else
                throw new Exception(msg);

            return result;
        }

        public virtual T Update(T entity)
        {
            T result = null;
            String msg = null;
            if (entity != null)
            {
                if (IsValid(entity, ref msg))
                {
                    result = Repo.Update<T>(entity);
                }
            }

            return result;
        }

        public void CopyValues<T>(T modifiedEntity, T existingEntity) where T : class
        {
            Repo.CopyValues(modifiedEntity, existingEntity);
        }

        public virtual T Get(Expression<Func<T, bool>> predicate)
        {
            return Repo.Get<T>(predicate);
        }

        public virtual List<T> GetList(Expression<Func<T, bool>> predicate)
        {
            return Repo.GetList<T>(predicate).ToList();
        }

        //df.IRepository.GetList<T, TKey>(Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> orderBy, int pageSize, int page)
        public virtual List<T> GetList<TKey>(Expression<Func<T,bool>> predicate, Expression<Func<T,TKey>>orderBy, bool isDescending, int pageSize, int page)
        {
            return Repo.GetList(predicate,orderBy, isDescending, pageSize,page).ToList();
        }

        public virtual List<T> GetList()
        {
            return Repo.GetList<T>().ToList();
        }

        public virtual void Delete<T>(T entity) where T : class
        {
            Repo.Delete(entity);
        }

        public virtual void DeleteRange<T>(IList<T> lst) where T : class
        {
            IEnumerable<T> l = lst.AsEnumerable();
            Repo.DeleteRange<T>(l);
        }

        public virtual void SaveChanges()
        {
            Repo.SubmitChanges();
        }

        public virtual async void SaveChangesAsync()
        {
            await Repo.SubmitChangesAsync();
        }
    }
}
