using DatabaseLayer.Helper;
using DB.Model.StorageInfo.Graph;
using Shared.Enums;

namespace BizLayer.Repositories.GraphR;

public static class OperationGraphValidations
{
    public static void GraphInWorkWithoutConfirmedDetails(OperationGraph? graph, ErrorsMapper mapper)
    {
        if (graph?.StatusId is not (int)GraphStatus.InWork)
            mapper.AddErrors("Нельзя изменить, который находится не в разработке или не на заморожен");
        if (graph?.OperationGraphDetails?.Any(gd => gd.IsConfirmed) ?? true)
            mapper.AddErrors("В графике есть подтвержденные детали");
    }

    public static void GraphInWorkOrPausedWithoutConfirmedDetails(OperationGraph? graph, ErrorsMapper mapper)
    {
        if (graph?.StatusId is not ((int)GraphStatus.InWork or (int)GraphStatus.Paused))
            mapper.AddErrors("Нельзя изменить, который находится не в разработке или не на заморожен");
        if (graph?.OperationGraphDetails?.Any(gd => gd.IsConfirmed) ?? true)
            mapper.AddErrors("В графике есть подтвержденные детали");
    }

    public static void GraphInWork(OperationGraph? graph, ErrorsMapper mapper)
    {
        if (graph?.StatusId is not (int)GraphStatus.InWork)
            mapper.AddErrors("Нельзя изменить, который находится не в разработке или не на заморожен");
    }

    public static void ValidationGraphsBeforeAddingToGroup(List<OperationGraph> graphs, ErrorsMapper mapper)
    {
        if (graphs.Any(g => g.OperationGraphMainGroups!.Any() || g.OperationGraphNextGroups!.Any()))
            mapper.AddErrors("Нельзя добавлять графики, которые уже состоят в группе");
        if (graphs.Any(g => g.OperationGraphUsers!.Any()))
            mapper.AddErrors("Нельзя добавлять графики, к которым есть доступ у других пользователей");
        if (graphs.Any(og => og.ProductDetailId.HasValue) && graphs.Any(og => !og.ProductDetailId.HasValue))
            mapper.AddErrors("В группе должны быть или только графики с изделиями или только без изделий");
        if (graphs.Any(og => og.StatusId != 12))
            mapper.AddErrors("Можно объединять графики, только если они находятся в разработке");
        if (graphs.Any(og => og.OperationGraphDetails?.Any(ogd => ogd.IsConfirmed) ?? true))
            mapper.AddErrors("Один из графиков содержит подтвержденные детали");
    }
}