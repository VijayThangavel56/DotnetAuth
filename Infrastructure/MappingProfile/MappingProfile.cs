﻿using AutoMapper;
using DotnetAuth.Domain.Contracts;
using DotnetAuth.Domain.Entities;

namespace DotnetAuth.Infrastructure.MappingProfile
{
    public class MappingProfile:Profile
    {
        public MappingProfile() {
            CreateMap<ApplicationUser, UserResponse>();
            CreateMap<ApplicationUser, CurrentUserResponse>();
            CreateMap<UserRegisterRequst, ApplicationUser>();
        }

    }
}
