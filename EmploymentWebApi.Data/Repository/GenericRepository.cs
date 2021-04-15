using EmploymentWebApi.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EmploymentWebApi.Data.Repository
{
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private bool _disposed = false;
        protected AppDbContext _context;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
        }

        public IQueryable<T> GetQuery()
        {
            return _context.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            return GetQuery().ToList();
        }

        public T Single(Expression<Func<T, bool>> where = null)
        {
            return (where == null)
                       ? _context.Set<T>().SingleOrDefault()
                       : _context.Set<T>().SingleOrDefault(where);
        }

        public T First(Expression<Func<T, bool>> where = null)
        {
            return (where == null)
                       ? _context.Set<T>().First()
                       : _context.Set<T>().First(where);
        }

        public void Attach(T entity)
        {
            _context.Set<T>().Attach(entity);
        }

        public virtual async Task<ICollection<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> query = (predicate != null) ? _context.Set<T>().Where(predicate) : _context.Set<T>();
            return await query.ToListAsync();
        }

        public virtual T Get(object id)
        {
            return _context.Set<T>().Find(id);
        }

        public IQueryable<T> GetObjectGraph(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "")
        {
            IQueryable<T> query = _context.Set<T>();

            if (filter != null)
                query = query.Where(filter);


            if (!string.IsNullOrWhiteSpace(includeProperties))
                query = includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            return orderBy != null ? orderBy(query).AsQueryable() : query.AsQueryable();
        }

        public virtual async Task<T> GetAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public virtual T Add(T t)
        {
            _context.Set<T>().Add(t);
            _context.SaveChanges();
            return t;
        }

        public virtual async Task<T> AddAsync(T t)
        {
            _context.Set<T>().Add(t);
            await _context.SaveChangesAsync();
            return t;
        }

        public virtual T Find(Expression<Func<T, bool>> match)
        {
            return _context.Set<T>().SingleOrDefault(match);
        }

        public virtual async Task<T> FindAsync(Expression<Func<T, bool>> match)
        {
            return await _context.Set<T>().SingleOrDefaultAsync(match);
        }

        public ICollection<T> FindAll(Expression<Func<T, bool>> match)
        {
            return _context.Set<T>().Where(match).ToList();
        }

        public async Task<ICollection<T>> FindAllAsync(Expression<Func<T, bool>> match)
        {
            return await _context.Set<T>().Where(match).ToListAsync();
        }

        public IQueryable<T> Filter(Expression<Func<T, bool>> predicate = null)
        {
            return (predicate == null) ? _context.Set<T>() : _context.Set<T>().Where(predicate).AsQueryable();
        }

        public IQueryable<T> Filter(Expression<Func<T, bool>> filter, out int total, int index = 0, int size = 50)
        {
            var skipCount = index * size;
            var resetSet = filter != null ? _context.Set<T>().Where(filter).AsQueryable() : _context.Set<T>().AsQueryable();
            resetSet = skipCount == 0 ? resetSet.Take(size) : resetSet.Skip(skipCount).Take(size);
            total = resetSet.Count();
            return resetSet.AsQueryable();
        }

        public virtual int Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
            return _context.SaveChanges();
        }

        public void Delete(Expression<Func<T, bool>> predicate)
        {
            var entitiesToDelete = Filter(predicate);
            
            foreach (var entity in entitiesToDelete)
            {
                if (_context.Entry(entity).State == EntityState.Detached)
                {
                    _context.Set<T>().Attach(entity);
                }

                _context.Set<T>().Remove(entity);
            }
        }

        public virtual async Task<int> DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            return await _context.SaveChangesAsync();
        }

        public virtual T Update(T t, object key)
        {
            if (t == null)
                return null;

            T exist = _context.Set<T>().Find(key);

            if (exist != null)
            {
                _context.Entry(exist).CurrentValues.SetValues(t);
                _context.SaveChanges();
            }

            return exist;
        }

        public virtual async Task<T> UpdateAsync(T t, object key)
        {
            if (t == null)
                return null;

            T exist = await _context.Set<T>().FindAsync(key);

            if (exist != null)
            {
                _context.Entry(exist).CurrentValues.SetValues(t);
                await _context.SaveChangesAsync();
            }

            return exist;
        }

        public int Count
        {
            get { return _context.Set<T>().Count(); }
        }

        public int GetCountFor(Expression<Func<T, bool>> predicate = null)
        {
            return (predicate == null)
                       ? _context.Set<T>().Count()
                       : _context.Set<T>().Count(predicate);
        }

        public async Task<int> CountAsync()
        {
            return await _context.Set<T>().CountAsync();
        }

        public virtual int Save()
        {
            return _context.SaveChanges();
        }

        public async virtual Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public bool Exist(Expression<Func<T, bool>> predicate = null)
        {
            return (predicate == null) ? _context.Set<T>().Any() : _context.Set<T>().Any(predicate);
        }

        public virtual IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> query = _context.Set<T>().Where(predicate);
            return query;
        }

        public T Find(params object[] keys)
        {
            return _context.Set<T>().Find(keys);
        }


        public virtual async Task<ICollection<T>> FindByAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).ToListAsync();
        }

        public IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> queryable = GetQuery();

            foreach (Expression<Func<T, object>> includeProperty in includeProperties)
            {
                queryable = queryable.Include<T, object>(includeProperty);
            }

            return queryable;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }

                this._disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
