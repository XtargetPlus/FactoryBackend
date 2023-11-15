namespace Shared.Dto.Graph.Read.Open;

public class GraphDetailItemHigherDto
{
    /// <summary>
    /// Можно ли добавить в конец списка
    /// </summary>
    public bool PossibleToAddToEnd { get; set; }

    /// <summary>
    /// Список групп операций тех процесса
    /// </summary>
    public List<GraphDetailItemGroupDto>? GroupItems { get; set; }
}