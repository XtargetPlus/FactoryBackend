namespace ServiceLayer.TechnologicalProcesses.Services.SubClasses;

/// <summary>
/// Ветка
/// </summary>
public class BranchNumber
{
    /// <summary>
    /// Id тех процесса - к какому тех процессу относится ветка
    /// </summary>
    public int TechProcessId { get; set; }
    /// <summary>
    /// Ее номер - приоритет операций этой ветки
    /// </summary>
    public int Priority { get; set; }
}
