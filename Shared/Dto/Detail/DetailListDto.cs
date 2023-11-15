namespace Shared.Dto.Detail;

public class DetailListDto : DetailMoreInfoDto
{
    public List<BaseDto>? DetailTypes { get; set; }
    public List<BaseDto>? Units { get; set; }
}
