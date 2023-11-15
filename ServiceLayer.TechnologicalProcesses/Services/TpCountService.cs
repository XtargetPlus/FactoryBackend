using DB;
using DatabaseLayer.DbRequests.TechnologicalProcessToDb;
using Shared.Dto.TechnologicalProcess;
using DatabaseLayer.IDbRequests;
using DB.Model.StatusInfo;
using Shared.Static;
using BizLayer.Repositories.DetailR;
using ServiceLayer.TechnologicalProcesses.Services.Interfaces;
using AutoMapper;
using Shared.Dto.Detail;

namespace ServiceLayer.TechnologicalProcesses.Services;

/// <summary>
/// Сервис подсчета количества технических процессов с условиями или без
/// </summary>
public class TpCountService : ITpCountService
{
    private readonly DbApplicationContext _context;
    private readonly IMapper _dataMapper;

    public TpCountService(DbApplicationContext context, IMapper dataMapper)
    {
        _context = context;
        _dataMapper = dataMapper;
    }

    /// <summary>
    /// Получаем количество технических процессов с учетом применяемых фильтров
    /// </summary>
    /// <param name="filters">Фильтры для выборки</param>
    /// <returns>Количество тех процессов</returns>
    public async Task<int?> GetAllAsync(GetAllReadonlyTechProcessRequestFilters filters)
    {
        List<int> productDetailsId = new();
        if (filters.ProductId > 0)
            productDetailsId = (await new DetailRepository(new(), _dataMapper).GetAllProductDetailsAsync(new GetAllProductDetailsDto
            {
                DetailId = filters.ProductId,
                IsHardDetail = true
            }, _context) ?? new()).Select(d => d.DetailId).ToList();
        return await new TpCountRequests(_context).GetAllAsync(filters, productDetailsId);
    }

    /// <summary>
    /// Получаем общее количество выданных тех процессов
    /// </summary>
    /// <param name="filters">Фильтры выборки, влияющие на подсчет</param>
    /// <returns>Общее количество выданных тех процессов</returns>
    public async Task<int?> GetAllIssuedAsync(GetAllIssuedTechProcessesRequestFilters filters)
    {
        List<int> productDetailsId = new();
        if (filters.ProductId > 0)
            productDetailsId = (await new DetailRepository(new(), _dataMapper).GetAllProductDetailsAsync(new GetAllProductDetailsDto
            {
                DetailId = filters.ProductId,
                IsHardDetail = true
            }, _context) ?? new()).Select(d => d.DetailId).ToList();
        return await new TpCountRequests(_context).GetAllIssuedAsync(filters, productDetailsId);
    }

    /// <summary>
    /// Получаем общее количество выданных дубликатов тех процесса, которые выполнил технолог
    /// </summary>
    /// <param name="developerId">Id технолога</param>
    /// <returns>Общее количество выдач</returns>
    public async Task<int?> GetAllIssuedFromTechnologistAsync(int developerId) =>
        await new CountToMainForm<TechnologicalProcessStatus>(_context).CountAsync(filter: tps => tps.TechnologicalProcess!.DeveloperId == developerId 
                                                                                                  && tps.StatusId == (int)TechProcessStatuses.IssuedDuplicate);

    /// <summary>
    /// Получаем общее количество выполненных технологом тех процессов
    /// </summary>
    /// <param name="developerId">Id технолога</param>
    /// <returns>Общее количество выполненных тех процессов</returns>
    public async Task<int?> GetAllCompletedDeveloperTpsAsync(int developerId) =>
        await new CountToMainForm<TechnologicalProcessStatus>(_context).CountAsync(
            filter: tps => tps.TechnologicalProcess!.DeveloperId == developerId && tps.StatusId == (int)TechProcessStatuses.Completed,
            false);

    public void Dispose() => _context.Dispose();  
}
