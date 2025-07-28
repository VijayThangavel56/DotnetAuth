using ECommerce.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.BLL.Interface
{
    public interface ICategoryInterface
    {
        Task<ApiResponseDto> GetAllCategoryAsync();
        Task<ApiResponseDto> GetCategoryByIdAsync(int id);
        Task<ApiResponseDto> CreateCategoryAsync(CategoryDto categoryDto);
        Task<ApiResponseDto> UpdateCategoryAsync(int id, CategoryDto categoryDto);
        Task<ApiResponseDto> DeleteCategoryAsync(int id);
    }
}
