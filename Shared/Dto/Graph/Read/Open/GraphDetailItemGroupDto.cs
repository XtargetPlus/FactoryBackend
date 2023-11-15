namespace Shared.Dto.Graph.Read.Open;

public class GraphDetailItemGroupDto
{
    /// <summary>
    /// Есть ли у группы ответвления
    /// </summary>
    public bool IsHaveBranches { get; set; }
    /// <summary>
    /// Возможно ли добавить в конец группы новую операцию
    /// </summary>
    public bool PossibleToAddToEnd { get; set; }
    /// <summary>
    /// Операции тех процесса группы
    /// </summary>
    public List<ReadGraphDetailItemDto>? Items { get; set; }
}