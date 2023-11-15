using DB.Model.TechnologicalProcessInfo;
using Shared.Enums;

namespace DatabaseLayer.Options.TechologicalProcessO;

public static class TpArchiveOptions
{
    public static IQueryable<TechnologicalProcess> TpArchiveSearch(
        this IQueryable<TechnologicalProcess> technologicalProcesses,
        string text,
        SerialNumberOrTitleFilter searchOptions)
    {
        if (string.IsNullOrEmpty(text)) 
            return technologicalProcesses;
        return searchOptions switch
        {
            SerialNumberOrTitleFilter.ForSerialNumber => technologicalProcesses.Where(tp => tp.Detail!.SerialNumber.Contains(text)),
            SerialNumberOrTitleFilter.ForTitle => technologicalProcesses.Where(tp => tp.Detail!.Title.Contains(text)),
            _ => technologicalProcesses,
        };
    }

    public static IQueryable<TechnologicalProcess> TpArchiveOrder(
        this IQueryable<TechnologicalProcess> technologicalProcesses,
        IssuedTechProcessOrderOptions orderOptions = IssuedTechProcessOrderOptions.Base,
        KindOfOrder kindOfOrder = KindOfOrder.Base)
    {
        return orderOptions switch
        {
            IssuedTechProcessOrderOptions.SerialNumber => kindOfOrder switch
            {
                KindOfOrder.Up => technologicalProcesses.OrderBy(tp => tp.Detail!.SerialNumber),
                KindOfOrder.Down => technologicalProcesses.OrderByDescending(tp => tp.Detail!.SerialNumber),
                _ => technologicalProcesses
            },
            IssuedTechProcessOrderOptions.Title => kindOfOrder switch
            {
                KindOfOrder.Up => technologicalProcesses.OrderBy(tp => tp.Detail!.Title),
                KindOfOrder.Down => technologicalProcesses.OrderByDescending(tp => tp.Detail!.Title),
                _ => technologicalProcesses
            },
            IssuedTechProcessOrderOptions.Material => kindOfOrder switch
            {
                KindOfOrder.Up => technologicalProcesses.OrderBy(tp => tp.TechnologicalProcessData.Material!.Title),
                KindOfOrder.Down => technologicalProcesses.OrderByDescending(tp => tp.TechnologicalProcessData.Material!.Title),
                _ => technologicalProcesses
            },
            IssuedTechProcessOrderOptions.BlankType => kindOfOrder switch
            {
                KindOfOrder.Up => technologicalProcesses.OrderBy(tp => tp.TechnologicalProcessData.BlankType!.Title),
                KindOfOrder.Down => technologicalProcesses.OrderByDescending(tp => tp.TechnologicalProcessData.BlankType!.Title),
                _ => technologicalProcesses
            },
            IssuedTechProcessOrderOptions.Developer => kindOfOrder switch
            {
                KindOfOrder.Up => technologicalProcesses.OrderBy(tp => tp.Developer!.FFL),
                KindOfOrder.Down => technologicalProcesses.OrderByDescending(tp => tp.Developer!.FFL),
                _ => technologicalProcesses
            },
            _ => technologicalProcesses,
        };
    }
}