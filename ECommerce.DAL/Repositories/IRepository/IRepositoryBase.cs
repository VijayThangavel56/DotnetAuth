using ECommerce.DTO;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace ECommerce.DAL.Repositories.IRepository
{
    public interface IRepositoryBase<T> where T : class
    {
        #region Add
        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        #endregion

        #region Update
        void Update(T entity);
        void UpdateRange(IEnumerable<T> entities);
        #endregion

        #region Delete
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);
        void DeleteWhere(Expression<Func<T, bool>> where);
        #endregion

        #region Query
        Task<T?> GetAsync(Expression<Func<T, bool>> where,Func<IQueryable<T>, IIncludableQueryable<T,object?>>? include=null,Expression<Func<T,T>>? selector=null,Expression<Func<T, object>>? orderBy = null, bool noTracking = false);
       
        Task<ICollection<T>> GetAllAsync(Func<IQueryable<T>, IIncludableQueryable<T, object?>>? include = null, Expression<Func<T, T>>? selector = null, Expression<Func<T, object>>? orderBy = null, bool noTracking = false);
        Task<ICollection<T>> GetManyAsync(Expression<Func<T, bool>> where,Func<IQueryable<T>,IIncludableQueryable<T,object?>>? include=null,Expression<Func<T,T>>? selector=null, Expression<Func<T, object>>? orderBy = null, bool noTracking = false);
        
        Task<bool> EntityExistAsync(Expression<Func<T,bool>> where);

        Task<long> CountAsync(Expression<Func<T, bool>>? where = null);

        Task<PagedResult<T>> GetPagedAsync(
            Expression<Func<T, bool>>? where = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object?>>? include = null,
            Expression<Func<T, T>>? selector = null,
            Expression<Func<T, object>>? orderBy = null,
            bool noTracking = false,
            int pageNumber = 1,
            int pageSize = 10);

        #endregion

        #region Utility
        void DetachChanges();
        #endregion
    }
}
