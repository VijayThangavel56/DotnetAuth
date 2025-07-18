using ECommerce.BLL.Interface;
using ECommerce.Domain.Entities;
using ECommerce.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.BLL.Implementation
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        public RoleService(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<bool> CreateRoleAsync(ApplicationRole role)
        {
            if (role == null || string.IsNullOrWhiteSpace(role.Name)) return false;  
            var exist = await _roleManager.RoleExistsAsync(role.Name);
            if (exist) return false;  
            var result = await _roleManager.CreateAsync(role);
            return result.Succeeded;  
        }


        public async Task<bool> DeleteRoleAsync(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null) return false;

            var result = await _roleManager.DeleteAsync(role);
            return result.Succeeded;
        }

        public async Task<IEnumerable<ApplicationRole>> GetAllRolesAsync()
        {
            return await _roleManager.Roles.ToListAsync();
        }

        public async Task<ApplicationRole?> GetRoleByIdAsync(string id)
        {
             return await _roleManager.FindByIdAsync(id);
        }

        public async Task<bool> UpdateRoleAsync(ApplicationRole updatedRole)
        {
            var existingRole = await _roleManager.FindByIdAsync(updatedRole.Id);
            if (existingRole == null) return false;

            existingRole.Name = updatedRole.Name;
 
            var result = await _roleManager.UpdateAsync(existingRole);
            return result.Succeeded;
        }

    }
}
