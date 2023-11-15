using AutoMapper;
using DB.Model.ToolInfo;
using Shared.Dto.Tools;

namespace Shared.AutoMapperProfiles.Tools;

public class ToolChildDtoProfile : Profile
{
    public ToolChildDtoProfile()
    {
        CreateProjection<ToolChild, GetToolChildDto>()
            .ForMember(
                desc => desc.Id, 
                opt => opt.MapFrom(
                    child => child.ChildId))
            .ForMember(
                desc => desc.Title,
                opt => opt.MapFrom(
                    child => child.Child!.Title))
            .ForMember(
                desc => desc.Serial,
                opt => opt.MapFrom(
                    child => child.Child!.SerialNumber))
            .ForMember(
                desc => desc.Note,
                opt => opt.MapFrom(
                    child => child.Child!.Note));

        CreateProjection<ToolChild, GetToolFatherDto>()
            .ForMember(
                desc => desc.Id, 
                opt => opt.MapFrom(
                    father => father.FatherId))
            .ForMember(
                desc => desc.Title,
                opt => opt.MapFrom(
                    father => father.Father!.Title))
            .ForMember(
                desc => desc.SerialNumber,
                opt => opt.MapFrom(
                    father => father.Father!.SerialNumber))
            .ForMember(
                desc => desc.Note,
                opt => opt.MapFrom(
                    father => father.Father!.Note));
    }
}