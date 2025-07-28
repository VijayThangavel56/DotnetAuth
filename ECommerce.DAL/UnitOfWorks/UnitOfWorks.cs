using ECommerce.DAL.Data;
using ECommerce.DAL.Repositories.IRepository;
using ECommerce.DAL.Repositories.Repository;
using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore.Storage;

namespace ECommerce.DAL.UnitOfWorks
{
    public class UnitOfWorks(ApplicationDbContext context) : IUnitOfWorks, IDisposable
    {

        /// <summary>
        /// Handles unit of work operations including transaction management and repository access.
        /// </summary>
        private readonly ApplicationDbContext _context = context;
        private IDbContextTransaction? _transaction;
        private bool _disposed;


        // Private repo instances
        private IApplicationUserRepository? _applicationUser;
        IRepositoryBase<Product>? _productRepository;
        IRepositoryBase<Category>? _categoryRepository;


        /// <inheritdoc />
        public IApplicationUserRepository ApplicationUser =>
            _applicationUser ??= new ApplicationUserRepository(_context);
        public IRepositoryBase<Product> Product =>
            _productRepository ??= new RepositoryBase<Product>(_context);

        public IRepositoryBase<Category> Category =>
            _categoryRepository ??= new RepositoryBase<Category>(_context);
        /// <summary>
        /// Begins a new database transaction.
        /// </summary>
        public async Task BeginTransactionAsync()
        {
            _transaction ??= await _context.Database.BeginTransactionAsync();
        }

        /// <summary>
        /// Commits the current transaction. Rolls back if an exception occurs.
        /// </summary>
        public async Task CommitAsync()
        {
            try
            {
                if (_transaction != null)
                {
                    await _transaction.CommitAsync();
                }
            }
            catch
            {
                if (_transaction != null)
                {
                    await _transaction.RollbackAsync();
                }
                throw;
            }
            finally
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
        }

        /// <summary>
        /// Rolls back the current transaction.
        /// </summary>
        public async Task RollbackAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        /// <summary>
        /// Saves all pending changes to the database.
        /// </summary>
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Disposes the context and transaction.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Protected dispose method to clean up resources.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _transaction?.Dispose();
                    _context?.Dispose();
                }
                _disposed = true;
            }
        }
    }
}
