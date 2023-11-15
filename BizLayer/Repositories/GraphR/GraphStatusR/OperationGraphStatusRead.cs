using Shared.Dto.Graph.Read.Open;
using Shared.Enums;

namespace BizLayer.Repositories.GraphR.GraphStatusR;

public static class OperationGraphStatusRead
{
    /// <summary>
    /// Получаем список возможных статусов, на которые может поменяться текущий статус
    /// </summary>
    /// <param name="currentStatus">Текущий статус</param>
    /// <returns>Список статусов</returns>
    public static List<GraphPossibleStatusDto> PossibleStatuses(GraphStatus currentStatus)
    {
        return currentStatus switch
        {
            GraphStatus.InWork => new List<GraphPossibleStatusDto>
            {
                new() { StatusId = 13, StatusTitle = "Активный", NeedSupervisorConfirmation = true},
                new() { StatusId = 14, StatusTitle = "Приостановлен" },
            },
            GraphStatus.Paused => new List<GraphPossibleStatusDto>
            {
                new() { StatusId = 12, StatusTitle = "Активный" }
            },
            GraphStatus.Active => new List<GraphPossibleStatusDto>
            {
                new() { StatusId = 15, StatusTitle = "Завершен" },
                new() { StatusId = 16, StatusTitle = "Отменен", NeedSupervisorConfirmation = true },
            },
            _ => new List<GraphPossibleStatusDto>()
        };
    }
}