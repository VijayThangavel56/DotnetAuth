using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DAL.Repositories.IRepository
{
    public interface IRepositoryBase<T> where T : class
    {
        Task AddAsync(T entity);
        Task AddAsync(IEnumerable<T> entities);

        void Update(T entity);
        void Update(IEnumerable<T> entities);
        Task<int> ExecuteUpdateAsync(Expression<Func<T, bool>> where, Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> setPropertyCalls);
        void Delete(T entity);
        void Delete(IEnumerable<T> entities);
        void Delete(Expression<Func<T, bool>> where);
        Task<int> ExecuteDeleteAsync(Expression<Func<T, bool>> where);

        Task<T?> GetByIdAsync(object id);
        /// <summary>
        /// Asynchronously fetch single record based on filter condition applied.        
        /// </summary>
        /// <remarks>
        ///     <para>
        ///     <strong>Performance Recommendations:</strong>
        ///         <list type="bullet">
        ///             <item>
        ///                 Set <i><strong>noTracking</strong></i> to <i><strong>true</strong></i> 
        ///                 for read-only operations to skip registering with EF change tracker.
        ///             </item>
        ///             <item>
        ///                 Use projection selector to fetch only columns required.
        ///             </item>
        ///         </list> 
        ///     </para>
        ///     <para>
        ///         <strong>Important:</strong>
        ///         Use projection selector only for read-only queries to avoid accidental in-complete updates to database.<br/><br/>
        ///     </para>
        ///     <para>
        ///         ** Read-only means the result entity not used for Add() or Update() operations.<br/><br/>
        ///     </para>
        ///     <para>
        ///         Refer <i>Developer Notes</i> in <see cref="RepositoryBase.cs"/> to understand in detail and sample projection experssion.
        ///     </para>
        /// </remarks>
        /// <param name="where">Filter condition</param>
        /// <param name="include">Expression to join/include child tables.</param>
        /// <param name="selector">Projection selector - to fetch only required columns.</param>
        /// <param name="noTracking">Skip registering the result entities to EF tracking to improve performance.</param>
        /// <returns></returns>
        Task<T?> GetAsync(Expression<Func<T, bool>> where, Func<IQueryable<T>, IIncludableQueryable<T, object?>>? include = null, Expression<Func<T, T>>? selector = null, bool noTracking = false);

        /// <summary>
        /// Asynchronously fetch single record based on filter condition applied. Take top 1 after sorting.        
        /// </summary>
        /// <remarks>
        ///     <para>
        ///     <strong>Performance Recommendations:</strong>
        ///         <list type="bullet">
        ///             <item>
        ///                 Set <i><strong>noTracking</strong></i> to <i><strong>true</strong></i> 
        ///                 for read-only operations to skip registering with EF change tracker.
        ///             </item>
        ///             <item>
        ///                 Use projection selector to fetch only columns required.
        ///             </item>
        ///         </list> 
        ///     </para>
        ///     <para>
        ///         <strong>Important:</strong>
        ///         Use projection selector only for read-only queries to avoid accidental in-complete updates to database.<br/><br/>
        ///     </para>
        ///     <para>
        ///         ** Read-only means the result entity not used for Add() or Update() operations.<br/><br/>
        ///     </para>
        ///     <para>
        ///         Refer <i>Developer Notes</i> in <see cref="RepositoryBase.cs"/> to understand in detail and sample projection experssion.
        ///     </para>
        /// </remarks>
        /// <param name="where">Filter condition</param>
        /// <param name="orderBy">Sorting expression</param>
        /// <param name="include">Expression to join/include child tables.</param>
        /// <param name="selector">Projection selector - to fetch only required columns.</param>
        /// <param name="noTracking">Skip registering the result entities to EF tracking to improve performance.</param>
        /// <param name="sortByDesc">Sort direction. Default to false. If true sort records in descending order.</param>
        /// <returns></returns>
        Task<T?> GetSortedAsync(Expression<Func<T, bool>> where, Expression<Func<T, object>> orderBy, Func<IQueryable<T>, IIncludableQueryable<T, object?>>? include = null, Expression<Func<T, T>>? selector = null, bool noTracking = false, bool sortByDesc = false);

        /// <summary>
        /// Asynchronously fetch one or more records based on filter condition applied.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///     <strong>Performance Recommendations:</strong>
        ///         <list type="bullet">
        ///             <item>
        ///                 Set <i><strong>noTracking</strong></i> to <i><strong>true</strong></i> 
        ///                 for read-only operations to skip registering with EF change tracker.
        ///             </item>
        ///             <item>
        ///                 Use projection selector to fetch only columns required.
        ///             </item>
        ///         </list> 
        ///     </para>
        ///     <para>
        ///         <strong>Important:</strong>
        ///         Use projection selector only for read-only queries to avoid accidental in-complete updates to database.<br/><br/>
        ///     </para>
        ///     <para>
        ///         ** Read-only means the result entity not used for Add() or Update() operations.<br/><br/>
        ///     </para>
        ///     <para>
        ///         Refer <i>Developer Notes</i> in <see cref="RepositoryBase.cs"/> to understand in detail and sample projection experssion.
        ///     </para>
        /// </remarks>
        /// <param name="where">Filter condition</param>
        /// <param name="include">Expression to join/include child tables.</param>
        /// <param name="selector">Projection selector - to fetch only required columns.</param>
        /// <param name="noTracking">Skip registering the result entities to EF tracking to improve performance.</param>
        /// <returns></returns>
        Task<IEnumerable<T>> GetManyAsync(Expression<Func<T, bool>> where, Func<IQueryable<T>, IIncludableQueryable<T, object?>>? include = null, Expression<Func<T, T>>? selector = null, bool noTracking = false);

        /// <summary>
        /// Asynchronously fetch one or more records based on filter condition applied.
        /// Take top N number of records after sorting in a direction mentioned.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///     <strong>Performance Recommendations:</strong>
        ///         <list type="bullet">
        ///             <item>
        ///                 Set <i><strong>noTracking</strong></i> to <i><strong>true</strong></i> 
        ///                 for read-only operations to skip registering with EF change tracker.
        ///             </item>
        ///             <item>
        ///                 Use projection selector to fetch only columns required.
        ///             </item>
        ///         </list> 
        ///     </para>
        ///     <para>
        ///         <strong>Important:</strong>
        ///         Use projection selector only for read-only queries to avoid accidental in-complete updates to database.<br/><br/>
        ///     </para>
        ///     <para>
        ///         ** Read-only means the result entity not used for Add() or Update() operations.<br/><br/>
        ///     </para>
        ///     <para>
        ///         Refer <i>Developer Notes</i> in <see cref="RepositoryBase.cs"/> to understand in detail and sample projection experssion.
        ///     </para>
        /// </remarks>
        /// <param name="where">Filter condition</param>
        /// <param name="orderBy">Sorting expression</param>
        /// <param name="include">Expression to join/include child tables.</param>
        /// <param name="selector">Projection selector - to fetch only required columns.</param>
        /// <param name="noTracking">Skip registering the result entities to EF tracking to improve performance.</param>
        /// <param name="sortByDesc">Sort direction. Default to false. If true sort records in descending order.</param>
        /// <returns></returns>
        Task<IEnumerable<T>> GetManySortedAsync(Expression<Func<T, bool>> where, Expression<Func<T, T>> orderBy, int take, Func<IQueryable<T>, IIncludableQueryable<T, object?>>? include = null, Expression<Func<T, T>>? selector = null, bool noTracking = false, bool sortByDesc = false);

        /// <summary>
        /// Asynchronously fetch all records from the entity / table.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///     <strong>Performance Recommendations:</strong>
        ///         <list type="bullet">
        ///             <item>
        ///                 Set <i><strong>noTracking</strong></i> to <i><strong>true</strong></i> 
        ///                 for read-only operations to skip registering with EF change tracker.
        ///             </item>
        ///             <item>
        ///                 Use projection selector to fetch only columns required.
        ///             </item>
        ///         </list> 
        ///     </para>
        ///     <para>
        ///         <strong>Important:</strong>
        ///         Use projection selector only for read-only queries to avoid accidental in-complete updates to database.<br/><br/>
        ///     </para>
        ///     <para>
        ///         ** Read-only means the result entity not used for Add() or Update() operations.<br/><br/>
        ///     </para>
        ///     <para>
        ///         Refer <i>Developer Notes</i> in <see cref="RepositoryBase.cs"/> to understand in detail and sample projection experssion.
        ///     </para>
        /// </remarks>
        /// <param name="include">Expression to join/include child tables.</param>
        /// <param name="selector">Projection selector - to fetch only required columns.</param>
        /// <param name="noTracking">Skip registering the result entities to EF tracking to improve performance.</param>
        /// <returns></returns>
        Task<IEnumerable<T>> GetAllAsync(Func<IQueryable<T>, IIncludableQueryable<T, object?>>? include = null, Expression<Func<T, T>>? selector = null, bool noTracking = false);

        /// <summary>
        /// Asynchrnously check if any records available for given condition.
        /// </summary>
        /// <param name="where">Filter condition</param>
        /// <returns>true or false</returns>
        Task<bool> EntityExistAsync(Expression<Func<T, bool>> where);

        /// <summary>
        /// Return total number of records for the given filter condition.
        /// </summary>
        /// <param name="where">Filter condition</param>
        /// <returns>true or false</returns>
        Task<long> CountAsync(Expression<Func<T, bool>> where);
        Task<IEnumerable<T>> GetManyPagedAsync(Expression<Func<T, bool>> where, Expression<Func<T, object>> orderBy, Func<IQueryable<T>, IIncludableQueryable<T, object?>>? include = null, Expression<Func<T, T>>? selector = null,
        int? pageNumber = null,
        int? pageSize = null,
        bool sortByDesc = false,
        bool noTracking = false);

        void DetachChanges();

    }
}
