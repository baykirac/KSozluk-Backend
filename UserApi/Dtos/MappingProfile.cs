using AutoMapper;
using UserApi.Entities;

namespace UserApi.Dtos;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDto>().
            ForMember(x => x.Password, opt => opt.MapFrom(x => "***")).
            ReverseMap();

        CreateMap<Role, RoleDto>().ReverseMap();
    }
}