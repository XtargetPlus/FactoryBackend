namespace Plan7.Helper.Controllers.Roles;

public static class UserControllerRoles
{
    public const string Add = "admin, OK, trainee"; 
    public const string ChangeRoleWithPassword = "admin";
    public const string Change = "admin, OK, trainee";
    public const string Delete = "admin, OK";
    public const string GetAll = "admin, OK, DirectorOK, trainee";
    public const string GetAllFromProfession = "admin, OK, DirectorOK, trainee";
    public const string GetAllFromSubdivision = "admin, OK, DirectorOK, trainee, technologistDeveloper";
    public const string GetMoreInfo = "admin, OK, DirectorOK, trainee";
    public const string GetAddInfo = "admin, OK, trainee";
    public const string GetAllTechnologistsDevelopers = "admin";
    public const string GetUserRole = "admin";
}
