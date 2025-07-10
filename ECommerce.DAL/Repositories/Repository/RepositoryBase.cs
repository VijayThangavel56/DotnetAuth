using ECommerce.DAL.Data;
using ECommerce.DAL.Repositories.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DAL.Repositories.Repository
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        private readonly ApplicationDbContext context;
        private readonly DbSet<T> _dbSet;

        public RepositoryBase(ApplicationDbContext dbContext)
        {
            context = dbContext;
            _dbSet = context.Set<T>();
        }

        public virtual async Task AddAsync(T entity) => await context.Set<T>().AddAsync(entity);

        public virtual async Task AddAsync(IEnumerable<T> entities) => await context.Set<T>().AddRangeAsync(entities);

        public virtual void Update(T entity) => context.Set<T>().Attach(entity);

        public virtual void Update(IEnumerable<T> entities) => context.Set<T>().AttachRange(entities);

        public virtual async Task<int> ExecuteUpdateAsync(Expression<Func<T, bool>> where, Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> setPropertyCalls)
        {
            var result = context.Set<T>().AsQueryable();
            return await result.Where(where).ExecuteUpdateAsync(setPropertyCalls);
        }

        public virtual async Task<int> UpdateAllAsync(Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> setPropertyCalls)
        {
            var result = context.Set<T>().AsQueryable();
            return await result.ExecuteUpdateAsync(setPropertyCalls);
        }

        public virtual void Delete(T entity) => context.Set<T>().Remove(entity);

        public virtual void Delete(IEnumerable<T> entities) => context.Set<T>().RemoveRange(entities);

        public virtual void Delete(Expression<Func<T, bool>> where) => Delete(context.Set<T>().Where<T>(where).AsEnumerable());

        public virtual async Task<int> ExecuteDeleteAsync(Expression<Func<T, bool>> where)
        {
            var result = context.Set<T>().AsQueryable();
            return await result.Where(where).ExecuteDeleteAsync();
        }

        public virtual async Task<T?> GetByIdAsync(object id) => await context.Set<T>().FindAsync(id);
        public virtual async Task<T?> GetAsync(Expression<Func<T, bool>> where, Func<IQueryable<T>, IIncludableQueryable<T, object?>>? include = null, Expression<Func<T, T>>? selector = null, bool noTracking = false)
        {
            var result = context.Set<T>().AsQueryable();
            if (include != null) { result = include(result); }
            if (selector != null) { result = result.Select(selector); }
            if (noTracking) { result = result.AsNoTracking(); }
            return await result.FirstOrDefaultAsync(where);
        }

        public virtual async Task<T?> GetSortedAsync(Expression<Func<T, bool>> where, Expression<Func<T, object>> orderBy, Func<IQueryable<T>, IIncludableQueryable<T, object?>>? include = null, Expression<Func<T, T>>? selector = null, bool noTracking = false, bool sortByDesc = false)
        {
            var result = context.Set<T>().AsQueryable();
            result = result.Where(where);
            result = sortByDesc ? result.OrderByDescending(orderBy) : result.OrderBy(orderBy);
            result = result.Take(1);
            if (include != null) { result = include(result); }
            if (selector != null) { result = result.Select(selector); }
            if (noTracking) { result = result.AsNoTracking(); }
            return await result.FirstOrDefaultAsync();
        }

        public virtual IEnumerable<T> GetMany(Expression<Func<T, bool>> where, Func<IQueryable<T>, IIncludableQueryable<T, object?>>? include = null, Expression<Func<T, T>>? selector = null, bool noTracking = false)
        {
            var result = context.Set<T>().AsQueryable();
            if (include != null) { result = include(result); }
            if (selector != null) { result = result.Select(selector); }
            if (noTracking) { result = result.AsNoTracking(); }
            return result.Where(where).ToList();
        }

        public virtual async Task<IEnumerable<T>> GetManyAsync(Expression<Func<T, bool>> where, Func<IQueryable<T>, IIncludableQueryable<T, object?>>? include = null, Expression<Func<T, T>>? selector = null, bool noTracking = false)
        {
            var result = context.Set<T>().AsQueryable();
            if (include != null) { result = include(result); }
            if (selector != null) { result = result.Select(selector); }
            if (noTracking) { result = result.AsNoTracking(); }
            return await result.Where(where).ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> GetManySortedAsync(Expression<Func<T, bool>> where, Expression<Func<T, T>> orderBy, int take, Func<IQueryable<T>, IIncludableQueryable<T, object?>>? include = null, Expression<Func<T, T>>? selector = null, bool noTracking = false, bool sortByDesc = false)
        {
            var result = context.Set<T>().AsQueryable();
            result = result.Where(where);
            result = sortByDesc ? result.OrderByDescending(orderBy) : result.OrderBy(orderBy);
            result = result.Take(take);
            if (include != null) { result = include(result); }
            if (selector != null) { result = result.Select(selector); }
            if (noTracking) { result = result.AsNoTracking(); }
            return await result.ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync(Func<IQueryable<T>, IIncludableQueryable<T, object?>>? include = null, Expression<Func<T, T>>? selector = null, bool noTracking = false)
        {
            var result = context.Set<T>().AsQueryable();
            if (include != null) { result = include(result); }
            if (selector != null) { result = result.Select(selector); }
            if (noTracking) { result = result.AsNoTracking(); }
            return await result.ToListAsync();
        }

        public virtual async Task<bool> EntityExistAsync(Expression<Func<T, bool>> where) => await context.Set<T>().AsNoTracking().AnyAsync(where);

        public virtual async Task<long> CountAsync(Expression<Func<T, bool>> where) => await context.Set<T>().Where(where).AsNoTracking().LongCountAsync();

        public virtual async Task<IEnumerable<T>> GetManyPagedAsync(Expression<Func<T, bool>> where, Expression<Func<T, object>> orderBy, Func<IQueryable<T>, IIncludableQueryable<T, object?>>? include = null, Expression<Func<T, T>>? selector = null,
        int? pageNumber = null,
        int? pageSize = null,
        bool sortByDesc = false,
        bool noTracking = false)
        {
            var result = context.Set<T>().AsQueryable();
            result = result.Where(where);
            result = sortByDesc ? result.OrderByDescending(orderBy) : result.OrderBy(orderBy);
            if (include != null) { result = include(result); }
            if (selector != null) { result = result.Select(selector); }
            if (noTracking) { result = result.AsNoTracking(); }
            if (pageNumber.HasValue && pageSize.HasValue && pageNumber > 0 && pageSize > 0)
            {
                int skip = (pageNumber.Value - 1) * pageSize.Value;
                result = result.Skip(skip).Take(pageSize.Value);
            }
            return await result.ToListAsync();
        }

        /// <summary>
        /// Use it to detach/undo all changes made in EF context and not committed to database yet.<br/>
        /// Useful when error occurs while commit to database. Catch error and use detach to undo all changes to the entities.<br/>
        /// Ex: try { <br/>
        ///     _context.table.Add(entity); <br/>
        ///     _context.SaveChanges();  <br/>
        /// } <br/>
        /// catch { <br/>
        ///     _context.DetachChanges();  <br/>
        ///     _context.table.Add(another entity);<br/>
        ///     _context.SaveChanges();<br/>
        /// }<br/>
        /// </summary>
        public virtual void DetachChanges()
        {
            // Detach all entries that have been modified or added
            var entries = context.ChangeTracker.Entries<T>()
                .Where(e => e.State == EntityState.Modified || e.State == EntityState.Added)
                .ToList();

            foreach (var entry in entries)
            {
                entry.State = EntityState.Detached;
            }
        }

        #region Developer Notes

        #region No Tracking

        /*
         * 
         * By default entity framework will register the each data fetched from database to the change tracker.
         * But if we fetching data only for read-only purpose and not going to make any updates to it,
         * its not necessary to register for change tracking. To let EF know that the fetch doesn't need tracking,
         * we can call the DbSet with AsNoTracking().
         * 
         * So if any table data fetching only for read-only purpose use "noTracking: true" to improve performance.         
         * 
         */

        #endregion No Tracking

        #region Query Project or Selector

        /*
         * 
         * When write DB fetch statements using EF, the LINQ expression converted to SQL query.
         * In such case the SQL query computed by EF for LINQ to SQL does include all columns of all joined tables.
         * But its not necessary to fetch all columns when we require only few. This does impact on performance by 
         * reading all unncessary data from database.
         * 
         * To improve it, we can use EF projections to read only necessary columns. Here projections meant to use LINQ Select() to mention only fields required.
         * 
         * Example:
         * 
         * To get all BandId & Description of a companygroup for specific award types, we write EF LINQ statement as,
         * 
         * _UnitOfWork.CompanyGroupBands.GetManyAsync(x => x.CompanyGroupAwardType.CompanyGroupId == 1346 && [1,20].Contains(x.CompanyGroupAwardType.AwardTypeId), x => x.Include(y => y.CompanyGroupAwardType).Include(y => y.Band))
         * 
         * This will get converted to below SQL statement,
         * 
         * exec sp_executesql N'SELECT [c].[CompanyGroupBandId], [c].[BandId], [c].[CGBandName], [c].[CompanyGroupAwardTypeId], [c].[CreatedById], [c].[CreatedDate], [c].[HideInAwardeeNews], [c].[HideinSocialPage], [c].[InActive], [c].[IsDefaultBand], [c].[IsSingleRedemption], [c].[NotSendAutoGenerateAwards], [c].[SourceCode], [c].[TotalSecondaryGiftLevels], [c].[UpdatedById], [c].[UpdatedDate], [c].[Validity], [c0].[CompanyGroupAwardTypeId], [c0].[ApprovalTypeId], [c0].[AwardTypeId], [c0].[CompanyGroupAwardTypeName], [c0].[CompanyGroupId], [c0].[CreatedById], [c0].[CreatedDate], [c0].[DisplayOrder], [c0].[HideAutoNotification], [c0].[UpdatedById], [c0].[UpdatedDate], [b].[BandId], [b].[AddPointsToAccount], [b].[AwardTypeId], [b].[BandDescription], [b].[BandPrice], [b].[CreatedById], [b].[CreatedDate], [b].[CurrencyId], [b].[InterCompanyBand], [b].[InvoiceBandPrice], [b].[ServiceYear], [b].[UpdatedById], [b].[UpdatedDate], [b].[USYearsofService]
            FROM [CompanyGroupBand] AS [c]
            INNER JOIN [CompanyGroupAwardType] AS [c0] ON [c].[CompanyGroupAwardTypeId] = [c0].[CompanyGroupAwardTypeId]
            INNER JOIN [Band] AS [b] ON [c].[BandId] = [b].[BandId]
            WHERE [c0].[CompanyGroupId] = @__cgId_0 AND CAST([c0].[AwardTypeId] AS int) IN (1, 20)',N'@__cgId_0 int',@__cgId_0=1346
         * 
         * Note that, above statement includes all columns from all tables joined. Simply SELECT * FROM...
         * 
         * Now to optimize we can write projection as below,

            // Define the projection selector using a LINQ expression
            Expression<Func<CompanyGroupBand, CompanyGroupBand>> projection = cgBand => new CompanyGroupBand
            {
                Band = new Band
                {
                    BandId = cgBand.Band.BandId,
                    BandDescription = cgBand.Band.BandDescription
                },
                CompanyGroupAwardType = new CompanyGroupAwardType
                {
                    CompanyGroupId = cgBand.CompanyGroupAwardType.CompanyGroupId,
                    AwardTypeId = cgBand.CompanyGroupAwardType.AwardTypeId
                }
            };

            _UnitOfWork.CompanyGroupBands.GetManyAsync(x => x.CompanyGroupAwardType.CompanyGroupId == 1346 && [1,20].Contains(x.CompanyGroupAwardType.AwardTypeId), x => x.Include(y => y.CompanyGroupAwardType).Include(y => y.Band), selector: projection);
         *
         * The converted SQL statement for above projected statement is,
         * exec sp_executesql N'SELECT [b].[BandId], [b].[BandDescription], [c0].[CompanyGroupId], [c0].[AwardTypeId]
            FROM [CompanyGroupBand] AS [c]
            INNER JOIN [CompanyGroupAwardType] AS [c0] ON [c].[CompanyGroupAwardTypeId] = [c0].[CompanyGroupAwardTypeId]
            INNER JOIN [Band] AS [b] ON [c].[BandId] = [b].[BandId]
            WHERE [c0].[CompanyGroupId] = @__cgId_0 AND CAST([c0].[AwardTypeId] AS int) IN (1, 20)',N'@__cgId_0 int',@__cgId_0=1346
         *
         * Note that, this included only columns required as we mentioned in project.
         * 
         * So with the help of projection we can do optimized SQL reads.
         * 
         * NOTE: Projections must be used only with NoTracking statements.
         */

        #endregion Query Project or Selector

        #endregion Developer Notes
    }
}
