using AutoMapper;
using DB.Model.SubdivisionInfo;
using Shared.Dto.Subdiv;

namespace Shared.AutoMapperProfiles.Subdivisions;

public class SubdivisionDtoProfile : Profile
{
    public SubdivisionDtoProfile()
    {
        CreateProjection<Subdivision, SubdivisionDto>();
    }
}
