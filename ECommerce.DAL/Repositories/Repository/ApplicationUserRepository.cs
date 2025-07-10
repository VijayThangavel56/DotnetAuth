using ECommerce.DAL.Data;
using ECommerce.DAL.Repositories.IRepository;
using ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DAL.Repositories.Repository
{
    public class ApplicationUserRepository : RepositoryBase<ApplicationUser>, IApplicationUserRepository
    {
        public ApplicationUserRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
