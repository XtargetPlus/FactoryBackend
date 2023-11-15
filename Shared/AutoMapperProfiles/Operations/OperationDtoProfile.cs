using AutoMapper;
using DB.Model.TechnologicalProcessInfo;
using Shared.Dto.Operation;

namespace Shared.AutoMapperProfiles.Operations;

public class OperationDtoProfile : Profile
{
    public OperationDtoProfile()
    {
        CreateProjection<Operation, OperationDto>();
    }
}
