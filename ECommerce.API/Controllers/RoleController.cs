using ECommerce.BLL.Interface;
using ECommerce.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize]
    public class RoleController : BaseController
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        [Route("get-all")]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _roleService.GetAllRolesAsync();
            return Ok(new { success = true, data = roles });
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetRoleById(string id)
        {
            var role = await _roleService.GetRoleByIdAsync(id);
            if (role == null)
                return NotFound(new { success = false, message = "Role not found." });

            return Ok(new { success = true, data = role });
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateRole([FromBody] ApplicationRole role)
        {
            var result = await _roleService.CreateRoleAsync(role);
            if (!result)
                return BadRequest(new { success = false, message = "Role already exists or invalid input." });

            return Ok(new { success = true, message = "Role created successfully." });
        }

        [HttpPut]
        [Route("{id}/update")]
        public async Task<IActionResult> UpdateRole(string id, [FromBody] ApplicationRole updatedRole)
        {
            updatedRole.Id = id;
            var result = await _roleService.UpdateRoleAsync(updatedRole);
            if (!result)
                return NotFound(new { success = false, message = "Role not found." });

            return Ok(new { success = true, message = "Role updated successfully." });
        }

        [HttpDelete]
        [Route("{id}/delete")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var result = await _roleService.DeleteRoleAsync(id);
            if (!result)
                return NotFound(new { success = false, message = "Role not found." });

            return Ok(new { success = true, message = "Role deleted successfully." });
        }
    }
}
