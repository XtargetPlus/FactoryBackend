using AutoMapper;
using DB.Model.UserInfo;
using Shared.Dto.TechnologicalProcess.TechProcessItem.Read;
using Shared.Dto.Users;

namespace Shared.AutoMapperProfiles.Users;

public class UserGetProfile : Profile
{
    public UserGetProfile()
    {
        CreateProjection<User, BaseUserGetDto>();

        CreateProjection<User, UserGetForHubDto>()
            .ForMember(dest => dest.Subdivision, opt => opt.MapFrom(src => src.Subdivision.Title))
            .ForMember(dest => dest.Profession, opt => opt.MapFrom(src => src.Profession.Title))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.Title));

        CreateProjection<User, UserGetWithSubdivisionDto>()
            .ForMember(dest => dest.Subdivision, opt => opt.MapFrom(src => src.Subdivision.Title));

        CreateProjection<User, UserGetProfessionDto>()
            .ForMember(dest => dest.Profession, opt => opt.MapFrom(src => src.Profession.Title));

        CreateProjection<User, UserGetDto>()
            .ForMember(dest => dest.Subdivision, opt => opt.MapFrom(src => src.Subdivision.Title))
            .ForMember(dest => dest.Profession, opt => opt.MapFrom(src => src.Profession.Title));

        CreateProjection<User, GetAllDevelopersTasksDto>()
            .ForMember(
                dest => dest.DeveloperId,
                opt => opt.MapFrom(u => u.Id))
            .ForMember(
                dest => dest.Developer,
                opt => opt.MapFrom(u => u.FFL));
    }
}
