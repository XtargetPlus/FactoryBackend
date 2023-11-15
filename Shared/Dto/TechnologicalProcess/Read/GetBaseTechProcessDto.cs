namespace Shared.Dto.TechnologicalProcess;

public class GetBaseTechProcessDto : IEquatable<GetBaseTechProcessDto>
{
    public int TechProcessId { get; set; }
    public string SerialNumber { get; set; } = default!;
    public string Title { get; set; } = default!;
    public DateOnly Date { get; set; }
    public bool IsActual { get; set; }

    public bool Equals(GetBaseTechProcessDto? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return TechProcessId.Equals(other.TechProcessId) && SerialNumber.Equals(other.SerialNumber);
    }

    public override int GetHashCode()
    {
        var hashId = TechProcessId.GetHashCode();
        var hashSerialNumber = SerialNumber.GetHashCode();
        return hashId ^ hashSerialNumber;
    }
}
