using DatabaseLayer.Helper;
using DB.Model.StorageInfo.Graph;

namespace BizLayer.Repositories.GraphR.GraphDetailItemR;

public static class OperationGraphDetailItemValidations
{
    public static void ValidationBeforeAddToBlock(OperationGraphDetail? detail, int techProcessItemId, int priority, ErrorsMapper errors)
    {
        if (detail?.OperationGraphDetailItems!.Any(i => i.TechnologicalProcessItemId == techProcessItemId) ?? true)
            errors.AddErrors("Деталь графика содержит данную операцию тех процесса");

        if (priority % 5 == 0)
            errors.AddErrors("Нельзя добавлять в блок с операцией из main");
    }

    public static void ValidationItemsPriorityEquals(List<OperationGraphDetailItem>? items, int priority, ErrorsMapper errors)
    {
        if (items is null) return;

        if (items.Any(i => i.TechnologicalProcessItem!.Priority != priority))
            errors.AddErrors("Не все операции находятся в одном блоке");
    }
}