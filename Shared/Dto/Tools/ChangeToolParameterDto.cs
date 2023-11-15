using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Shared.Enums;

namespace Shared.Dto.Tools;

public class ChangeToolParameterDto
{
    [Range(1,int.MaxValue)]
    public int Id { get; set; }
    [Required]
    public string Title { get; set; }
    [DefaultValue(Enums.ToolParameterOptions.String)]
    [Range((int)ToolParameterOptions.Number,(int)ToolParameterOptions.String)]
    public ToolParameterOptions ToolParameterOptions { get; set; }
}