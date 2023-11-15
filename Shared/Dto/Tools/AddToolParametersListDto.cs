using System.ComponentModel.DataAnnotations;

namespace Shared.Dto.Tools;

public class AddToolParametersListDto
{
    [Range(1, int.MaxValue)]
    public int ToolId { get; set; }
 
    public List<AddToolParametersDto> AddToolParameters { get; set; }
}