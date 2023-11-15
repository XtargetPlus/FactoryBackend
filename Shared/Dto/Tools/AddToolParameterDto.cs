using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Shared.Enums;

namespace Shared.Dto.Tools;

public class AddToolParameterDto
{
    [Required]
    public string Title { get; set; }   

    public int? UnitId { get; set; }
}