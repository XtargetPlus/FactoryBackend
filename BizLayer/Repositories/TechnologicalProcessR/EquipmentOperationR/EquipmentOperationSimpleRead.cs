using DatabaseLayer.Helper;
using DatabaseLayer.IDbRequests;
using DB.Model.SubdivisionInfo.EquipmentInfo;

namespace BizLayer.Repositories.TechnologicalProcessR.EquipmentOperationR;

public static class EquipmentOperationSimpleRead
{
    public static async Task<EquipmentOperation?> GetByIdAsync(BaseModelRequests<EquipmentOperation> repository, int equipmentOperationId, ErrorsMapper mapper)
    {
        var equipmentOperation = await repository.FindByIdAsync(equipmentOperationId);

        if (equipmentOperation is null)
            mapper.AddErrors("Не удалось получить операцию тех процесса на станке");

        return equipmentOperation;
    }

    public static async Task<EquipmentOperation?> GetAsync(BaseModelRequests<EquipmentOperation> repository, int equipmentId, int techProcessItemId, ErrorsMapper mapper)
    {
        var equipmentOperation = await repository.FindFirstAsync(filter: eo => eo.EquipmentId == equipmentId
                                                                               && eo.TechnologicalProcessItemId == techProcessItemId);
        if (equipmentOperation is null)
            mapper.AddErrors("Не удалось получить операцию на станке");
        return equipmentOperation;
    }

    public static async Task<EquipmentOperation?> GetByPriorityAsync(BaseModelRequests<EquipmentOperation> repository, int priority, int techProcessItemId, ErrorsMapper mapper)
    {
        var equipmentOperation = await repository.FindFirstAsync(filter: eo => eo.Priority == priority
                                                                               && eo.TechnologicalProcessItemId == techProcessItemId);
        if (equipmentOperation is null)
            mapper.AddErrors("Не удалось получить операцию на станке");
        return equipmentOperation;
    }
}