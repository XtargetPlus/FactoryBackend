using System.ComponentModel.DataAnnotations;

namespace Shared.Dto.Tools;

public class AddReplaceabilityDto
{
    [Range(1,int.MaxValue)]
    public int MasterId { get; set; }
    [Range(1, int.MaxValue)]
    public int SlaveId { get; set; }
}