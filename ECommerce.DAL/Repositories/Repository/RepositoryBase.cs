using ECommerce.DAL.Data;
using ECommerce.DAL.Repositories.IRepository;
using ECommerce.DTO;
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

        public async Task AddAsync(T entity) =>await context.Set<T>().AddAsync(entity);

        public async Task AddRangeAsync(IEnumerable<T> entities) => await context.Set<T>().AddRangeAsync(entities);

        public async Task<long> CountAsync(Expression<Func<T, bool>> where) => await context.Set<T>().Where(where).AsNoTracking().LongCountAsync();

        public void Delete(T entity) => context.Set<T>().Remove(entity);

        public void DeleteRange(IEnumerable<T> entities) => context.Set<T>().RemoveRange(entities);

        public void DeleteWhere(Expression<Func<T, bool>> where) => context.Set<T>().RemoveRange(context.Set<T>().Where(where));

        public void DetachChanges()
        {
            throw new NotImplementedException();
        }

        public Task<bool> EntityExistAsync(Expression<Func<T, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<T>> GetAllAsync(Func<IQueryable<T>, IIncludableQueryable<T, object?>>? include = null, Expression<Func<T, T>>? selector = null, Expression<Func<T, object>>? orderBy = null, bool noTracking = false)
        {
            throw new NotImplementedException();
        }

        public Task<T?> GetAsync(Expression<Func<T, bool>> where, Func<IQueryable<T>, IIncludableQueryable<T, object?>>? include = null, Expression<Func<T, T>>? selector = null, Expression<Func<T, object>>? orderBy = null, bool noTracking = false)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<T>> GetManyAsync(Expression<Func<T, bool>> where, Func<IQueryable<T>, IIncludableQueryable<T, object?>>? include = null, Expression<Func<T, T>>? selector = null, Expression<Func<T, object>>? orderBy = null, bool noTracking = false)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResult<T>> GetPagedAsync(Expression<Func<T, bool>>? where = null, Func<IQueryable<T>, IIncludableQueryable<T, object?>>? include = null, Expression<Func<T, T>>? selector = null, Expression<Func<T, object>>? orderBy = null, bool noTracking = false, int pageNumber = 1, int pageSize = 10)
        {
            throw new NotImplementedException();
        }

        public void Update(T entity)
        {
            throw new NotImplementedException();
        }

        public void UpdateRange(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }
    }
}
