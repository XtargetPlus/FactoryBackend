using AutoMapper;
using DB.Model.UserInfo;
using Shared.Dto.Users;

namespace Shared.AutoMapperProfiles.Users;

public class UserDtoProfile : Profile
{
    public UserDtoProfile()
    {
        CreateProjection<User, UserDto>()
            .ForMember(dest => dest.Password, opt => opt.MapFrom(user => user.Password ?? ""))
            .ForMember(dest => dest.Profession, opt => opt.MapFrom(user => user.Profession.Title))
            .ForMember(dest => dest.Subdivision, opt => opt.MapFrom(user => user.Subdivision.Title))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(user => user.Status.Title));
    }
}
