namespace Shared.Dto.Graph.Access;

public class GetAllUserGraphAccessDto
{
    public int UserId { get; set; }
    public string FFL { get; set; }
    public string ProfessionNumber { get; set; }
    public string Subdivision { get; set; }
    public string Access { get; set; }
}