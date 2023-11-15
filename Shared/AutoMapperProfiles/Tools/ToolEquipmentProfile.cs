using AutoMapper;
using DB.Model.ToolInfo;
using Shared.Dto.Tools;

namespace Shared.AutoMapperProfiles.Tools;

public class ToolEquipmentProfile : Profile
{
    public ToolEquipmentProfile()
    {
        CreateProjection<EquipmentTool, GetToolEquipmentDto>()
            .ForMember(
                et => et.Id,
                opt => opt.MapFrom(te => te.EquipmentId))
            .ForMember(
                et => et.Title,
                opt => opt.MapFrom(
                    et => et.Equipment!.Title))
            .ForMember(et => et.SerialNumber,
                opt => opt.MapFrom(
                    et => et.Equipment!.SerialNumber));
    }
}