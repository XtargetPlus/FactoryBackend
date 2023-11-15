using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Shared.Dto.Detail;

public class GetAllProductDetailsDto
{
    /// <summary>
    /// Id детали
    /// </summary>
    [Required]
    [Range(1, int.MaxValue)]
    public int DetailId { get; set; }
    
    /// <summary>
    /// true - получить только детали с подготовкой производства
    /// </summary>
    [DefaultValue(true)]
    public bool IsHardDetail { get; set; }
}