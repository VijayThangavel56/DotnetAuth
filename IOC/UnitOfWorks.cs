using DotnetAuth.Infrastructure.Context;
using DotnetAuth.Repository;
using DotnetAuth.Repository.IRepository;
using Microsoft.EntityFrameworkCore.Storage;

namespace DotnetAuth.IOC
{
    /// <summary>
    /// Handles unit of work operations including transaction management and repository access.
    /// </summary>
    public class UnitOfWorks : IUnitOfWorks, IDisposable
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction? _transaction;
        private bool _disposed;


        // Private repo instances
        private IApplicationUserRepository? _applicationUser;
        public UnitOfWorks(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public IApplicationUserRepository ApplicationUser =>
            _applicationUser ??= new ApplicationUserRepository(_context);


        /// <summary>
        /// Begins a new database transaction.
        /// </summary>
        public async Task BeginTransactionAsync()
        {
            if (_transaction == null)
            {
                _transaction = await _context.Database.BeginTransactionAsync();
            }
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
