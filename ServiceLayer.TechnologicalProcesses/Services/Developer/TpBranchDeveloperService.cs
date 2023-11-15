using DatabaseLayer.IDbRequests;
using DB;
using DB.Model.TechnologicalProcessInfo;
using Shared.Dto.TechnologicalProcess;
using Shared.Dto.TechnologicalProcess.TechProcessItem.Branch.CUD;
using Shared.Enums;
using BizLayer.Repositories.TechnologicalProcessR.TechnologicalProcessItemR;
using DatabaseLayer.Helper;
using DatabaseLayer.DatabaseChange;
using ServiceLayer.TechnologicalProcesses.Services.Interfaces.IDeveloper;
using AutoMapper;
using BizLayer.Repositories.TechnologicalProcessR;

namespace ServiceLayer.TechnologicalProcesses.Services.Developer;

public class TpBranchDeveloperService : ErrorsMapper, ITpBranchDeveloperService
{
    private readonly DbApplicationContext _context;
    private readonly BaseModelRequests<TechnologicalProcessItem> _repository;
    private readonly IMapper _dataMapper;

    public TpBranchDeveloperService(DbApplicationContext context, IMapper dataMapper)
    {
        _context = context;
        _dataMapper = dataMapper;
        _repository = new(_context, _dataMapper);
    }

    /// <summary>
    /// Создание ветки для операции технического процесса
    /// </summary>
    /// <param name="dto">Информация для создания ветки</param>
    /// <returns>Операция заглушка для немедленного редактирования или null (ошибки с предупреждениями)</returns>
    public async Task<(int techProcessItemId, int branch)?> AddAsync(AddBranchWithItemDto dto)
    {
        TpReadonlyService techProcessReadonlyService = new(_context, _dataMapper);

        var techProcessItem = await TechProcessItemSimpleRead.GetAsync(_repository, dto.MainTechProcessItemId, this);
        if (HasErrors)
            return null;

        if (techProcessItem!.TechnologicalProcessId != dto.NewItemInformation.TechProcessId)
            AddErrors("Связь операций тех процесса из разных тех процессов");
        var branchNumbers = await techProcessReadonlyService.GetNumberOfBranchesAsync(techProcessItem.Id, QueryFilterOptions.TurnOff);
        if (techProcessReadonlyService.HasErrors)
            AddErrors(string.Join("\n", techProcessReadonlyService.Errors));
        if (branchNumbers == null)
            AddErrors("Не удалось получить ветки тех процесса");

        if (HasErrors)
            return null;

        var branch = branchNumbers!.Count == 0 ? techProcessItem.Priority + 1 : branchNumbers.MaxBy(x => x) + 1;

        if (branch % 5 == 0)
        {
            AddErrors($"Выход за пределы допустимого количества веток операции {techProcessItem.OperationNumber}, допустимое количество веток - 4");
            return null;
        }

        TechnologicalProcessItem? newTechProcessItem = new()
        {
            Number = 1,
            OperationNumber = dto.NewItemInformation.OperationNumber,
            OperationId = dto.NewItemInformation.OperationId,
            Priority = branch,
            Count = dto.NewItemInformation.Count,
            Show = true,
            TechnologicalProcessId = dto.NewItemInformation.TechProcessId,
            MainTechnologicalProcessItemId = techProcessItem.Id,
            Note = dto.NewItemInformation.Note,
        };
        
        newTechProcessItem = await _context.AddWithValidationsAndSaveAsync(newTechProcessItem, this);
        return (newTechProcessItem?.Id ?? 0, branch);
    }

    /// <summary>
    /// Перемещение операций из главной ветки в новое ответвления другой операции тех процесса
    /// </summary>
    /// <param name="dto">Информация для перемещения</param>
    /// <returns></returns>
    public async Task FromMainToNewBranchAsync(FromMainToNewBranchDto dto)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        if (dto.MainItemsId.Contains(dto.MainTechProcessItemId))
            AddErrors("Главная операция содержится в списке операций на добавление в ветку");

        var mainItem = await TechProcessItemSimpleRead.GetAsync(_repository, dto.MainTechProcessItemId, this);
        var branchNumbers = await new TpReadonlyService(_context, _dataMapper).GetNumberOfBranchesAsync(dto.MainTechProcessItemId, QueryFilterOptions.TurnOff);

        var newBranchItems = await new TechProcessItemHardRead(_repository, this)
                                                                   .GetMainWithIncludeBranchesItems(mainItem?.TechnologicalProcessId ?? 0, dto.MainItemsId, QueryFilterOptions.TurnOff);

        await TechProcessBranchValidations.FromMainToNewBranchValidation(branchNumbers, newBranchItems, _context, this);

        if (HasErrors)
            return;

        for (var i = 0; i < newBranchItems!.Count; i++)
        {
            newBranchItems[i].Priority = branchNumbers![^1] + 1;
            newBranchItems[i].Number = i + 1;
            newBranchItems[i].MainTechnologicalProcessItemId = mainItem!.Id;
        }

        await _context.SaveChangesWithValidationsAsync(this);

        await new TechProcessItemRepository(_repository, this).NumberAndPriorityRecalculationAsync(mainItem!.TechnologicalProcessId);

        await _context.SaveChangesWithValidationsAsync(this);

        if (HasErrors || HasWarnings)
            return;

        await transaction.CommitAsync();
    }

    /// <summary>
    /// Перемещение из главной ветки в существующую ветку перед операцией ответвления с перерасчетом номеров и приоритетов
    /// </summary>
    /// <param name="dto">Информация для перемещения</param>
    /// <returns></returns>
    public async Task FromMainToBranchAsync(FromMainToBranchDto dto)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        TechProcessItemHardRead hardRead = new(_repository, this);

        var insertedItem = (await hardRead.GetMainWithIncludeBranchesItems(dto.TechProcessId, new List<int> { dto.CurrentTargetMainItemId }))?.SingleOrDefault();
        var branchItems = await hardRead.GetBranchItemsAsync(dto.MainTechProcessItemId, dto.Branch, QueryFilterOptions.TurnOff);

        await TechProcessBranchValidations.FromMainToBranchValidationAsync(insertedItem, branchItems, dto.BeforeBranchItemId, _context, this);

        if (HasErrors)
            return;

        var targetBranchItemNumber = branchItems!.Where(item => item.Id == dto.BeforeBranchItemId).Select(item => item.Number).FirstOrDefault();
        branchItems = branchItems!.Where(item => item.Number > targetBranchItemNumber).ToList();

        var oldPriority = insertedItem!.Priority;

        insertedItem.Number = targetBranchItemNumber + 1;
        insertedItem.Priority = dto.Branch;
        insertedItem.MainTechnologicalProcessItemId = dto.MainTechProcessItemId;

        branchItems.ForEach(item => item.Number++);

        await _context.SaveChangesWithValidationsAsync(this);

        if (await new TechProcessItemRepository(_repository, this).RecalculationWithHigherPriorityAsync(insertedItem.TechnologicalProcessId, oldPriority) == 1)
            await _context.SaveChangesWithValidationsAsync(this);

        if (HasErrors || HasWarnings)
            return;

        await transaction.CommitAsync();
    }

    /// <summary>
    /// Перемещения всех операций ответвления в конец главной ветки
    /// </summary>
    /// <param name="dto">Информация для перемещения</param>
    /// <returns></returns>
    public async Task GetBranchToMainAsync(BaseBranchDto dto)
    {
        TechProcessItemHardRead hardRead = new(_repository, this);

        var branchItems = await hardRead.GetBranchItemsAsync(dto.MainTechProcessItemId, dto.Branch);
        if (HasErrors)
            return;

        var lastMainBranchItem = (await hardRead.GetMainItemsWithRangeForNumberAsync(int.MaxValue, 0, branchItems!.First().TechnologicalProcessId))
            ?.MaxBy(tpi => tpi.Number);
        
        var biggerBranchesItems = await hardRead.GetBranchesItemsAsync(
            dto.MainTechProcessItemId,
            hardRead.GetBranchNumbersWithRequirement(
                        await new TpReadonlyService(_context, _dataMapper).GetNumberOfBranchesAsync(dto.MainTechProcessItemId, QueryFilterOptions.TurnOff) ?? new(),
                        b => b > dto.Branch),
            QueryFilterOptions.TurnOff);
        
        biggerBranchesItems?.ForEach(item => item.Priority--);

        if (HasErrors)
            return;

        for (var i = 0; i < branchItems!.Count; i++)
        {
            branchItems[i].Number = lastMainBranchItem!.Number + i + 1;
            branchItems[i].Priority = lastMainBranchItem.Priority + (5 * (i + 1));
            branchItems[i].MainTechnologicalProcessItemId = null;
        }

        await _context.SaveChangesWithValidationsAsync(this);
    }

    /// <summary>
    /// Перемещение операции ветки тех процесса в главную ветку перед определенной операции с перерасчетом приоритетов и номеров
    /// </summary>
    /// <param name="dto">Информация для перемещения</param>
    /// <returns></returns>
    public async Task GetBranchItemToMainAsync(GetBranchItemToMainDto dto)
    {
        var isFirstItemIsNotZero = dto.BeforeMainItemId > 0;

        var firstItem = isFirstItemIsNotZero ? await TechProcessItemSimpleRead.GetWithTechProcessIdAsync(_repository, dto.TechProcessId, dto.BeforeMainItemId, this) : null;
        var secondItem = await TechProcessItemSimpleRead.GetWithTechProcessIdAsync(_repository, dto.TechProcessId, dto.CurrentTargetBranchItemId, this);

        if (secondItem?.MainTechnologicalProcessItemId is null)
            AddErrors("Передвигаемая операция тех процесса находится не в ответвлении");
        if (isFirstItemIsNotZero && firstItem?.MainTechnologicalProcessItemId is not null)
            AddErrors("Операция главной ветки является операцией ответвления");

        if (HasErrors)
            return;

        TechProcessItemHardRead hardRead = new(_repository, this);

        var countBranchItems = (await hardRead.GetBranchItemsAsync((int)secondItem!.MainTechnologicalProcessItemId!, secondItem.Priority))?.Count;

        if (countBranchItems is 1)
        {
            var branchesItems = await hardRead.GetBranchesItemsAsync(
                (int)secondItem.MainTechnologicalProcessItemId,
                hardRead.GetBranchNumbersWithRequirement(
                    await new TpReadonlyService(_context, _dataMapper).GetNumberOfBranchesAsync((int)secondItem.MainTechnologicalProcessItemId, QueryFilterOptions.TurnOff) ?? new(), 
                    b => b > secondItem.Priority),
                QueryFilterOptions.TurnOff);

            branchesItems?.ForEach(item => item.Priority--);
        }
        else
        {
            await new TechProcessItemRepository(_repository, this)
                .RecalculateBranchItemsAsync(secondItem.MainTechnologicalProcessItemId.Value, secondItem.Id, secondItem.Priority, this);

            if (HasErrors)
                return;
        }

        secondItem.MainTechnologicalProcessItemId = null;
        secondItem.Number = isFirstItemIsNotZero ? firstItem!.Number + 1 : 1;
        secondItem.Priority = isFirstItemIsNotZero ? firstItem!.Priority + 5 : 5;

        var biggestNumberItems = await _repository.GetAllAsync(
            filter: tpi => tpi.TechnologicalProcessId == dto.TechProcessId
                           && tpi.MainTechnologicalProcessItemId == null
                           && (dto.BeforeMainItemId == 0 || tpi.Number > firstItem!.Number),
            trackingOptions: TrackingOptions.WithTracking,
            addQueryFilters: Convert.ToBoolean((int)QueryFilterOptions.TurnOff));

        foreach (var item in biggestNumberItems ?? new())
        {
            item.Number++;
            item.Priority += 5;
            var branchesItems = await hardRead.GetBranchesItemsWithoutValidationAsync(item.TechnologicalProcessId, item.Id, QueryFilterOptions.TurnOff);
            branchesItems?.ForEach(item => item.Priority += 5);
        }
        if (HasErrors) 
            return;

        await _context.SaveChangesWithValidationsAsync(this);
    }

    /// <summary>
    /// Меняем ветки операции технического процесса местами
    /// </summary>
    /// <param name="dto">Информация о ветках для изменения</param>
    /// <returns></returns>
    public async Task SwapAsync(SwapBranchesDto dto)
    {
        TechProcessItemHardRead hardRead = new(_repository, this);

        (await hardRead.GetBranchItemsAsync(dto.MainTechProcessItemId, dto.FirstBranch, QueryFilterOptions.TurnOff))?.ForEach(tpi => tpi.Priority = dto.SecondBranch);
        (await hardRead.GetBranchItemsAsync(dto.MainTechProcessItemId, dto.SecondBranch, QueryFilterOptions.TurnOff))?.ForEach(tpi => tpi.Priority = dto.FirstBranch);

        if (HasErrors)
            return;
        await _context.SaveChangesWithValidationsAsync(this);
    }

    /// <summary>
    /// Вставка ветки операции тех процесса между двумя другими ветками той же операции
    /// </summary>
    /// <param name="dto">Информация для вставки</param>
    /// <returns></returns>
    public async Task InsertBetweenAsync(InsertBetweenBranchDto dto)
    {
        if (dto.BeforeBranch == dto.CurrentTargetBranch)
            AddErrors("Ветки №1 и №2 одинаковы");

        TechProcessItemHardRead hardRead = new(_repository, this);

        var targetBranchItems = await hardRead.GetBranchItemsAsync(dto.MainTechProcessItemId, dto.CurrentTargetBranch, QueryFilterOptions.TurnOff);
        if (HasErrors)
            return;

        var isBeforeBranchNotNull = dto.BeforeBranch != 0;
        var oldBranch = dto.CurrentTargetBranch;
        var newBranch = isBeforeBranchNotNull ? dto.BeforeBranch : dto.CurrentTargetBranch / 5 * 5 + 1;

        var branchesItems = await hardRead.GetBranchesItemsAsync(
            dto.MainTechProcessItemId,
            hardRead.GetBranchNumbersWithRequirement(
                    await new TpReadonlyService(_context, _dataMapper).GetNumberOfBranchesAsync(dto.MainTechProcessItemId, QueryFilterOptions.TurnOff) ?? new(),
                    b => oldBranch > newBranch ? (b >= newBranch && b < oldBranch) : (b <= newBranch && b > oldBranch)),
            QueryFilterOptions.TurnOff);
        if (HasErrors)
            return;

        branchesItems!.ForEach(item => item.Priority += oldBranch > newBranch ? 1 : -1);
        targetBranchItems!.ForEach(tpi => tpi.Priority = newBranch);

        await _context.SaveChangesWithValidationsAsync(this);
    }

    /// <summary>
    /// Сокрытие или раскрытие ветки вместе с ее операциями
    /// </summary>
    /// <param name="dto">Информация для сокрытия или раскрытия ветки</param>
    /// <returns></returns>
    public async Task HideOrUncoverAsync(HideOrUncoverBranchDto dto)
    {
        TechProcessItemHardRead hardRead = new(_repository, this);
        var targetBranchItems = await hardRead.GetBranchItemsAsync(dto.MainTechProcessItemId, dto.Branch, QueryFilterOptions.TurnOff);

        if (dto.Visibility) 
            await TechProcessBranchValidations.HideItemsValidationAsync(targetBranchItems, _context, this);

        if (HasErrors)
            return; 

        targetBranchItems!.ForEach(tpi => tpi.Show = dto.Visibility);

        await _context.SaveChangesWithValidationsAsync(this);
    }

    /// <summary>
    /// Удаление ветки со всеми ее операциями
    /// </summary>
    /// <param name="dto">Информация об ветке для удаления</param>
    /// <returns></returns>
    public async Task DeleteWithItemsAsync(BaseBranchDto dto)
    {
        TpReadonlyService techProcessReadonlyService = new(_context, _dataMapper);
        TechProcessItemHardRead hardRead = new(_repository, this);

        await using var transaction = await _context.Database.BeginTransactionAsync();

        _repository.Remove(await hardRead.GetBranchItemsAsync(dto.MainTechProcessItemId, dto.Branch, QueryFilterOptions.TurnOff));
        await _context.SaveChangesWithValidationsAsync(this);

        // Получаем список веток операции TpiId, и заносим только те ветки, которые младше по приоритету ветки Branch
        var branchNumbers = (await techProcessReadonlyService.GetNumberOfBranchesAsync(dto.MainTechProcessItemId) ?? new()).Where(i => i > dto.Branch).ToList();
        if (techProcessReadonlyService.HasErrors)
        {
            AddErrors(string.Join("\n", techProcessReadonlyService.Errors));
            return;
        }
        if (branchNumbers.Count > 0)
        {
            var branchesItems = await hardRead.GetBranchesItemsAsync(
                dto.MainTechProcessItemId,
                branchNumbers,
                QueryFilterOptions.TurnOff);
            
            branchesItems?.ForEach(item => item.Priority--);
            
            if (HasErrors)
                return;
            await _context.SaveChangesWithValidationsAsync(this);
        }

        if (_repository.HasWarnings)
            AddWarnings(_repository.Warnings);
        if (HasErrors || HasWarnings)
            return;

        await transaction.CommitAsync();
    }

    public void Dispose() => _context.Dispose();
}
