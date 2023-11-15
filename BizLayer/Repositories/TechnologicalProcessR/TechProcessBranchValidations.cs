using DatabaseLayer.Helper;
using DB.Model.StorageInfo.Graph;
using DB.Model.TechnologicalProcessInfo;
using Microsoft.EntityFrameworkCore;
using Shared.BasicStructuresExtensions;

namespace BizLayer.Repositories.TechnologicalProcessR;

public static class TechProcessBranchValidations
{
    public static async Task FromMainToNewBranchValidation(List<int>? branchNumbers, List<TechnologicalProcessItem>? newBranchItems, DbContext context, ErrorsMapper errors)
    {
        if (branchNumbers is null)
            errors.AddErrors("Не удалось получить список веток");
        if (branchNumbers?.Count == 4)
            errors.AddErrors("Выход за границы максимального количества веток");

        var operationDetailItemSet = context.Set<OperationGraphDetailItem>();

        if (newBranchItems is null)
            return;

        foreach (var item in newBranchItems)
        {
            if (await operationDetailItemSet.AnyAsync(i => i.TechnologicalProcessItemId == item.Id))
                errors.AddErrors($"Операция {item.Number} задействована в деталях операционных графиков");
        }
    }

    public static async Task FromMainToBranchValidationAsync(
        TechnologicalProcessItem? insertedItem,
        List<TechnologicalProcessItem>? branchItems, 
        int beforeBranchItemId, 
        DbContext context, 
        ErrorsMapper errors)
    {
        if (!(branchItems?.Any(item => beforeBranchItemId == 0 || item.Id == beforeBranchItemId) ?? false))
            errors.AddErrors("Операции ветки не содержат операцию, перед которой должна стоять вставляемая операция");

        if (insertedItem is null)
            return;

        if (await context.Set<OperationGraphDetailItem>().AnyAsync(i => i.TechnologicalProcessItemId == insertedItem.Id))
            errors.AddErrors($"Операция {insertedItem.Number} задействована в деталях операционных графиков");
    }

    public static async Task HideItemsValidationAsync(List<TechnologicalProcessItem>? items, DbContext context, ErrorsMapper errors)
    {
        if (items.IsNullOrEmpty())
            return;

        var operationDetailItemSet = context.Set<OperationGraphDetailItem>();

        foreach (var item in items!)
        {
            if (await operationDetailItemSet.AnyAsync(i => i.TechnologicalProcessItemId == item.Id))
                errors.AddErrors($"Операция {item.Number} задействована в деталях операционных графиков");
        }
    }
}