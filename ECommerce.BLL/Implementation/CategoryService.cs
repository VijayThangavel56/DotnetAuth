using AutoMapper;
using ECommerce.BLL.Interface;
using ECommerce.DAL.UnitOfWorks;
using ECommerce.Domain.Entities;
using ECommerce.DTO;
using ECommerce.Shared.Helper;

namespace ECommerce.BLL.Implementation
{
    public class CategoryService : ICategoryInterface
    {
        private readonly IUnitOfWorks _unitOfWorks;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWorks unitOfWorks, IMapper mapper)
        {
            _unitOfWorks = unitOfWorks;
            _mapper = mapper;
        }

        public async Task<ApiResponseDto> CreateCategoryAsync(CategoryDto categoryDto)
        {
            var categoryEntity = _mapper.Map<Category>(categoryDto);

            await _unitOfWorks.Category.AddAsync(categoryEntity);
            await _unitOfWorks.CommitAsync();

            var result = _mapper.Map<CategoryDto>(categoryEntity);
            return ApiResponseHelper.Created(result);
        }

        public async Task<ApiResponseDto> DeleteCategoryAsync(int id)
        {
            var existingCategory = await _unitOfWorks.Category.GetByIdAsync(id);
            if (existingCategory == null)
                return ApiResponseHelper.NotFound([new ApiMessage { Code = "404", Message = "Category Not Found" }]);

            _unitOfWorks.Category.Delete(existingCategory);
            await _unitOfWorks.CommitAsync(); // ❗ You missed this in your code.

            return ApiResponseHelper.Ok();
        }

        public async Task<ApiResponseDto> GetAllCategoryAsync()
        {
            var categories = await _unitOfWorks.Category.GetAllAsync();
            var result = _mapper.Map<List<CategoryDto>>(categories);

            return ApiResponseHelper.Ok(result);
        }

        public async Task<ApiResponseDto> GetCategoryByIdAsync(int id)
        {
            var category = await _unitOfWorks.Category.GetByIdAsync(id);
            if (category == null)
                return ApiResponseHelper.NotFound([new ApiMessage { Code = "404", Message = "Category Not Found" }]);

            var result = _mapper.Map<CategoryDto>(category);
            return ApiResponseHelper.Ok(result);
        }

        public async Task<ApiResponseDto> UpdateCategoryAsync(int id, CategoryDto categoryDto)
        {
            var existingCategory = await _unitOfWorks.Category.GetByIdAsync(id);
            if (existingCategory == null)
                return ApiResponseHelper.NotFound([new ApiMessage { Code = "404", Message = "Category Not Found" }]);

            _mapper.Map(categoryDto, existingCategory);

            _unitOfWorks.Category.Update(existingCategory);
            await _unitOfWorks.CommitAsync();

            var result = _mapper.Map<CategoryDto>(existingCategory);
            return ApiResponseHelper.Ok(result);
        }
    }
}
