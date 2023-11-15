using AutoMapper;
using DB.Model.SubdivisionInfo;
using Shared.Dto.Subdiv;

namespace Shared.AutoMapperProfiles.Subdivisions;

public class SubdivisionGetDtoProfile : Profile
{
    public SubdivisionGetDtoProfile()
    {
        CreateProjection<Subdivision, SubdivisionGetDto>()
            .ForMember(dest => dest.CountChildren, opt => opt.MapFrom(subdivision => subdivision.Subdivisions!.Count));
    }
}
