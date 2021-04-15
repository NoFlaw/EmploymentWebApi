using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EmploymentWebApi.Data.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        T Add(T t);
        Task<T> AddAsync(T t);
        Task<int> CountAsync();
        int Delete(T entity);

        /// <summary>
        ///   Delete objects from database by specified filter expression.
        /// </summary>
        /// <param name="predicate"> </param>
        void Delete(Expression<Func<T, bool>> predicate);

        Task<int> DeleteAsync(T entity);

        /// <summary>
        ///   Returns SingleOrDefault that satisfies the where clause
        /// </summary>
        /// <param name="where"> takes an nullable condition </param>
        T Single(Expression<Func<T, bool>> where);

        /// <summary>
        ///   Returns First that satisfies the where clause
        /// </summary>
        /// <param name="where"> takes an nullable condition </param>
        T First(Expression<Func<T, bool>> where);

        /// <summary>
        ///   Adds object to the context underlying set as if it was read from the database.
        /// </summary>
        /// <param name="entity"> Specified the object to save. </param>
        void Attach(T entity);

        /// <summary>
        ///   Get the total object count.
        /// </summary>
        int Count { get; }

        /// <summary>
        ///   Get the total count for object that satisfies the where clause
        /// </summary>
        /// <param name="predicate"> takes an nullable condition </param>
        int GetCountFor(Expression<Func<T, bool>> predicate);

        /// <summary>
        ///   Gets objects via optional filter, sort order, and includes
        /// </summary>
        /// <param name="filter"> </param>
        /// <param name="orderBy"> </param>
        /// <param name="includeProperties"> </param>
        /// <returns> </returns>
        IQueryable<T> GetObjectGraph(Expression<Func<T, bool>> filter = null,
                                     Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                     string includeProperties = "");

        /// <summary>
        ///   Find object by specified expression and within accordance to predicate if provided.
        /// </summary>
        /// <param name="predicate"> </param>
        bool Exist(Expression<Func<T, bool>> predicate);

        void Dispose();

        T Find(Expression<Func<T, bool>> match);

        /// <summary>
        ///   Gets objects from database by a given filter.
        /// </summary>
        /// <param name="predicate"> Specified a filter </param>
        IQueryable<T> Filter(Expression<Func<T, bool>> predicate);

        /// <summary>
        ///   Gets objects from database with filtering and paging.
        /// </summary>
        /// <param name="filter"> Specified a filter </param>
        /// <param name="total"> Returns the total records count of the filter. </param>
        /// <param name="index"> Specified the page index, default: 0 </param>
        /// <param name="size"> Specified the page size, default: 50 </param>
        IQueryable<T> Filter(Expression<Func<T, bool>> filter, out int total, int index = 0, int size = 50);

        /// <summary>
        ///   Find object by keys.
        /// </summary>
        /// <param name="keys"> Specified the search keys. </param>
        T Find(params object[] keys);

        ICollection<T> FindAll(Expression<Func<T, bool>> match);
        Task<ICollection<T>> FindAllAsync(Expression<Func<T, bool>> match);
        Task<T> FindAsync(Expression<Func<T, bool>> match);
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);
        Task<ICollection<T>> FindByAsync(Expression<Func<T, bool>> predicate);
        T Get(object id);
        IQueryable<T> GetQuery();
        IEnumerable<T> GetAll();
        Task<ICollection<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate);
        IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties);
        Task<T> GetAsync(int id);
        int Save();
        Task<int> SaveAsync();
        T Update(T t, object key);
        Task<T> UpdateAsync(T t, object key);
    }
}
