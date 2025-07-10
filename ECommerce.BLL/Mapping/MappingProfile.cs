using AutoMapper;
using ECommerce.Domain.Entities;
using ECommerce.DTO;

namespace ECommerce.BLL.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ApplicationUser, UserResponse>();
            CreateMap<ApplicationUser, CurrentUserResponse>();
            CreateMap<UserRegisterRequst, ApplicationUser>();
        }

    }
}
