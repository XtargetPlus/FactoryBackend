using System.ComponentModel;
using AutoMapper;
using BizLayer.Repositories.ToolR.ToolCatalogR;
using DatabaseLayer.DatabaseChange;
using DatabaseLayer.Helper;
using DatabaseLayer.IDbRequests;
using DB;
using DB.Model.ToolInfo;
using Microsoft.EntityFrameworkCore;
using ServiceLayer.Tools.Services.Interfaces;
using Shared.BasicStructuresExtensions;
using Shared.Dto.Tools;
using Shared.Enums;

namespace ServiceLayer.Tools.Services;

public class ToolCatalogsService : ErrorsMapper, IToolCatalogsService
{
    private readonly DbApplicationContext _context;
    private readonly IMapper _dataMapper;
    private readonly BaseModelRequests<ToolCatalog> _repository;

    public ToolCatalogsService(DbApplicationContext context, IMapper dataMapper)
    {
        _context = context;
        _dataMapper = dataMapper;
        _repository = new BaseModelRequests<ToolCatalog>(context, dataMapper);
    }
    public async Task ChangeCatalogAsync(ChangeToolCatalogDto dto)
    {
        var toolCatalog = await _repository.FindByIdAsync(dto.Id);
        if (toolCatalog is null)
        {
            AddErrors("Не удалось найти каталог");
            return;
        }

        if (toolCatalog.Title != dto.Title && dto.Title is not null)
            toolCatalog.Title = dto.Title;
        if (toolCatalog.FatherId != dto.FatherId)
            toolCatalog.FatherId = dto.FatherId;

        await _context.SaveChangesWithValidationsAsync(this);
            

    }

    public async Task<int?> AddCatalogAsync(AddToolCatalogDto dto)
    {
        var toolCatalog = await _context.AddWithValidationsAndSaveAsync(new ToolCatalog
        {
            Title = dto.Title,
            FatherId = dto.FatherId > 0 ? dto.FatherId : null
        }, this);

        return toolCatalog?.Id;
    }

    public async Task ChangeChildCatalogAsync(ChangeToolAndCatalogChild dto)
    {
        var toolRepository = new BaseModelRequests<Tool>(_context, _dataMapper);

        var catalogs = await _repository.GetAllAsync(
            filter: tc => dto.Catalogs.Contains(tc.Id),
            trackingOptions: TrackingOptions.WithTracking);
        
        var tools = await toolRepository.GetAllAsync(
            filter: t => dto.Tools.Contains(t.Id),
            include: i => i.Include(t => t.ToolCatalogsConcretes!),
            trackingOptions: TrackingOptions.WithTracking);

        ToolCatalogValidations.ChangeFatherCatalogValidations(tools, catalogs, dto, this);
        if (HasErrors) return;

        catalogs!.ForEach(c => c.FatherId = dto.FatherId);
        tools!.ForEach(t => t.ToolCatalogsConcretes = new()
        {
            new()
            {
                ToolCatalogId = dto.FatherId
            }
        });
        await _context.SaveChangesWithValidationsAsync(this);
    }

    public async Task DeleteCatalogAsync(int id)
    {
        _context.Remove(new ToolCatalog { Id = id });
        await _context.SaveChangesWithValidationsAsync(this, false);
    }
    
    public async Task<OpenCatalogDto> AddKeyAsync(GetAllOpenCatalog dto)
    {
        var toolsService = new BaseModelRequests<Tool>(_context, _dataMapper);
        OpenCatalogDto openCatalog = new()
        {
            ToolCatalogs = await _repository.GetAllAsync<GetToolCatalogDto>(filter: tc => tc.FatherId == dto.FatherId),
            Tools = await toolsService.GetAllAsync<GetToolDto>(filter: t => t.ToolCatalogsConcretes!.Any(cc => dto.FatherId != 0 && cc.ToolCatalogId == dto.FatherId))
        };
        int k = 1;
        string v = "";
        if (dto.Key != "0")
            v = dto.Key;
        openCatalog.ToolCatalogs?.ForEach(tc => tc.Key = (v + k++.ToString()));
        openCatalog.Tools?.ForEach(t => t.Key = (v + k++.ToString()));
        
        foreach(var item in openCatalog.ToolCatalogs!)
        {
            var T = await toolsService.GetAllAsync<GetToolDto>(filter: t => t.ToolCatalogsConcretes!.Any(cc => cc.ToolCatalogId == item.Id));
            var TC = await _repository.GetAllAsync<GetToolCatalogDto>(filter: tc => tc.FatherId == item.Id);
            if(TC.Count != 0 || T.Count != 0 )
                item.HasChild = true;
        }
        
        return openCatalog;
    }

    public void Dispose() => _context.Dispose();

    public async Task<List<GetToolCatalogDto>?> GetLevelAsync([DefaultValue(null)] int? fatherId)
    {
        var toolCatalogs = await _repository.GetAllAsync<GetToolCatalogDto>(filter: tc => tc.FatherId == fatherId,
            include: i => i.Include(tc => tc.ChildrenToolCatalogs!));
        foreach(var item in toolCatalogs!)
        {
            var t = await _repository.GetAllAsync<GetToolCatalogDto>(filter: tc => tc.FatherId == item.Id,
            include: i => i.Include(tc => tc.ChildrenToolCatalogs!));
            if(t?.Count != 0)
                item.HasChild = true;
        }
        if(toolCatalogs is null)
            AddErrors("Не удалось найти подкаталоги");
        return toolCatalogs;
    }

    
}