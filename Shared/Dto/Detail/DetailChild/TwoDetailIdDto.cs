using System.ComponentModel.DataAnnotations;

namespace Shared.Dto.Detail.DetailChild;

public class TwoDetailIdDto
{
    [Range(1, int.MaxValue, ErrorMessage = "Недопустимое значение - n < 0")]
    public int FatherDetailId { get; set; }
    
    [Range(1, int.MaxValue, ErrorMessage = "Недопустимое значение - n < 0")]
    public int ChildDetailId { get; set; }
}
