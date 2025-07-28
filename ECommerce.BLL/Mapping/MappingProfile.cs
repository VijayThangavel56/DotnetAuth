using AutoMapper;
using ECommerce.Domain.Entities;
using ECommerce.DTO;

namespace ECommerce.BLL.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            ProductMap();
            AuthMap();
        }

        public void AuthMap()
        {
            CreateMap<ApplicationUser, UserResponse>();
            CreateMap<ApplicationUser, CurrentUserResponse>();
            CreateMap<UserRegisterRequst, ApplicationUser>();
        }
        public void ProductMap()
        {
            CreateMap<Product, ProductDto>();
            CreateMap<ProductDto, ProductDto>();
        }

    }
}
