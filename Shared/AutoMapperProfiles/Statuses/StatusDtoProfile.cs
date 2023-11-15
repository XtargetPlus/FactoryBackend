using AutoMapper;
using DB.Model.StatusInfo;
using Shared.Dto.Status;

namespace Shared.AutoMapperProfiles.Statuses;

public class StatusDtoProfile : Profile
{
    public StatusDtoProfile()
    {
        CreateProjection<Status, StatusDto>();
    }
}
