using McAlister.Study.CoreSample1.Definitions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace McAlister.Study.CoreSample1.Services
{
    /// <summary>
    /// Returns IQueryable in case Repository wants to use method and take advantage of lazy loading, but IQuerable should not go above Repository
    /// https://blog.ploeh.dk/2012/03/26/IQueryableTisTightCoupling/
    /// </summary>
    /// <typeparam name="C"></typeparam>
    public class GenericRepository<C> : DbContext, IDisposable
        where C : DbContext, new()
    {
        private C _DataContext;
        public virtual C DataContext
        {
            get
            {
                if (_DataContext == null)
                {
                    _DataContext = new C();
                }
                return _DataContext;
            }
        }

        public virtual T Get<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            if (predicate != null)
            {
                var x = DataContext.Set<T>().Where(predicate).SingleOrDefault();
                return x;
            }
            else
            {
                throw new ApplicationException("Predicate value must be passed to Get<T>.");
            }
        }

        public virtual IQueryable<T> GetList<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return DataContext.Set<T>().Where(predicate);
        }

        public virtual IQueryable<T> GetList<T, TKey>(Expression<Func<T, bool>> predicate,
            Expression<Func<T, TKey>> orderBy, bool isDescending, int pageSize, int page) where T : class
        {
            if(isDescending)
                return GetList(predicate).OrderByDescending(orderBy).Skip(pageSize * (page - 1)).Take(pageSize);
            else
                return GetList(predicate).OrderBy(orderBy).Skip(pageSize * (page - 1)).Take(pageSize);
        }

        public virtual IQueryable<T> GetList<T, TKey>(Expression<Func<T, TKey>> orderBy, bool isDecending, int pageSize, int page) where T : class
        {
            if(isDecending)
                return GetList<T>().OrderByDescending(orderBy).Skip(pageSize * (page - 1)).Take(pageSize);
            else
                return GetList<T>().OrderBy(orderBy).Skip(pageSize * (page - 1)).Take(pageSize);
        }


        public virtual IQueryable<T> GetList<T>() where T : class
        {
            return DataContext.Set<T>();
        }

        public T AddOrUpdate<T>(T entity) where T : class
        {
            //recommend do not use, becasue if model was incomplete, entity will be incomplete and EF will not know and write incomplete entity, thus data can be lost
            throw new NotImplementedException();
        }

        public T Insert<T>(T entity) where T : class
        {
            DataContext.Set<T>().Add(entity);
            return entity;
        }

        public virtual void CopyValues<T>(T modifiedEntity, T existingEntity) where T : class
        {
            var attachedEntry = DataContext.Entry(existingEntity);
            attachedEntry.CurrentValues.SetValues(modifiedEntity);
        }

        public virtual T GetById<T>(object[] ids) where T : class
        {
            var e = DataContext.Set<T>().Find(ids);
            return e;
        }

        public virtual T Update<T>(T entity) where T : class
        {
            //assumes entity is tracked because you just retrieved it and updated its values
            var entry = DataContext.Entry<T>(entity);
            entry.State = EntityState.Modified;
            return entity;
        }

        public virtual void Delete<T>(T entity) where T : class
        {
            if (entity != null)
            {
                var entry = DataContext.Entry(entity);
                if (entry.State == EntityState.Detached)
                    DataContext.Set<T>().Attach(entity);
                DataContext.Set<T>().Remove(entity);
            }
        }

        public virtual void Delete<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            var lst = GetList<T>(predicate);
            DbSet<T> objectSet = DataContext.Set<T>();
            foreach (var entity in lst)
            {
                objectSet.Remove(entity);
            }
        }

        public void DeleteRange<T>(IEnumerable<T> lst) where T : class
        {
            DataContext.Set<T>().RemoveRange(lst);
        }

        public virtual OperationStatus ExecuteStoreCommand(string cmdText, params object[] parameters)
        {
            var opStatus = new OperationStatus { Status = true };

            try
            {
                opStatus.RecordsAffected = DataContext.Database.ExecuteSqlRaw(cmdText, parameters);
            }
            catch (Exception exp)
            {
                OperationStatus.CreateFromException("Error executing store command: ", exp);
            }
            return opStatus;
        }

        public int SubmitChanges()
        {
            int result = 0;
            try
            {
                result = this.DataContext.SaveChanges();
            }
            catch (DbUpdateException dbUEx)
            {
                TrackEntityChanges();
                var sqlException = dbUEx.GetBaseException();
                if (sqlException != null)
                    throw sqlException;
                throw dbUEx;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public async Task<int> SubmitChangesAsync()
        {
            int result = 0;
            try
            {
                result = await this.DataContext.SaveChangesAsync();
            }
            catch (DbUpdateException dbUEx)
            {
                TrackEntityChanges();
                var sqlException = dbUEx.GetBaseException();
                if (sqlException != null)
                    throw sqlException;
                throw dbUEx;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        private void TrackEntityChanges()
        {
            foreach (EntityEntry entry in this.DataContext.ChangeTracker.Entries())
            {
                TrackEntityChanges(entry);
            }
        }

        private void TrackEntityChanges(EntityEntry entry)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.State = EntityState.Detached;
                    break;
                case EntityState.Modified:
                case EntityState.Deleted:
                    entry.Reload();
                    break;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _DataContext = null;
            }
        }
    }
}
