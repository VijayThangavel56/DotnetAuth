using ECommerce.DAL.Repositories.IRepository;
using ECommerce.Domain.Entities;
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
        IRepositoryBase<Product> Product { get; }
        IRepositoryBase<Category> Category { get; }
        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }
}
