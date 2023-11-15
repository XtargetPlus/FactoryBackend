using AutoMapper;
using DB.Model.TechnologicalProcessInfo;
using Shared.Dto.TechnologicalProcess;
using Shared.Dto.TechnologicalProcess.Read;
using Shared.Static;
using System.Globalization;

namespace Shared.AutoMapperProfiles.TechProcesses;

/// <summary>
/// 
/// </summary>
public class GetTechProcessDtoProfile : Profile
{
    public GetTechProcessDtoProfile()
    {
        // Projections

        CreateProjection<TechnologicalProcess, GetReadonlyTechProcessInfoDto>()
            .ForMember(dest => dest.TechProcessId, opt => opt.MapFrom(tp => tp.Id))
            .ForMember(dest => dest.DetailType, opt => opt.MapFrom(tp => tp.Detail!.DetailType!.Title))
            .ForMember(dest => dest.DetailSerialNumber, opt => opt.MapFrom(tp => tp.Detail!.SerialNumber))
            .ForMember(dest => dest.DetailTitle, opt => opt.MapFrom(tp => tp.Detail!.Title))
            .ForMember(dest => dest.FinishDate, opt => opt.MapFrom(tp => tp.FinishDate.ToString()))
            .ForMember(dest => dest.StatusDate, opt => opt.MapFrom(tp => tp.TechnologicalProcessStatuses!
                                                                           .OrderBy(tps => tps.StatusDate)
                                                                           .Select(tps => tps.StatusDate)
                                                                           .Last()
                                                                           .ToString("dd:MM:yyyy HH:mm:ss", CultureInfo.InvariantCulture)))
            .ForMember(dest => dest.Material, opt => opt.MapFrom(tp => tp.TechnologicalProcessData.Material == null ? null : tp.TechnologicalProcessData.Material.Title))
            .ForMember(dest => dest.MaterialId, opt => opt.MapFrom(tp => tp.TechnologicalProcessData.MaterialId))
            .ForMember(dest => dest.BlankType, opt => opt.MapFrom(tp => tp.TechnologicalProcessData.BlankType == null ? null : tp.TechnologicalProcessData.BlankType.Title))
            .ForMember(dest => dest.BlankTypeId, opt => opt.MapFrom(tp => tp.TechnologicalProcessData.BlankTypeId))
            .ForMember(dest => dest.Developer, opt => opt.MapFrom(tp => tp.Developer!.FFL))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(tp => tp.TechnologicalProcessStatuses!
                                                                       .OrderBy(tps => tps.StatusDate)
                                                                       .Select(tps => tps.Status!.Title)
                                                                       .LastOrDefault() ?? ""))
            .ForMember(dest => dest.Rate, opt => opt.MapFrom(tp => tp.TechnologicalProcessData.Rate ?? ""))
            .ForMember(dest => dest.ManufacturingPriority, opt => opt.MapFrom(tp => Convert.ToInt32(tp.ManufacturingPriority)));

        CreateProjection<TechnologicalProcess, GetExtendedTechProcessDataDto>()
            .ForMember(dest => dest.TechProcessId, opt => opt.MapFrom(tp => tp.Id))
            .ForMember(dest => dest.Note, opt => opt.MapFrom(tp => tp.TechnologicalProcessStatuses!.OrderBy(tps => tps.StatusDate).Last().Note))
            .ForMember(dest => dest.SerialNumber, opt => opt.MapFrom(tp => tp.Detail!.SerialNumber))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(tp => tp.Detail!.Title))
            .ForMember(dest => dest.Priority, opt => opt.MapFrom(tp => (int)tp.DevelopmentPriority))
            .ForMember(dest => dest.Date, opt => opt.MapFrom(tp => DateOnly.FromDateTime(tp.TechnologicalProcessStatuses!.OrderBy(tps => tps.StatusDate).Last().StatusDate)));

        CreateProjection<TechnologicalProcess, GetAllReadonlyTechProcessDto>()
            .ForMember(dest => dest.SerialNumber, opt => opt.MapFrom(tp => tp.Detail!.SerialNumber))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(tp => tp.Detail!.Title))
            .ForMember(dest => dest.Material, opt => opt.MapFrom(tp => tp.TechnologicalProcessData.Material == null ? null : tp.TechnologicalProcessData.Material.Title))
            .ForMember(dest => dest.BlankType, opt => opt.MapFrom(tp => tp.TechnologicalProcessData.BlankType == null ? null : tp.TechnologicalProcessData.BlankType.Title))
            .ForMember(dest => dest.Developer, opt => opt.MapFrom(tp => tp.Developer!.FFL))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(tp => tp.TechnologicalProcessStatuses!.OrderBy(s => s.StatusDate).Last().Status!.Title));

        CreateProjection<TechnologicalProcess, GetAllIssuedTechProcessesDto>()
            .ForMember(dest => dest.SerialNumber, opt => opt.MapFrom(tp => tp.Detail!.SerialNumber))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(tp => tp.Detail!.Title))
            .ForMember(dest => dest.BlankType, opt => opt.MapFrom(tp => tp.TechnologicalProcessData.BlankType!.Title))
            .ForMember(dest => dest.Material, opt => opt.MapFrom(tp => tp.TechnologicalProcessData.Material!.Title))
            .ForMember(dest => dest.Developer, opt => opt.MapFrom(tp => $"{tp.Developer!.FFL} - {tp.Developer.ProfessionNumber}"));

        CreateProjection<TechnologicalProcess, DetailedTechProcessInfoDto>()
            .ForMember(dest => dest.TechProcessId, opt => opt.MapFrom(tp => tp.Id))
            .ForMember(dest => dest.ManufacturingPriority, opt => opt.MapFrom(tp => (int)tp.ManufacturingPriority))
            .ForMember(dest => dest.Developer, opt => opt.MapFrom(tp => tp.Developer!.FFL))
            .ForMember(dest => dest.Rate, opt => opt.MapFrom(tp => tp.TechnologicalProcessData.Rate))
            .ForMember(dest => dest.Material, opt => opt.MapFrom(tp => tp.TechnologicalProcessData.Material.Title))
            .ForMember(dest => dest.BlankType, opt => opt.MapFrom(tp => tp.TechnologicalProcessData.BlankType.Title))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(tp => tp.TechnologicalProcessStatuses
                .Where(tps => TechProcessVariables.StatusesWithoutIssueDuplicate.Contains(tps.StatusId))
                .OrderBy(tps => tps.StatusDate)
                .Select(tps => tps.Status!.Title)
                .LastOrDefault()))
            .ForMember(dest => dest.StatusDate, opt => opt.MapFrom(tp => tp.TechnologicalProcessStatuses
                .Where(tps => TechProcessVariables.StatusesWithoutIssueDuplicate.Contains(tps.StatusId))
                .OrderBy(tps => tps.StatusDate)
                .Select(tps => tps.StatusDate)
                .Last()
                .ToString("dd:MM:yyyy HH:mm:ss", CultureInfo.InvariantCulture)));

        CreateProjection<TechnologicalProcess, DeveloperTaskDto>()
            .ForMember(
                dest => dest.TechProcessId,
                opt => opt.MapFrom(tp => tp.Id))
            .ForMember(
                dest => dest.Priority,
                opt => opt.MapFrom(tp => tp.DevelopmentPriority))
            .ForMember(
                dest => dest.Date,
                opt => opt.MapFrom(tp => tp.FinishDate))
            .ForMember(
                dest => dest.SerialNumber,
                opt => opt.MapFrom(tp => tp.Detail!.SerialNumber))
            .ForMember(
                dest => dest.Title,
                opt => opt.MapFrom(tp => tp.Detail!.Title))
            .ForMember(
                dest => dest.StatusId,
                opt => opt.MapFrom(tp => tp.TechnologicalProcessStatuses!
                    .OrderBy(tps => tps.StatusDate)
                    .Select(tps => tps.StatusId)
                    .LastOrDefault()))
            .ForMember(
                dest => dest.Note,
                opt => opt.MapFrom(tp => tp.TechnologicalProcessStatuses!
                    .OrderBy(tps => tps.StatusDate)
                    .Select(tps => tps.Note)
                    .LastOrDefault()));
    }
}
