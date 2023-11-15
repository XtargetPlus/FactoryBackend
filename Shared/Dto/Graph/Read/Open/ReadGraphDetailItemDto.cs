namespace Shared.Dto.Graph.Read.Open;

public class ReadGraphDetailItemDto
{
    /// <summary>
    /// Id операции детали графика
    /// </summary>
    public int GraphDetailItemId { get; set; }
    /// <summary>
    /// Id операции тех процесса
    /// </summary>
    public int ItemId { get; set; }
    /// <summary>
    /// Номер позиции
    /// </summary>
    public int PositionNumber { get; set; }
    /// <summary>
    /// Приоритет операции тех процесса
    /// </summary>
    public int Priority { get; set; }
    /// <summary>
    /// Номер операции тех процесса
    /// </summary>
    public string ItemNumber { get; set; } = default!;
    /// <summary>
    /// Фактическое количество деталей, прошедшие эту операцию
    /// </summary>
    public float FactCount { get; set; }
    /// <summary>
    /// Есть ли бракы
    /// </summary>
    public bool IsHaveDefective { get; set; }
    /// <summary>
    /// Завершена ли полностью операция
    /// </summary>
    public bool IsCompleted { get; set; }
}