using ECommerce.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.BLL.Interface
{
    public interface IProductInterface
    {
        Task<ApiResponseDto> GetAllProductsAsync();
        Task<ApiResponseDto> GetProductByIdAsync(int id);
        Task<ApiResponseDto> CreateProductAsync(ProductDto productDto);
        Task<ApiResponseDto> UpdateProductAsync(int id, ProductDto productDto);
        Task<ApiResponseDto> DeleteProductAsync(int id);

    }
}
