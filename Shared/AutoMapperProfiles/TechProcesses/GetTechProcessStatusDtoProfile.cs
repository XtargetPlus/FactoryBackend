using AutoMapper;
using DB.Model.StatusInfo;
using Shared.Dto.TechnologicalProcess;
using Shared.Dto.TechnologicalProcess.Issue.Read;
using System.Globalization;

namespace Shared.AutoMapperProfiles.TechProcesses;

public class GetTechProcessStatusDtoProfile : Profile
{
    public GetTechProcessStatusDtoProfile()
    {
        CreateProjection<TechnologicalProcessStatus, GeTechProcessDevelopmentStagesDto>()
            .ForMember(dest => dest.StatusDate, opt => opt.MapFrom(tps => tps.StatusDate.ToString("dd-MM-yyyy HH:mm")))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(tps => tps.Status!.Title));

        CreateProjection<TechnologicalProcessStatus, GetBaseTechProcessDto>()
            .ForMember(dest => dest.IsActual, opt => opt.MapFrom(tp => tp.TechnologicalProcess!.IsActual))
            .ForMember(dest => dest.TechProcessId, opt => opt.MapFrom(tps => tps.TechnologicalProcessId))
            .ForMember(dest => dest.SerialNumber, opt => opt.MapFrom(tps => tps.TechnologicalProcess!.Detail!.SerialNumber))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(tps => tps.TechnologicalProcess!.Detail!.Title))
            .ForMember(dest => dest.Date, opt => opt.MapFrom(tps => DateOnly.FromDateTime(tps.StatusDate)));

        CreateProjection<TechnologicalProcessStatus, GetTechProcessDuplicateDto>()
            .ForMember(dest => dest.TechProcessStatusId, opt => opt.MapFrom(tps => tps.Id))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(tps => tps.UserId))
            .ForMember(dest => dest.FFL, opt => opt.MapFrom(tps => tps.User!.FFL))
            .ForMember(dest => dest.ProfessionNumber, opt => opt.MapFrom(tps => tps.User!.ProfessionNumber))
            .ForMember(dest => dest.Issued, opt => opt.MapFrom(tps => tps.StatusDate.ToString("dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture)))
            .ForMember(dest => dest.Subdivision, opt => opt.MapFrom(tps => tps.Subdivision!.FatherId != null ? $"{tps.Subdivision.Father!.Title}: {tps.Subdivision.Title}" : tps.Subdivision.Title));

        CreateProjection<TechnologicalProcessStatus, IssuedTechProcessesFromTechnologistDto>()
            .ForMember(dest => dest.SerialNumber, opt => opt.MapFrom(tps => tps.TechnologicalProcess!.Detail!.SerialNumber))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(tps => tps.TechnologicalProcess!.Detail!.Title))
            .ForMember(dest => dest.FFL, opt => opt.MapFrom(tps => tps.User!.FFL))
            .ForMember(dest => dest.ProfessionNumber, opt => opt.MapFrom(tps => tps.User!.ProfessionNumber))
            .ForMember(dest => dest.Issued, opt => opt.MapFrom(tps => tps.StatusDate.ToString("dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture)))
            .ForMember(dest => dest.Subdivision, opt => opt.MapFrom(tps => tps.Subdivision!.FatherId != null ? $"{tps.Subdivision.Father!.Title}: {tps.Subdivision.Title}" : tps.Subdivision.Title));
    }
}
 