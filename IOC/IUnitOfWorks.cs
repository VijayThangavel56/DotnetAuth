using DotnetAuth.Repository.IRepository;

namespace DotnetAuth.IOC
{
    public interface IUnitOfWorks:IDisposable
    {
        IApplicationUserRepository ApplicationUser { get; }
        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }
}
