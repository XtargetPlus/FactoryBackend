using System.ComponentModel.DataAnnotations;

namespace Shared.Dto.Tools;

public class AddToolParametersDto
{
    [Range(1,int.MaxValue)]
    public int ParameterId { get; set; }
    public string Value { get; set; }
}