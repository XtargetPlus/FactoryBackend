using DB.Model.AccessoryInfo;
using DB.Model.StatusInfo;
using DB.Model.StorageInfo;
using DB.Model.StorageInfo.Graph;
using DB.Model.SubdivisionInfo;
using DB.Model.SubdivisionInfo.EquipmentInfo;
using DB.Model.TechnologicalProcessInfo;
using DB.Model.UserInfo.RoleInfo;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DB.Model.UserInfo;

/// <summary>
/// Пользователь
/// </summary>
public class User : BaseModel, IValidatableObject
{
    /// <summary>
    /// Имя
    /// </summary>
    public string FirstName { get; set; } = null!;
    /// <summary>
    /// Фамилия
    /// </summary>
    public string LastName { get; set; } = null!;
    /// <summary>
    /// Отчество
    /// </summary>
    public string FathersName { get; set; } = null!;
    /// <summary>
    /// ФИО
    /// </summary>
    public string FFL { get; set; } = null!;
    /// <summary>
    /// Табельный номер
    /// </summary>
    public string ProfessionNumber { get; set; } = null!;
    /// <summary>
    /// Пароль
    /// </summary>
    public string? Password { get; set; } = null!;
    /// <summary>
    /// Подразделение
    /// </summary>
    public int SubdivisionId { get; set; }
    public Subdivision Subdivision { get; set; } = null!;
    /// <summary>
    /// Профессия
    /// </summary>
    public int ProfessionId { get; set; }
    public Profession Profession { get; set; } = null!;
    /// <summary>
    /// Статус работника
    /// </summary>
    public int StatusId { get; set; }
    public Status Status { get; set; } = null!;
    /// <summary>
    /// Роль
    /// </summary>
    public int? RoleId { get; set; }
    public Role? Role { get; set; }
    public List<TechnologicalProcess>? TechnologicalProcesses { get; set; }
    public List<AccessoryDevelopmentStatus>? AccessoryDevelopmentStatuses { get; set; }
    public List<TechnologicalProcessStatus>? TechnologicalProcessStatuses { get; set; }
    public List<Accessory>? Accessories { get; set; }
    public List<EquipmentStatusValue>? EquipmentStatusValues { get; set; }
    public List<EquipmentStatusUser>? EquipmentStatusUsers { get; set; }
    public List<MoveDetail>? MoveDetails { get; set; }
    public List<OperationGraph>? ConfirmedGraphs { get; set; }
    public List<OperationGraph>? OperationGraphs { get; set; }
    public List<OperationGraphUser>? OperationGraphUsers { get; set; }

    public User() { }
    public User(string firstName, string lastName, string fathersName, string professionNumber)
    {
        FirstName = firstName;
        LastName = lastName;
        FathersName = fathersName;
        ProfessionNumber = professionNumber;
    }

    /// <summary>
    /// Валидация данных на низком уровне
    /// </summary>
    /// <param name="validationContext">Текущий контекст запроса</param>
    /// <returns>Список ошибок с названием полей, которые не прошли валидацию</returns>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (FirstName.Length < 1)
            yield return new ValidationResult("Длина имени минимум 1 символ", new[] { nameof(FirstName) });
        if (FirstName.Length > 50)
            yield return new ValidationResult("Длина имени максимум 50 символ", new[] { nameof(FirstName) });

        if (LastName.Length < 1)
            yield return new ValidationResult("Длина фамилии минимум 1 символ", new[] { nameof(LastName) });
        if (LastName.Length > 50)
            yield return new ValidationResult("Длина фамилии максимум 50 символ", new[] { nameof(LastName) });

        if (FathersName.Length < 1)
            yield return new ValidationResult("Длина отчества минимум 1 символ", new[] { nameof(FathersName) });
        if (FathersName.Length > 50)
            yield return new ValidationResult("Длина отчества максимум 50 символ", new[] { nameof(FathersName) });

        if (FFL.Length < 1)
            yield return new ValidationResult("Длина ФИО минимум 1 символ", new[] { nameof(FFL) });
        if (FFL.Length > 50)
            yield return new ValidationResult("Длина ФИО максимум 50 символ", new[] { nameof(FFL) });

        if (ProfessionNumber.Length < 1)
            yield return new ValidationResult("Длина табельного номера минимум 1 символ", new[] { nameof(ProfessionNumber) });
        if (ProfessionNumber.Length > 10)
            yield return new ValidationResult("Длина табельного номера максимум 10 символ", new[] { nameof(ProfessionNumber) });

        if (Password?.Length < 1)
            yield return new ValidationResult("Длина пароля минимум 1 символ", new[] { nameof(Password) });
        if (Password?.Length > 20)
            yield return new ValidationResult("Длина пароля максимум 20 символ", new[] { nameof(Password) });

        if (validationContext.GetService(typeof(DbContext)) is not DbContext context) yield break;
        
        if (context.Set<User>().Any(u => u.Id != Id && u.ProfessionNumber == ProfessionNumber))
            yield return new ValidationResult("Пользовать с данным табельным номером уже существует", new[] { nameof(ProfessionNumber) });

        if (!context.Set<Subdivision>().Any(s => s.Id == SubdivisionId))
            yield return new ValidationResult("Попытка прикрепить пользователю несуществующее подразделение", new[] { nameof(SubdivisionId) });
        if (!context.Set<Profession>().Any(p => p.Id == ProfessionId))
            yield return new ValidationResult("Попытка прикрепить пользователю несуществующую должность", new[] { nameof(ProfessionId) });
        if (!context.Set<Status>().Any(s => s.Id == StatusId))
            yield return new ValidationResult("Попытка прикрепить пользователю несуществующий статус", new[] { nameof(StatusId) });
        if (RoleId != null && !context.Set<Role>().Any(r => r.Id == RoleId))
            yield return new ValidationResult("Попытка прикрепить пользователю несуществующую роль", new[] { nameof(RoleId) });
    }
}
