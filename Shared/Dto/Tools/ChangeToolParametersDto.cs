using System.ComponentModel.DataAnnotations;

namespace Shared.Dto.Tools;

public class ChangeToolParametersDto
{
    [Range(1,int.MaxValue)]
    public int ToolId { get; set; }
    [Range(1,int.MaxValue)]
    public int ParametersId { get; set; }
    public string Value { get; set; }
}