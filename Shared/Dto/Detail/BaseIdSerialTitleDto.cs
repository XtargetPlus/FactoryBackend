namespace Shared.Dto.Detail;

public class BaseIdSerialTitleDto : BaseSerialTitleDto, IEquatable<BaseIdSerialTitleDto>
{
    public int DetailId { get; set; }

    public bool Equals(BaseIdSerialTitleDto? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return DetailId.Equals(other.DetailId) && SerialNumber.Equals(other.SerialNumber);
    }

    public override int GetHashCode()
    {
        int hashId = DetailId.GetHashCode();
        int hashSerialNumber = SerialNumber.GetHashCode();
        return hashId ^ hashSerialNumber;
    }
}
