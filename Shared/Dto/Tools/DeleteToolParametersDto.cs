using System.ComponentModel.DataAnnotations;

namespace Shared.Dto.Tools;

public class DeleteToolParametersDto
{
    [Range(1,int.MaxValue)]
    public int ToolId { get; set; }
    public List<int>? ParametersId { get; set; }
}