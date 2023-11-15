using DB.Model.TechnologicalProcessInfo;
using Microsoft.EntityFrameworkCore;
using Shared.Enums;
using Shared.BasicStructuresExtensions;
using DB.Model.StatusInfo;
using Shared.Static;

namespace DatabaseLayer.Options.TechologicalProcessO;

public static class TpReadonlyOptions
{
    public static IQueryable<TechnologicalProcess> TpReadonlySearch(
        this IQueryable<TechnologicalProcess> technologicalProcesses,
        string text,
        TechProcessReadonlySearchOptions searchOptions)
    {
        if (string.IsNullOrEmpty(text))
            return technologicalProcesses;
        return searchOptions switch
        {
            TechProcessReadonlySearchOptions.SerialNumber => technologicalProcesses.Where(tp => tp.Detail!.SerialNumber.Contains(text)),
            TechProcessReadonlySearchOptions.Title => technologicalProcesses.Where(tp => tp.Detail!.Title.Contains(text)),
            TechProcessReadonlySearchOptions.Developer => technologicalProcesses.Where(tp => tp.Developer!.FFL.Contains(text)),
            _ => technologicalProcesses
        };
    }

    public static IQueryable<TechnologicalProcess> TpReadonlyOrder(
        this IQueryable<TechnologicalProcess> technologicalProcesses,
        KindOfOrder kindOfOrder,
        TechProcessReadonlyOrderOptions orderOptions = TechProcessReadonlyOrderOptions.Base)
    {
        return orderOptions switch
        {
            TechProcessReadonlyOrderOptions.SerialNumber => kindOfOrder switch
            {
                KindOfOrder.Up => technologicalProcesses.OrderBy(tp => tp.Detail!.SerialNumber),
                KindOfOrder.Down => technologicalProcesses.OrderByDescending(tp => tp.Detail!.SerialNumber),
                _ => technologicalProcesses
            },
            TechProcessReadonlyOrderOptions.Title => kindOfOrder switch
            {
                KindOfOrder.Up => technologicalProcesses.OrderBy(tp => tp.Detail!.Title),
                KindOfOrder.Down => technologicalProcesses.OrderByDescending(tp => tp.Detail!.Title),
                _ => technologicalProcesses
            },
            TechProcessReadonlyOrderOptions.Rate => kindOfOrder switch
            {
                KindOfOrder.Up => technologicalProcesses.OrderBy(tp => tp.TechnologicalProcessData.Rate),
                KindOfOrder.Down => technologicalProcesses.OrderByDescending(tp => tp.TechnologicalProcessData.Rate),
                _ => technologicalProcesses
            },
            TechProcessReadonlyOrderOptions.Developer => kindOfOrder switch
            {
                KindOfOrder.Up => technologicalProcesses.OrderBy(tp => tp.Developer!.FFL),
                KindOfOrder.Down => technologicalProcesses.OrderByDescending(tp => tp.Developer!.FFL),
                _ => technologicalProcesses
            },
            _ => technologicalProcesses
        };
    }

    public static IQueryable<TechnologicalProcess> CompletedTpsOrder(
        this IQueryable<TechnologicalProcess> technologicalProcesses,
        CompletedDeveloperTechProcessesOrderOptions orderOptions = CompletedDeveloperTechProcessesOrderOptions.Date,
        KindOfOrder kindOfOrder = KindOfOrder.Base)
    {
        return orderOptions switch
        {
            CompletedDeveloperTechProcessesOrderOptions.Date => kindOfOrder switch
            {
                KindOfOrder.Up => technologicalProcesses.OrderBy(tp => 
                    tp.TechnologicalProcessStatuses!.First(tps => tps.StatusId == (int)TechProcessStatuses.Completed).StatusDate),
                KindOfOrder.Down => technologicalProcesses.OrderByDescending(tp =>
                    tp.TechnologicalProcessStatuses!.First(tps => tps.StatusId == (int)TechProcessStatuses.Completed).StatusDate),
                _ => technologicalProcesses.OrderByDescending(tp =>
                    tp.TechnologicalProcessStatuses!.First(tps => tps.StatusId == (int)TechProcessStatuses.Completed).StatusDate)
            },
            CompletedDeveloperTechProcessesOrderOptions.SerialNumber => kindOfOrder switch
            {
                KindOfOrder.Up => technologicalProcesses.OrderBy(tp => tp.Detail!.SerialNumber),
                KindOfOrder.Down => technologicalProcesses.OrderByDescending(tp => tp.Detail!.SerialNumber),
                _ => technologicalProcesses.OrderByDescending(tp => 
                    tp.TechnologicalProcessStatuses!.First(tps => tps.StatusId == (int)TechProcessStatuses.Completed).StatusDate)
            },
            CompletedDeveloperTechProcessesOrderOptions.Title => kindOfOrder switch
            {
                KindOfOrder.Up => technologicalProcesses.OrderBy(tp => tp.Detail!.Title),
                KindOfOrder.Down => technologicalProcesses.OrderByDescending(tp => tp.Detail!.Title),
                _ => technologicalProcesses.OrderByDescending(tp => 
                    tp.TechnologicalProcessStatuses!.First(tps => tps.StatusId == (int)TechProcessStatuses.Completed).StatusDate)
            },
            _ => technologicalProcesses.OrderByDescending(tp =>
                tp.TechnologicalProcessStatuses!.First(tps => tps.StatusId == (int)TechProcessStatuses.Completed).StatusDate)
        };
    }

    public static IQueryable<TechnologicalProcess> TpReadonlyFromProduct(this IQueryable<TechnologicalProcess> technologicalProcesses, List<int>? detailsId)
    {
        return detailsId.IsNullOrEmpty() 
            ? technologicalProcesses 
            : technologicalProcesses.Where(tp => detailsId!.Contains(tp.DetailId));
    }

    public static IQueryable<TechnologicalProcess> TpFromBlankType(this IQueryable<TechnologicalProcess> technologicalProcesses, int blankTypeId)
    {
        return blankTypeId <= 0 
            ? technologicalProcesses 
            : technologicalProcesses.Where(tp => tp.TechnologicalProcessData.BlankType != null && tp.TechnologicalProcessData.BlankType.Id == blankTypeId);
    }

    public static IQueryable<TechnologicalProcess> TpFromMaterial(this IQueryable<TechnologicalProcess> technologicalProcesses, int materialId)
    {
        return materialId <= 0 
            ? technologicalProcesses 
            : technologicalProcesses.Where(tp => tp.TechnologicalProcessData.Material != null && tp.TechnologicalProcessData.Material.Id == materialId);
    }

    public static IQueryable<TechnologicalProcess> TpFromDetailType(this IQueryable<TechnologicalProcess> technologicalProcesses, int detailTypeId)
    {
        return detailTypeId <= 0 
            ? technologicalProcesses 
            : technologicalProcesses.Where(tp => tp.Detail!.DetailTypeId == detailTypeId);
    }

    public static IQueryable<TechnologicalProcess> TpFromStatus(this IQueryable<TechnologicalProcess> technologicalProcesses, int statusId)
    {
        technologicalProcesses = technologicalProcesses.Include(tp => tp.TechnologicalProcessStatuses);
        
        return statusId <= 0 
            ? technologicalProcesses 
            : technologicalProcesses.Where(tp => tp.TechnologicalProcessStatuses!.OrderBy(s => s.StatusDate).Last().StatusId == statusId);
    }
}