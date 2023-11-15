namespace Plan7.Helper.Controllers.Roles;

public static class ProductControllerRoles
{
    public const string Add = "admin, trainee";
    public const string Change = "admin, trainee";
    public const string Delete = "admin, trainee";
    public const string GetAll = "admin, trainee";
    public const string GetAllForChoice = "admin, trainee, technologistDeveloper, DirectorTechnologist";
}
