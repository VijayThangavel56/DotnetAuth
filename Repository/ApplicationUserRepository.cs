using DotnetAuth.Domain.Entities;
using DotnetAuth.Infrastructure.Context;
using DotnetAuth.Repository.IRepository;

namespace DotnetAuth.Repository
{
    public class ApplicationUserRepository:RepositoryBase<ApplicationUser>, IApplicationUserRepository
    {
        public ApplicationUserRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
