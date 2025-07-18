using ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.BLL.Interface
{
    public interface IRoleService
    {
        Task<IEnumerable<ApplicationRole>> GetAllRolesAsync();
        Task<ApplicationRole?> GetRoleByIdAsync(string id);
        Task<bool> CreateRoleAsync(ApplicationRole role);
        Task<bool> UpdateRoleAsync(ApplicationRole role);
        Task<bool> DeleteRoleAsync(string id);
    }

}
