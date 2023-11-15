using AutoMapper;
using DB.Model.SubdivisionInfo.EquipmentInfo;
using Shared.Dto.TechnologicalProcess;

namespace Shared.AutoMapperProfiles.TechProcesses;

public class GetEquipmentOperationDtoProfile : Profile
{
    public GetEquipmentOperationDtoProfile()
    {
        CreateProjection<EquipmentOperation, GetEquipmentOperationDto>()
            .ForMember(
                dest => dest.EquipmentOperationId,
                opt => opt.MapFrom(eo => eo.Id))
            .ForMember(
                dest => dest.EquipmentSerialNumber, 
                opt => opt.MapFrom(eo => eo.Equipment!.SerialNumber))
            .ForMember(
                dest => dest.EquipmentTitle, 
                opt => opt.MapFrom(eo => eo.Equipment!.Title))
            .ForMember(
                dest => dest.Subdivision,
                opt => opt.MapFrom(eo => eo.Equipment!.Subdivision!.Title));
    }
}
