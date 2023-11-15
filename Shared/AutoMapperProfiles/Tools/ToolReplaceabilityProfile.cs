using AutoMapper;
using DB.Model.ToolInfo;
using Shared.Dto.Tools;

namespace Shared.AutoMapperProfiles.Tools;

public class ToolReplaceabilityProfile : Profile
{
    public ToolReplaceabilityProfile()
    {
        CreateProjection<ToolReplaceability, GetToolReplaceabilityDto>()
            .ForMember(
                desc => desc.Id,
                opt => opt.MapFrom(
                    replaceability => replaceability.Slave!.Id))
            .ForMember(desc => desc.Title,
                opt => opt.MapFrom(replaceability => replaceability.Slave!.Title
                    ))
            .ForMember(
                desc => desc.SerialNumber,
                opt => opt.MapFrom(
                    replaceability => replaceability.Slave!.SerialNumber))
            .ForMember(
                desc => desc.Note,
            opt => opt.MapFrom(
                replaceability => replaceability.Slave!.Note));
    }
}