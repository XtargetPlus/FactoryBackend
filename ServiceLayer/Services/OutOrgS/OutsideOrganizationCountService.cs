using DatabaseLayer.IDbRequests;
using DB;
using DB.Model.AccessoryInfo;
using ServiceLayer.IServicesRepository.IOutsideOrganizationCountServices;

namespace ServiceLayer.Services.OutOrgS;

/// <summary>
/// Сервис подсчета количества сторонних организаций с фильтрами и без
/// </summary>
public class OutsideOrganizationCountService : IOutsideOrganizationCountService
{
    private readonly DbApplicationContext _context;
    private readonly CountToMainForm<OutsideOrganization> _count;

    public OutsideOrganizationCountService(DbApplicationContext context)
    {
        _context = context;
        _count = new(_context);
    }

    /// <summary>
    /// Получаем количество 
    /// </summary>
    /// <returns>Количество</returns>
    public async Task<int?> GetAllAsync() => await _count.CountAsync();

    public void Dispose() => _context.Dispose();
}
