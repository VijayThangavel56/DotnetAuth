using ECommerce.DAL.Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DAL.UnitOfWorks
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
