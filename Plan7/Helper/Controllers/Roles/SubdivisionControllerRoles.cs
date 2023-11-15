namespace Plan7.Helper.Controllers.Roles;

public static class SubdivisionControllerRoles
{
    public const string Add = "admin, OK, trainee";
    public const string Change = "admin, OK, trainee";
    public const string Delete = "admin, OK, trainee";
    public const string GetAllLevel = "admin, OK, DirectorOK, trainee";
    public const string GetAll = "admin, Ok, DirectorOK, trainee, technologistDeveloper";
    public const string GetAllByEquipmentContain = "admin, OK, trainee, technologistDeveloper";
    public const string GetAllWithoutTechProcess = "admin, technologistDeveloper";
}
