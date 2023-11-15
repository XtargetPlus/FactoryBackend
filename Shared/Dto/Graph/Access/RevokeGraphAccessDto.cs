namespace Shared.Dto.Graph.Access;

public class RevokeGraphAccessDto
{
    public int GraphId { get; set; }
    public List<int> UsersId { get; set; }
}