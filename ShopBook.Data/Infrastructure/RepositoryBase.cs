using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ShopBook.Data.Infrastructure
{
    public abstract class RepositoryBase<T> : IRepository<T> where T : class
    {
        private BookstoreContext _dataContext;

        protected RepositoryBase(BookstoreContext dbContext)
        {
            _dataContext = dbContext;
        }

        // protected ProjectDbContext DbContext => _dataContext ?? (_dataContext = DbFactory());

        public async Task<T> AddASync(T entity)
        {
            _dataContext.Set<T>().Add(entity);
            _dataContext.Entry(entity).State = EntityState.Added;
            await _dataContext.SaveChangesAsync();
            return entity;
        }
        public bool CheckContains(Expression<Func<T, bool>> predicate)
        {
            return _dataContext.Set<T>().Count(predicate) > 0;
        }

        public async Task<bool> CheckContainsAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dataContext.Set<T>().CountAsync(predicate) > 0;
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> where)
        {
            return await _dataContext.Set<T>().CountAsync(where);
        }

        public async Task<T> DeleteAsync(int id)
        {
            var entity = await _dataContext.Set<T>().FindAsync(id);
            if (entity == null)
            {
                return entity;
            }

            _dataContext.Set<T>().Remove(entity);
            _dataContext.Entry(entity).State = EntityState.Deleted;
            await _dataContext.SaveChangesAsync();

            return entity;
        }

        public async Task<T> DeleteAsync(string id)
        {
            var entity = await _dataContext.Set<T>().FindAsync(id);
            if (entity == null)
            {
                return entity;
            }

            _dataContext.Set<T>().Remove(entity);
            _dataContext.Entry(entity).State = EntityState.Deleted;
            await _dataContext.SaveChangesAsync();

            return entity;
        }

        public async Task DeleteMulti(Expression<Func<T, bool>> where)
        {
            IEnumerable<T> objects = _dataContext.Set<T>().Where(where).AsEnumerable();
            foreach (T obj in objects)
                _dataContext.Remove(obj);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<IQueryable<T>> GetAllAsync(string[] includes = null)
        {
            var query = await _dataContext.Set<T>().ToListAsync();
            return query.AsQueryable();
        }

        public async Task<IQueryable<T>> GetAllAsync(Expression<Func<T, bool>> predicate, string[] includes = null)
        {
            if (includes != null && includes.Count() > 0)
            {
                var query = _dataContext.Set<T>().Include(includes.First());
                foreach (var include in includes.Skip(1))
                {
                    query = query.Include(include);
                }
                var query1 = await query.Where(predicate).ToListAsync();
                return query1.AsQueryable();
            }
            var query2 = await _dataContext.Set<T>().Where(predicate).ToListAsync();
            return query2.AsQueryable();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dataContext.Set<T>().FindAsync(id);
        }

        public async Task<T> GetByIdAsync(string id)
        {
            return await _dataContext.Set<T>().FindAsync(id);
        }

        public async Task<T> GetSingleByConditionAsync(Expression<Func<T, bool>> expression, string[] includes = null)
        {
            if (includes != null && includes.Count() > 0)
            {
                var query = _dataContext.Set<T>().Include(includes.First());
                foreach (var include in includes.Skip(1))
                    query = query.Include(include);
                return await query.FirstOrDefaultAsync(expression);
            }
            return await _dataContext.Set<T>().FirstOrDefaultAsync(expression);
        }

        public async Task<T> UpdateASync(T entity)
        {
            _dataContext.Set<T>().Attach(entity);
            _dataContext.Entry(entity).State = EntityState.Modified;
            await _dataContext.SaveChangesAsync();
            return entity;
        }
    }
}
