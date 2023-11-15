using DB.Model.AccessoryInfo;
using DB.Model.DetailInfo;
using DB.Model.ProductInfo;
using DB.Model.StatusInfo;
using DB.Model.StorageInfo;
using DB.Model.StorageInfo.Graph;
using DB.Model.SubdivisionInfo;
using DB.Model.SubdivisionInfo.EquipmentInfo;
using DB.Model.TechnologicalProcessInfo;
using DB.Model.TechnologicalProcessInfo.TechnologicalProcessDataInfo;
using DB.Model.UserInfo;
using DB.Model.UserInfo.RoleInfo;
using DB.Model.WorkInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace DB;

public class DbApplicationContext : DbContext
{
    public DbSet<TechnologicalProcessData> TechnologicalProcessData { get; set; } = null!;
    public DbSet<TechnologicalProcessItem> TechnologicalProcessItems { get; set; } = null!;
    public DbSet<TechnologicalProcess> TechnologicalProcesses { get; set; } = null!;
    public DbSet<Operation> Operations { get; set; } = null!;

    public DbSet<EquipmentPlan> EquipmentPlans { get; set; } = null!;
    public DbSet<WorkingPart> WorkingParts { get; set; } = null!;
    public DbSet<EquipmentSchedule> EquipmentSchedules { get; set; } = null!;

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Role> Roles { get; set; } = null!;
    public DbSet<UserForm> UserForms { get; set; } = null!;
    public DbSet<RoleClient> RoleClients { get; set; } = null!;
    public DbSet<Profession> Professions { get; set; } = null!;

    public DbSet<Subdivision> Subdivisions { get; set; } = null!;
    public DbSet<EquipmentDetail> EquipmentDetails { get; set; } = null!;
    public DbSet<EquipmentParam> EquipmentParams { get; set; } = null!;
    public DbSet<EquipmentParamValue> EquipmentParamValues { get; set; } = null!;
    public DbSet<EquipmentDetailContent> EquipmentDetailContents { get; set; } = null!;
    public DbSet<EquipmentDetailReplacement> EquipmentDetailReplacements { get; set; } = null!;
    public DbSet<EquipmentFailure> EquipmentFailures { get; set; } = null!;
    public DbSet<EquipmentStatus> EquipmentStatuses { get; set; } = null!;
    public DbSet<EquipmentStatusValue> EquipmentStatusValues { get; set; } = null!;
    public DbSet<EquipmentStatusUser> EquipmentStatusUsers { get; set; } = null!;
    public DbSet<Equipment> Equipments { get; set; } = null!;
    public DbSet<EquipmentOperation> EquipmentsOperations { get; set; } = null!;

    public DbSet<Storage> Storages { get; set; } = null!;
    public DbSet<StoragePlace> StoragePlaces { get; set; } = null!;
    public DbSet<StoragePlaceMoveDetail> StoragePlaceMoveDetails { get; set; } = null!;
    public DbSet<OperationGraph> OperationGraphs { get; set; } = null!;
    public DbSet<OperationGraphDetail> OperationGraphDetails { get; set; } = null!;
    public DbSet<OperationGraphDetailGroup> OperationGraphDetailGroups { get; set; } = null!;
    public DbSet<OperationGraphDetailItem> OperationGraphDetailItems { get; set; } = null!;
    public DbSet<OperationGraphGroup> OperationGraphGroups { get; set; } = null!;
    public DbSet<OperationGraphUser> OperationGraphUsers { get; set; } = null!;
    public DbSet<MoveType> MoveTypes { get; set; } = null!;
    public DbSet<MoveDetail> MoveDetails { get; set; } = null!;

    public DbSet<TechnologicalProcessStatus> TechnologicalProcessStatuses { get; set; } = null!;
    public DbSet<Status> Statuses { get; set; } = null!;
    public DbSet<AccessoryDevelopmentStatus> AccessoryDevelopmentStatuses { get; set; } = null!;

    public DbSet<Client> Clients { get; set; } = null!;
    public DbSet<ClientProduct> ClientProducts { get; set; } = null!;
    public DbSet<Product> Products { get; set; } = null!;

    public DbSet<BlankType> BlankTypes { get; set; } = null!;
    public DbSet<Detail> Details { get; set; } = null!;
    public DbSet<Unit> Units { get; set; } = null!;
    public DbSet<DetailReplaceability> DetailReplaceabilities { get; set; } = null!;
    public DbSet<DetailType> DetailTypes { get; set; } = null!;
    public DbSet<DetailsChild> DetailsChildren { get; set; } = null!;
    public DbSet<Material> Materials { get; set; } = null!;

    public DbSet<OutsideOrganization> OutsideOrganizations { get; set; } = null!;
    public DbSet<AccessoryType> AccessoryTypes { get; set; } = null!;
    public DbSet<AccessoryEquipment> AccessoryEquipments { get; set; } = null!;
    public DbSet<Accessory> Accessories { get; set; } = null!;

    /// <summary>
    /// Провайдер логирования
    /// </summary>
    private static readonly ILoggerFactory MyLoggerFactory = LoggerFactory.Create(builder =>
    {
        builder.AddProvider(new DbLoggerProvider());
    });

    /// <summary>
    /// Конструктор который использует asp.net (так же можно использовать при тестировании)
    /// </summary>
    /// <param name="options">Настройки базы</param>
    public DbApplicationContext(DbContextOptions<DbApplicationContext> options)
    : base(options) { }

    /// <summary>
    /// Пустой конструктор для тестирования
    /// </summary>
    public DbApplicationContext() { }

    /// <summary>
    /// Настройка контекста базы данных
    /// </summary>
    /// <param name="optionsBuilder">Строитель</param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured)
            optionsBuilder.UseLoggerFactory(MyLoggerFactory);
        else
        {
            optionsBuilder.UseMySql("Server=localhost;User=5p4r3_0l3g;Password=Xtarget.plus_ZeroWolf1;Database=test_plan7;", 
                                    new MySqlServerVersion(new Version(8, 0, 32)));
        }
    }

    /// <summary>
    /// Настройка перед созданием
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        List<Unit> units = new()
        {
            new() { Id = 1, Title = "шт"}
        };

        List<Role> roles = new() {
            new() { Id = 1, Title = "admin" },
            new() { Id = 2, Title = "PDO" },
            new() { Id = 3, Title = "DirectorPDO" },
            new() { Id = 4, Title = "LeadPDO" },
            new() { Id = 5, Title = "DirectorOK" },
            new() { Id = 6, Title = "OK" },
            new() { Id = 7, Title = "technologistDeveloper" },
        };

        List<UserForm> forms = new() {
            new() { Id = 1, Title = "role" },
            new() { Id = 2, Title = "detail_structure" },
            new() { Id = 3, Title = "detail" },
            new() { Id = 4, Title = "detail-type" },
            new() { Id = 5, Title = "detail_swap" },
            new() { Id = 6, Title = "user" },
            new() { Id = 7, Title = "profession" },
            new() { Id = 8, Title = "subdivision" },
            new() { Id = 9, Title = "user-form" },
            new() { Id = 10, Title = "role_client" },
            new() { Id = 11, Title = "product" },
            new() { Id = 12, Title = "move-type" },
            new() { Id = 13, Title = "status" },
            new() { Id = 14, Title = "equipment-failure" },
            new() { Id = 15, Title = "operation" },
            new() { Id = 16, Title = "blank-type" },
        };

        List<RoleClient> roleClients = new()
        {
            new() { UserFormId = forms[0].Id, RoleId = roles[0].Id, Add = true, Edit = true, Delete = true, Browsing = true },
            new() { UserFormId = forms[8].Id, RoleId = roles[0].Id, Add = true, Edit = true, Delete = true, Browsing = true },
            new() { UserFormId = forms[9].Id, RoleId = roles[0].Id, Add = true, Edit = true, Delete = true, Browsing = true },
        };

        List<Status> statuses = new()
        {
            new() { Id = 1, Title = "Работает", TableName = "users" },
            new() { Id = 2, Title = "Не в работе", TableName = "tp" },
            new() { Id = 3, Title = "Уволен", TableName = "users" },
            new() { Id = 4, Title = "В работе", TableName = "tp" },
            new() { Id = 5, Title = "Приостановлен", TableName = "tp" },
            new() { Id = 6, Title = "На согласовании", TableName = "tp" },
            new() { Id = 7, Title = "Возврат на доработку", TableName = "tp" },
            new() { Id = 8, Title = "На выдачу", TableName = "tp" },
            new() { Id = 9, Title = "Выдан", TableName = "tp" },
            new() { Id = 10, Title = "Выдан дубликат", TableName = "tp" },
            new() { Id = 11, Title = "Выполнено", TableName = "tp" },
            new() { Id = 12, Title = "В разработке", TableName = "operGraph" },
            new() { Id = 13, Title = "Активный", TableName = "operGraph" },
            new() { Id = 14, Title = "Приостановлен", TableName = "operGraph" },
            new() { Id = 15, Title = "Завершен", TableName = "operGraph" },
            new() { Id = 16, Title = "Отменен", TableName = "operGraph" },
        };

        List<Profession> professions = new()
        {
            new() { Id = 1, Title = "Инженер-программист"},
            new() { Id = 2, Title = "Инженер-технолог 3 категории"},
            new() { Id = 3, Title = "Инженер-технолог 2 категории"},
            new() { Id = 4, Title = "Инженер-технолог 1 категории"},
            new() { Id = 5, Title = "Техник"},
            new() { Id = 6, Title = "Ведущий инженер-технолог"},
        };

        Subdivision it = new() { Id = 1, Title = "15 Отдел разработки программного обеспечения" };

        List<User> users = new()
        {
            new()
            {
                Id = 1,
                FirstName = "Олег",
                LastName = "Поляков",
                FathersName = "Андреевич",
                FFL = "Поляков О.А.",
                ProfessionNumber = "0001-0001",
                Password = "admin1",
                ProfessionId = professions[0].Id,
                SubdivisionId = it.Id,
                StatusId = statuses[0].Id,
                RoleId = roles[0].Id,
            },
            new()
            {
                Id = 2,
                FirstName = "Андрей",
                LastName = "Скрипка",
                FathersName = "Викторович",
                FFL = "Скрипка А.В.",
                ProfessionNumber = "0001-0002",
                Password = "admin2",
                ProfessionId = professions[0].Id,
                SubdivisionId = it.Id,
                StatusId = statuses[0].Id,
                RoleId = roles[0].Id,
            },
            new()
            {
                Id = 3,
                FirstName = "Иван",
                LastName = "Фролов",
                FathersName = "Алексеевич",
                FFL = "Фролов И.А.",
                ProfessionNumber = "0001-0003",
                Password = "admin3",
                ProfessionId = professions[0].Id,
                SubdivisionId = it.Id,
                StatusId = statuses[0].Id,
                RoleId = roles[0].Id,
            },
            new()
            {
                Id = 4,
                FirstName = "Михаил",
                LastName = "Лобанов",
                FathersName = "Александрович",
                FFL = "Лобанов М.А.",
                ProfessionNumber = "0001-0004",
                Password = "admin4",
                ProfessionId = professions[0].Id,
                SubdivisionId = it.Id,
                StatusId = statuses[0].Id,
                RoleId = roles[0].Id,
            },
            new()
            {
                Id = 5,
                FirstName = "Виктор",
                LastName = "Кулаков",
                FathersName = "Андреевич",
                FFL = "Кулаков В.А.",
                ProfessionNumber = "0001-0005",
                Password = "admin5",
                ProfessionId = professions[0].Id,
                SubdivisionId = it.Id,
                StatusId = statuses[0].Id,
                RoleId = roles[0].Id,
            }
        };

        modelBuilder.Entity<Unit>().HasData(units);
        modelBuilder.Entity<UserForm>().HasData(forms);
        modelBuilder.Entity<Role>().HasData(roles);
        modelBuilder.Entity<RoleClient>().HasData(roleClients);
        modelBuilder.Entity<Status>().HasData(statuses);
        modelBuilder.Entity<Profession>().HasData(professions);
        modelBuilder.Entity<Subdivision>().HasData(it);
        modelBuilder.Entity<User>().HasData(users);
    }
}
