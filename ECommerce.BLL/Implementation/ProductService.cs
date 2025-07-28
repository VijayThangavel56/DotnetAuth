using ECommerce.Shared.Helper;
using ECommerce.DTO;
using ECommerce.Domain.Entities;
using AutoMapper;
using ECommerce.DAL.UnitOfWorks;
using ECommerce.BLL.Interface;
using System.Net;

public class ProductService : IProductInterface
{
    private readonly IUnitOfWorks _unitOfWorks;
    private readonly IMapper _mapper;

    public ProductService(IUnitOfWorks unitOfWorks, IMapper mapper)
    {
        _unitOfWorks = unitOfWorks;
        _mapper = mapper;
    }

    public async Task<ApiResponseDto> CreateProductAsync(ProductDto productDto)
    {
        var product = _mapper.Map<Product>(productDto);
        await _unitOfWorks.Product.AddAsync(product);
        await _unitOfWorks.CommitAsync();
        var result = _mapper.Map<ProductDto>(product);
        return ApiResponseHelper.Created(result);
    }

    public async Task<ApiResponseDto> DeleteProductAsync(int id)
    {
        var existingProduct = await _unitOfWorks.Product.GetByIdAsync(id);
        if (existingProduct == null)
            return ApiResponseHelper.NotFound([new ApiMessage { Message = "Product Not Found" }]);

        _unitOfWorks.Product.Delete(existingProduct);
        await _unitOfWorks.CommitAsync();

        return ApiResponseHelper.Ok();
    }

    public async Task<ApiResponseDto> GetAllProductsAsync()
    {
        var products = await _unitOfWorks.Product.GetAllAsync();
        var result = _mapper.Map<List<ProductDto>>(products);
        return ApiResponseHelper.Ok(result);
    }

    public async Task<ApiResponseDto> GetProductByIdAsync(int id)
    {
        var product = await _unitOfWorks.Product.GetByIdAsync(id);
        if (product == null)
            return ApiResponseHelper.NotFound([new ApiMessage { Code = "404", Message = "Product Not Found" }]);

        var result = _mapper.Map<ProductDto>(product);
        return ApiResponseHelper.Ok(result);
    }

    public async Task<ApiResponseDto> UpdateProductAsync(int id, ProductDto productDto)
    {
        var existingProduct = await _unitOfWorks.Product.GetByIdAsync(id);
        if (existingProduct == null)
            return ApiResponseHelper.NotFound([new ApiMessage { Code = "404", Message = "Product Not Found" }]);

        existingProduct.Name = productDto.Name;
        existingProduct.Description = productDto.Description;
        existingProduct.Price = productDto.Price;
        existingProduct.StockQuantity = productDto.StockQuantity;
        existingProduct.CategoryId = productDto.CategoryId;

        _unitOfWorks.Product.Update(existingProduct);
        await _unitOfWorks.CommitAsync();

        var result = _mapper.Map<ProductDto>(existingProduct);
        return ApiResponseHelper.Ok(result);
    }
}
