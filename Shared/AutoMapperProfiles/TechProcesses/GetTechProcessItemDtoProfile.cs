using AutoMapper;
using DB.Model.TechnologicalProcessInfo;
using Shared.Dto.Graph.Read;
using Shared.Dto.Graph.Read.BranchesItems;
using Shared.Dto.TechnologicalProcess;

namespace Shared.AutoMapperProfiles.TechProcesses;

public class GetTechProcessItemDtoProfile : Profile
{
    public GetTechProcessItemDtoProfile()
    {
        // Projections

        CreateProjection<TechnologicalProcessItem, GetTechProcessItemDto>()
            .ForMember(dest => dest.ShortName, opt => opt.MapFrom(tpi => tpi.Operation!.ShortName))
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(tpi => tpi.Operation!.FullName));

        CreateProjection<TechnologicalProcessItem, GetAllToAddToEndDto>()
            .ForMember(
                dest => dest.TechProcessItemId,
                opt => opt.MapFrom(tpi => tpi.Id))
            .ForMember(
                dest => dest.ShortName,
                opt => opt.MapFrom(tpi => tpi.Operation!.ShortName))
            .ForMember(
                dest => dest.FullName,
                opt => opt.MapFrom(tpi => tpi.Operation!.FullName));

        CreateProjection<TechnologicalProcessItem, BranchItemDto>()
            .ForMember(
                dest => dest.TechProcessItemId,
                opt => opt.MapFrom(tpi => tpi.Id))
            .ForMember(
                dest => dest.ShortName,
                opt => opt.MapFrom(tpi => tpi.Operation!.ShortName))
            .ForMember(
                dest => dest.FullName,
                opt => opt.MapFrom(tpi => tpi.Operation!.FullName));

        // Maps

        CreateMap<BranchItemDto, GetAllToAddToEndDto>();
    }
}
