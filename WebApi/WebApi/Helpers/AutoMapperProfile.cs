using AutoMapper;
using WebApi.DataAccessLayer.Entities;
using WebApi.Models.UIModels;

namespace WebApi.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserModel, UserDto>();
            CreateMap<UserDto, UserModel>();

            CreateMap<Role, RoleUIModel>();
            CreateMap<RoleUIModel, Role>();
        }
    }
}