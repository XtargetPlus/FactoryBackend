using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Shared.Static;

public class BasePagination
{
    /// <summary>
    /// Сколько получить
    /// </summary>
    [DefaultValue(50)]
    [Range(0, int.MaxValue)]
    public int Take { get; set; }
    /// <summary>
    /// Сколько пропустить
    /// </summary>
    [DefaultValue(0)]
    [Range(0, int.MaxValue)]
    public int Skip { get; set; }
}
