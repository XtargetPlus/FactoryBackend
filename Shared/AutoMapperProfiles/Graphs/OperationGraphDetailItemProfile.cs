using AutoMapper;
using DB.Model.StorageInfo.Graph;
using Shared.Dto.Graph.Read;
using Shared.Dto.Graph.Read.Open;

namespace Shared.AutoMapperProfiles.Graphs;

public class OperationGraphDetailItemProfile : Profile
{
    public OperationGraphDetailItemProfile()
    {
        // Projections

        CreateProjection<OperationGraphDetailItem, InterimGraphDetailItemDto>()
            .ForMember(
                dest => dest.GraphDetailId,
                opt => opt.MapFrom(i => i.OperationGraphDetailId))
            .ForMember(
                dest => dest.GraphDetailItemId,
                opt => opt.MapFrom(i => i.Id))
            .ForMember(
                dest => dest.ItemId,
                opt => opt.MapFrom(i => i.TechnologicalProcessItemId))
            .ForMember(
                dest => dest.ItemNumber,
                opt => opt.MapFrom(i => i.TechnologicalProcessItem!.OperationNumber))
            .ForMember(
                dest => dest.PositionNumber,
                opt => opt.MapFrom(i => i.OrdinalNumber))
            .ForMember(
                dest => dest.Priority,
                opt => opt.MapFrom(i => i.TechnologicalProcessItem!.Priority))
            .ForMember(
            dest => dest.IsCompleted,
            opt => opt.MapFrom(i => i.OperationGraphDetail!.CountInStream == i.FactCount));

        // Maps

        CreateMap<InterimGraphDetailItemDto, ReadGraphDetailItemDto>();
    }
}