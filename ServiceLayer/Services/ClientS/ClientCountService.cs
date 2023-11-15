using DatabaseLayer.IDbRequests;
using DB;
using ServiceLayer.IServicesRepository.IClientServices;
using DB.Model.ProductInfo;

namespace ServiceLayer.Services.ClientS;

/// <summary>
/// Сервис подсчета количества клиентов
/// </summary>
public class ClientCountService : IClientCountService
{
    private readonly DbApplicationContext _context;
    private readonly CountToMainForm<Client> _count;

    public ClientCountService(DbApplicationContext context)
    {
        _context = context;
        _count = new(_context);
    }

    /// <summary>
    /// Получаем количество клиентов
    /// </summary>
    /// <returns>Количество клиентов int</returns>
    public async Task<int?> GetAllAsync() => await _count.CountAsync();

    public void Dispose() => _context.Dispose();
}
