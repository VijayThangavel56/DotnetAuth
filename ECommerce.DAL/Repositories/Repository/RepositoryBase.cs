using ECommerce.DAL.Data;
using ECommerce.DAL.Repositories.IRepository;
using ECommerce.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace ECommerce.DAL.Repositories.Repository
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;

        public RepositoryBase(ApplicationDbContext dbContext)
        {
            _context = dbContext;
            _dbSet = _context.Set<T>();
        }

        #region Add Methods
        public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);
        public async Task AddRangeAsync(IEnumerable<T> entities) => await _dbSet.AddRangeAsync(entities);
        #endregion

        #region Update Methods
        public void Update(T entity) => _dbSet.Update(entity);
        public void UpdateRange(IEnumerable<T> entities) => _dbSet.UpdateRange(entities);
        #endregion

       public async Task<T?> GetByIdAsync(object id)
        {
            return await _dbSet.FindAsync(id).AsTask();
        }

        #region Delete Methods
        public void Delete(T entity) => _dbSet.Remove(entity);
        public void DeleteRange(IEnumerable<T> entities) => _dbSet.RemoveRange(entities);
        public void DeleteWhere(Expression<Func<T, bool>> where) => _dbSet.RemoveRange(_dbSet.Where(where));
        #endregion

        #region Utility Methods
        public async Task<long> CountAsync(Expression<Func<T, bool>> where)
            => await _dbSet.Where(where).AsNoTracking().LongCountAsync();

        public Task<bool> EntityExistAsync(Expression<Func<T, bool>> where)
            => _dbSet.AnyAsync(where);

        public void DetachChanges()
        {
            var entries = _context.ChangeTracker.Entries<T>()
                .Where(e => e.State == EntityState.Modified || e.State == EntityState.Added)
                .ToList();

            foreach (var entry in entries)
                entry.State = EntityState.Detached;
        }
        #endregion

        #region Query Methods

        private IQueryable<T> BuildQuery(
            Expression<Func<T, bool>>? where,
            Func<IQueryable<T>, IIncludableQueryable<T, object?>>? include,
            Expression<Func<T, T>>? selector,
            Expression<Func<T, object>>? orderBy,
            bool noTracking)
        {
            var query = _dbSet.AsQueryable();

            if (where is not null) query = query.Where(where);
            if (include is not null) query = include(query);
            if (selector is not null) query = query.Select(selector);
            if (orderBy is not null) query = query.OrderBy(orderBy);
            if (noTracking) query = query.AsNoTracking();

            return query;
        }

        public async Task<ICollection<T>> GetAllAsync(
            Func<IQueryable<T>, IIncludableQueryable<T, object?>>? include = null,
            Expression<Func<T, T>>? selector = null,
            Expression<Func<T, object>>? orderBy = null,
            bool noTracking = false)
        {
            var query = BuildQuery(null, include, selector, orderBy, noTracking);
            return await query.ToListAsync();
        }

        public async Task<T?> GetAsync(
            Expression<Func<T, bool>> where,
            Func<IQueryable<T>, IIncludableQueryable<T, object?>>? include = null,
            Expression<Func<T, T>>? selector = null,
            Expression<Func<T, object>>? orderBy = null,
            bool noTracking = false)
        {
            var query = BuildQuery(where, include, selector, orderBy, noTracking);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<ICollection<T>> GetManyAsync(
            Expression<Func<T, bool>> where,
            Func<IQueryable<T>, IIncludableQueryable<T, object?>>? include = null,
            Expression<Func<T, T>>? selector = null,
            Expression<Func<T, object>>? orderBy = null,
            bool noTracking = false)
        {
            var query = BuildQuery(where, include, selector, orderBy, noTracking);
            return await query.ToListAsync();
        }

        public async Task<PagedResult<T>> GetPagedAsync(
            Expression<Func<T, bool>>? where = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object?>>? include = null,
            Expression<Func<T, T>>? selector = null,
            Expression<Func<T, object>>? orderBy = null,
            bool noTracking = false,
            int pageNumber = 1,
            int pageSize = 10)
        {
            var query = BuildQuery(where, include, selector, orderBy, noTracking);

            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize < 1 ? 10 : pageSize;

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<T>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
        #endregion
    }
}
