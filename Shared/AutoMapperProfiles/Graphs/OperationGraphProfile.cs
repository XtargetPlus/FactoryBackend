using AutoMapper;
using DB.Model.StorageInfo.Graph;
using Shared.Dto.Graph.Read;
using Shared.Dto.Graph.Read.Open;

namespace Shared.AutoMapperProfiles.Graphs;

public class OperationGraphProfile : Profile
{
    public OperationGraphProfile()
    {
        var userId = 0;

        // Projections

        CreateProjection<OperationGraph, GetAllOperationGraphDto>()
            .ForMember(
                dest => dest.OperationGraphId,
                opt => opt.MapFrom(g => g.Id))
            .ForMember(
                dest => dest.Access,
                opt => opt.MapFrom(g => 
                    (g.OwnerId == userId || g.OperationGraphUsers!.Any(gu => gu.UserId == userId && !gu.IsReadonly)) 
                        ? "Полный"
                        : "Только на чтение"))
            .ForMember(
                dest => dest.IsMainGraph,
                opt => opt.MapFrom(g => 
                    !g.OperationGraphNextGroups!.Any()))
            .ForMember(
                dest => dest.DetailSerialNumber,
                opt => opt.MapFrom(g => g.ProductDetailId.HasValue
                    ? g.ProductDetail!.SerialNumber
                    : null))
            .ForMember(
                dest => dest.DetailTitle,
                opt => opt.MapFrom(g => g.ProductDetailId.HasValue
                    ? g.ProductDetail!.Title
                    : null))
            .ForMember(
                dest => dest.PlanCount,
                opt => opt.MapFrom(g => g.PlanCount))
            .ForMember(
                dest => dest.Subdivision,
                opt => opt.MapFrom(g => g.Subdivision!.Title))
            .ForMember(
                dest => dest.Status,
                opt => opt.MapFrom(g => g.Status!.Title))
            .ForMember(
                dest => dest.GraphDate,
                opt => opt.MapFrom(g => g.GraphDate.ToString("Y")));

        CreateProjection<OperationGraph, GetAllOperationGraphFromOwnerDto>()
            .ForMember(
                dest => dest.OperationGraphId,
                opt => opt.MapFrom(g => g.Id))
            .ForMember(
                dest => dest.IsMainGraph,
                opt => opt.MapFrom(g =>
                    !g.OperationGraphNextGroups!.Any()))
            .ForMember(
                dest => dest.DetailSerialNumber,
                opt => opt.MapFrom(g => g.ProductDetailId.HasValue
                    ? g.ProductDetail!.SerialNumber
                    : null))
            .ForMember(
                dest => dest.DetailTitle,
                opt => opt.MapFrom(g => g.ProductDetailId.HasValue
                    ? g.ProductDetail!.Title
                    : null))
            .ForMember(
                dest => dest.PlanCount,
                opt => opt.MapFrom(g => g.PlanCount))
            .ForMember(
                dest => dest.Subdivision,
                opt => opt.MapFrom(g => g.Subdivision!.Title))
            .ForMember(
                dest => dest.GraphDate,
                opt => opt.MapFrom(g => g.GraphDate.ToString("Y")));

        CreateProjection<OperationGraph, GetAllSinglesOperationGraphDto>()
            .ForMember(
                dest => dest.OperationGraphId,
                opt => opt.MapFrom(g => g.Id))
            .ForMember(
                dest => dest.DetailSerialNumber,
                opt => opt.MapFrom(g => g.ProductDetail!.SerialNumber))
            .ForMember(
                dest => dest.DetailTitle,
                opt => opt.MapFrom(g => g.ProductDetail!.Title))
            .ForMember(
                dest => dest.PlanCount,
                opt => opt.MapFrom(g => g.PlanCount))
            .ForMember(
                dest => dest.GraphDate,
                opt => opt.MapFrom(g => g.GraphDate.ToString("Y")))
            .ForMember(
                dest => dest.Subdivision,
                opt => opt.MapFrom(g => g.Subdivision!.Title));

        CreateProjection<OperationGraph, GraphInfoDto>()
            .ForMember(
                dest => dest.GraphId,
                opt => opt.MapFrom(g => g.Id))
            .ForMember(
                dest => dest.ReadOnly,
                opt => opt.MapFrom(g =>
                    (g.OwnerId != userId && !g.OperationGraphUsers!.Any(gu => gu.UserId == userId && !gu.IsReadonly))))
            .ForMember(
                dest => dest.CurrentStatusId,
                opt => opt.MapFrom(g => g.StatusId))
            .ForMember(
                dest => dest.CurrentStatusTitle,
                opt => opt.MapFrom(g => g.Status!.Title))
            .ForMember(
                dest => dest.DetailSerialNumber,
                opt => opt.MapFrom(g => g.ProductDetail!.SerialNumber))
            .ForMember(
                dest => dest.DetailTitle,
                opt => opt.MapFrom(g => g.ProductDetail!.Title))
            .ForMember(
                dest => dest.GraphDate,
                opt => opt.MapFrom(g => g.GraphDate.ToString("Y")))
            .ForMember(
                dest => dest.PlanCount,
                opt => opt.MapFrom(g => g.PlanCount))
            .ForMember(
                dest => dest.Subdivision,
                opt => opt.MapFrom(g => g.Subdivision!.Title));
    }
}