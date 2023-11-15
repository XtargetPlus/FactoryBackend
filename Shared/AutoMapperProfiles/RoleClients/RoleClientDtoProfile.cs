using AutoMapper;
using DB.Model.UserInfo;
using Shared.Dto.Role;

namespace Shared.AutoMapperProfiles.RoleClients;

public class RoleClientDtoProfile : Profile
{
    public RoleClientDtoProfile()
    {
        CreateProjection<RoleClient, RoleClientFuncDto>();

        CreateProjection<RoleClient, RoleClientConcreteDto>()
            .ForMember(dest => dest.UserForm, opt => opt.MapFrom(roleClient => roleClient.UserForm!.Title))
            .ForMember(dest => dest.Role, opt => opt.MapFrom(roleClient => roleClient.Role!.Title));
    }
}
