using DB.Model.StatusInfo;
using Shared.Enums;

namespace DatabaseLayer.Options.TechologicalProcessO;

public static class TpStatusesOptions
{
    public static IQueryable<TechnologicalProcessStatus> IssuedTechProcessesFromTechnologistOrder(
        this IQueryable<TechnologicalProcessStatus> technologicalProcessStatus,
        IssuedTechProcessesFromTechnologistOrderOptions orderOptions = IssuedTechProcessesFromTechnologistOrderOptions.Base,
        KindOfOrder kindOfOrder = KindOfOrder.Base)
    {
        return orderOptions switch
        {
            IssuedTechProcessesFromTechnologistOrderOptions.DateOfIssue => kindOfOrder switch
            {
                KindOfOrder.Up => technologicalProcessStatus.OrderBy(tps => tps.StatusDate),
                KindOfOrder.Down => technologicalProcessStatus.OrderByDescending(tps => tps.StatusDate),
                _ => technologicalProcessStatus.OrderByDescending(tps => tps.StatusDate)
            },
            IssuedTechProcessesFromTechnologistOrderOptions.SerialNumber => kindOfOrder switch
            {
                KindOfOrder.Up => technologicalProcessStatus.OrderBy(tps => tps.TechnologicalProcess!.Detail!.SerialNumber),
                KindOfOrder.Down => technologicalProcessStatus.OrderByDescending(tps => tps.TechnologicalProcess!.Detail!.SerialNumber),
                _ => technologicalProcessStatus.OrderByDescending(tps => tps.StatusDate)
            },
            IssuedTechProcessesFromTechnologistOrderOptions.Title => kindOfOrder switch
            {
                KindOfOrder.Up => technologicalProcessStatus.OrderBy(tps => tps.TechnologicalProcess!.Detail!.Title),
                KindOfOrder.Down => technologicalProcessStatus.OrderByDescending(tps => tps.TechnologicalProcess!.Detail!.Title),
                _ => technologicalProcessStatus.OrderByDescending(tps => tps.StatusDate)
            },
            IssuedTechProcessesFromTechnologistOrderOptions.FFL => kindOfOrder switch
            {
                KindOfOrder.Up => technologicalProcessStatus.OrderBy(tps => tps.User!.FFL),
                KindOfOrder.Down => technologicalProcessStatus.OrderByDescending(tps => tps.User!.FFL),
                _ => technologicalProcessStatus.OrderByDescending(tps => tps.StatusDate)
            },
            IssuedTechProcessesFromTechnologistOrderOptions.ProfessionNumber => kindOfOrder switch
            {
                KindOfOrder.Up => technologicalProcessStatus.OrderBy(tps => tps.User!.FFL),
                KindOfOrder.Down => technologicalProcessStatus.OrderByDescending(tps => tps.User!.FFL),
                _ => technologicalProcessStatus.OrderByDescending(tps => tps.StatusDate)
            },
            IssuedTechProcessesFromTechnologistOrderOptions.Subdivision => kindOfOrder switch
            {
                KindOfOrder.Up => technologicalProcessStatus.OrderBy(tps => tps.Subdivision!.Title),
                KindOfOrder.Down => technologicalProcessStatus.OrderByDescending(tps => tps.Subdivision!.Title),
                _ => technologicalProcessStatus.OrderByDescending(tps => tps.StatusDate)
            },
            _ => technologicalProcessStatus.OrderByDescending(tps => tps.StatusDate)
        };
    }

    public static IQueryable<TechnologicalProcessStatus> TpDevelopmentStagesOrder(
        this IQueryable<TechnologicalProcessStatus> technologicalProcessStatus,
        TechProcessDevelopmentStagesOrderOptions orderOptions = TechProcessDevelopmentStagesOrderOptions.StatusDate,
        KindOfOrder kindOfOrder = KindOfOrder.Up)
    {
        return orderOptions switch
        {
            TechProcessDevelopmentStagesOrderOptions.StatusDate => kindOfOrder switch
            {
                KindOfOrder.Up => technologicalProcessStatus.OrderBy(tps => tps.StatusDate),
                KindOfOrder.Down => technologicalProcessStatus.OrderByDescending(tps => tps.StatusDate),
                _ => technologicalProcessStatus.OrderBy(tps => tps.StatusDate)
            },
            TechProcessDevelopmentStagesOrderOptions.Status => kindOfOrder switch
            {
                KindOfOrder.Up => technologicalProcessStatus.OrderBy(tps => tps.Status!.Title),
                KindOfOrder.Down => technologicalProcessStatus.OrderByDescending(tps => tps.Status!.Title),
                _ => technologicalProcessStatus.OrderBy(tps => tps.StatusDate)
            },
            _ => technologicalProcessStatus.OrderBy(tps => tps.StatusDate)
        }; ;
    }

    public static IQueryable<TechnologicalProcessStatus> CompletedTpsOrder(
        this IQueryable<TechnologicalProcessStatus> technologicalProcessStatuses,
        CompletedDeveloperTechProcessesOrderOptions orderOptions = CompletedDeveloperTechProcessesOrderOptions.Date,
        KindOfOrder kindOfOrder = KindOfOrder.Base)
    {
        return orderOptions switch
        {
            CompletedDeveloperTechProcessesOrderOptions.Date => kindOfOrder switch
            {
                KindOfOrder.Up => technologicalProcessStatuses.OrderBy(tps => tps.StatusDate),
                KindOfOrder.Down => technologicalProcessStatuses.OrderByDescending(tps => tps.StatusDate),
                _ => technologicalProcessStatuses.OrderByDescending(tps => tps.StatusDate)
            },
            CompletedDeveloperTechProcessesOrderOptions.SerialNumber => kindOfOrder switch
            {
                KindOfOrder.Up => technologicalProcessStatuses.OrderBy(tps => tps.TechnologicalProcess!.Detail!.SerialNumber),
                KindOfOrder.Down => technologicalProcessStatuses.OrderByDescending(tps => tps.TechnologicalProcess!.Detail!.SerialNumber),
                _ => technologicalProcessStatuses.OrderByDescending(tps => tps.StatusDate)
            },
            CompletedDeveloperTechProcessesOrderOptions.Title => kindOfOrder switch
            {
                KindOfOrder.Up => technologicalProcessStatuses.OrderBy(tps => tps.TechnologicalProcess!.Detail!.Title),
                KindOfOrder.Down => technologicalProcessStatuses.OrderByDescending(tps => tps.TechnologicalProcess!.Detail!.Title),
                _ => technologicalProcessStatuses.OrderByDescending(tps => tps.StatusDate)
            },
            _ => technologicalProcessStatuses.OrderByDescending(tps => tps.StatusDate)
        };
    }

    public static IQueryable<TechnologicalProcessStatus> IssuedDuplicatesOrder(
        this IQueryable<TechnologicalProcessStatus> technologicalProcessStatuses,
        IssuedDuplicateTechProcessOrderOptions orderOptions = IssuedDuplicateTechProcessOrderOptions.Base,
        KindOfOrder kindOfOrder = KindOfOrder.Base)
    {
        return orderOptions switch
        {
            IssuedDuplicateTechProcessOrderOptions.DateOfIssue => kindOfOrder switch
            {
                KindOfOrder.Up => technologicalProcessStatuses.OrderBy(tps => tps.StatusDate),
                KindOfOrder.Down => technologicalProcessStatuses.OrderByDescending(tps => tps.StatusDate),
                _ => technologicalProcessStatuses.OrderByDescending(tps => tps.StatusDate)
            },
            IssuedDuplicateTechProcessOrderOptions.ProfessionNumber => kindOfOrder switch
            {
                KindOfOrder.Up => technologicalProcessStatuses.OrderBy(tps => tps.User!.ProfessionNumber),
                KindOfOrder.Down => technologicalProcessStatuses.OrderByDescending(tps => tps.User!.ProfessionNumber),
                _ => technologicalProcessStatuses.OrderByDescending(tps => tps.StatusDate)
            },
            IssuedDuplicateTechProcessOrderOptions.Subdivision => kindOfOrder switch
            {
                KindOfOrder.Up => technologicalProcessStatuses.OrderBy(tps => tps.Subdivision!.Title),
                KindOfOrder.Down => technologicalProcessStatuses.OrderByDescending(tps => tps.Subdivision!.Title),
                _ => technologicalProcessStatuses.OrderByDescending(tps => tps.StatusDate)
            },
            IssuedDuplicateTechProcessOrderOptions.FFL => kindOfOrder switch
            {
                KindOfOrder.Up => technologicalProcessStatuses.OrderBy(tps => tps.User!.FFL),
                KindOfOrder.Down => technologicalProcessStatuses.OrderByDescending(tps => tps.User!.FFL),
                _ => technologicalProcessStatuses.OrderByDescending(tps => tps.StatusDate)
            },
            _ => technologicalProcessStatuses.OrderByDescending(tps => tps.StatusDate)
        };
    }
}