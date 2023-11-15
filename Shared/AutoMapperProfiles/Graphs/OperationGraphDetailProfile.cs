using AutoMapper;
using DB.Model.StorageInfo.Graph;
using Shared.Dto.Graph.Read.Open;

namespace Shared.AutoMapperProfiles.Graphs;

public class OperationGraphDetailProfile : Profile
{
    public OperationGraphDetailProfile()
    {
        var openWithRepeats = true;

        CreateProjection<OperationGraphDetail, GraphDetailDto>()
            .ForMember(
                dest => dest.GraphDetailId,
                opt => opt.MapFrom(d => d.Id))
            .ForMember(
                dest => dest.DetailTitle,
                opt => opt.MapFrom(d => d.Detail!.Title))
            .ForMember(
                dest => dest.DetailSerialNumber,
                opt => opt.MapFrom(d => d.Detail!.SerialNumber))
            .ForMember(
                dest => dest.TechProcessId,
                opt => opt.MapFrom(d => d.TechnologicalProcessId))
            .ForMember(
                dest => dest.IsHaveOtherTechProcesses,
                opt => opt.MapFrom(d => d.Detail!.TechnologicalProcesses!.Count > 1))
            .ForMember(
                dest => dest.LocalPositionNumber,
                opt => opt.MapFrom(d => d.DetailGraphNumber))
            .ForMember(
                dest => dest.TotalPlanCount,
                opt => opt.MapFrom(d => d.TotalPlannedNumber))
            .ForMember(
                dest => dest.PlanCount,
                opt => opt.MapFrom(d => d.PlannedNumber))
            .ForMember(
                dest => dest.IsDublicate,
                opt => opt.MapFrom(d => !d.TotalPlannedNumber.HasValue))
            .ForMember(
                dest => dest.PositionNumberToDisplay,
                opt => opt.MapFrom(d =>
                    openWithRepeats ? d.DetailGraphNumberWithRepeats : d.DetailGraphNumberWithoutRepeats.ToString()));
    }
}