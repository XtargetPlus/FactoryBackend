namespace Plan7.Helper.Controllers.Roles.TechnologicalProcesses;

public static class TpReadonlyControllerRoles
{
    public const string GetAllTps = "admin, trainee, technologistDeveloper, DirectorTechnologist";
    public const string GetAllTpItems = "admin, trainee, technologistDeveloper, DirectorTechnologist";
    public const string GetAllTpOperationEquipments = "admin, trainee, technologistDeveloper, DirectorTechnologist";
    public const string GetDevelopmentStages = "admin, trainee, technologistDeveloper, DirectorTechnologist";
    public const string GetNumberOfBrunches = "admin, trainee, technologistDeveloper, DirectorTechnologist";
    public const string GetInfo = "admin, trainee, technologistDeveloper, DirectorTechnologist";
    public const string GetAllCompletedDeveloperTps = "admin, technologistDeveloper, DirectorTechnologist";
    public const string GetAllTpsReadyForDelivery = "admin, technologistDeveloper";
    public const string GetAllIssuedTechProcesses = "admin, technologistDeveloper";
    public const string GetAllIssuedDuplicatesTechProcess = "admin, technologistDeveloper";
    public const string GetAllIssuedTechProcessesFromTechnologist = "admin, technologistDeveloper, DirectorTechnologist";
    public const string GetStatusChangeOptions = "admin, technologistDeveloper, DirectorTechnologist";
    public const string GetStatuses = "admin, technologistDeveloper, DirectorTechnologist";
    public const string GetDeveloperTasks = "admin, technologistDeveloper, DirectorTechnologist";
}
