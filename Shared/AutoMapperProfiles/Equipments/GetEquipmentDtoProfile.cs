using AutoMapper;
using DB.Model.SubdivisionInfo.EquipmentInfo;
using Shared.Dto.Equipment;

namespace Shared.AutoMapperProfiles.Equipments;

public class GetEquipmentDtoProfile : Profile
{
    public GetEquipmentDtoProfile()
    {
        CreateProjection<Equipment, GetEquipmentDto>()
            .ForMember(dest => dest.Subdivision, opt => opt.MapFrom(equipment => equipment.Subdivision!.Title));
    }
}
