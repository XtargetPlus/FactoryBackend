namespace Shared.Dto.Graph.Read.Open;

public class InterimGraphDetailItemDto
{
    /// <summary>
    /// Id детали графика
    /// </summary>
    public int GraphDetailId { get; set; }
    /// <summary>
    /// Id операции детали графика
    /// </summary>
    public int GraphDetailItemId { get; set; }
    /// <summary>
    /// Id операции тех процесса
    /// </summary>
    public int ItemId { get; set; }
    /// <summary>
    /// Id номер позиции
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