using Shared.Dto.Detail.DetailTree;
using Shared.Dto.Detail;
using Shared.Dto.Detail.DetailChild;
using DatabaseLayer.Helper;
using DatabaseLayer.IDbRequests.DetailToDb;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace BizLayer.Repositories.DetailR;

public class DetailRepository
{
    private readonly ErrorsMapper _mapper;
    private readonly IMapper _dataMapper;

    public DetailRepository(ErrorsMapper mapper, IMapper dataMapper) => (_mapper, _dataMapper) = (mapper, dataMapper);

    /// <summary>
    /// Получаем весь состав изделия без повторяющихся деталей
    /// </summary>
    /// <param name="detailId">Id изделия</param>
    /// <returns></returns>
    public async Task<List<BaseIdSerialTitleDto>?> GetAllProductDetailsAsync(GetAllProductDetailsDto dto, DbContext context)
    {
        List<BaseIdSerialTitleDto> details = new();
        // Получаем базовую информацию об изделии
        BaseSerialTitleDto? baseDetail = await new DetailRequests(context, _dataMapper).FindBaseInfoAsync(dto.DetailId);
        if (baseDetail is null)
        {
            _mapper.AddErrors("Не удалось обнаружить информацию об изделии");
            return null;
        }
        // создаем корневой узел
        DetailTree tree = new(dto.DetailId, await GetChildForTree(dto.DetailId, context))
        {
            SerialNumber = baseDetail.SerialNumber,
            Title = baseDetail.Title
        };
        // Переходим в первый внутренний узел, начинаем перебор дерева
        tree.GetNext();
        if (tree.Next is null)
            return details;
        while (tree.Next is not null || tree.Back is not null)
        {
            // если дошли до терминального узла
            if (tree.Next is null && tree.Back is not null)
            {
                tree = tree.Back;
                tree.GetNext();
            }
            // если есть внутренний узел
            if (tree.Next is not null)
            {
                // переходим во внутренний узел
                tree = new(
                    tree,
                    tree.Next,
                    await GetChildForTree(tree.Next.Id, context));
                tree.GetNext();
            }
            // добавляем деталь в список, если его еще нет
            if (details.FirstOrDefault(d => d.DetailId == tree.Id) is null && tree.Id != dto.DetailId)
            {
                switch (dto.IsHardDetail)
                {
                    case true when await DetailSimpleRead.IsHardDetailAsync(context, tree.Id):
                    case false:
                        details.Add(new BaseIdSerialTitleDto { DetailId = tree.Id, Title = tree.Title, SerialNumber = tree.SerialNumber });
                        break;
                }
            }
            // если нет внутренних узлов
            if (tree.Items.Count == 0)
            {
                tree = tree.Back;
                tree.GetNext();
            }
        }
        return details;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="detailId"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task<List<GetProductDetailsWithUsabilityDto>> GetProductDetailsWithRepeatsAndUsabilityAsync(int detailId, DbContext context)
    {
        var details = new List<GetProductDetailsWithUsabilityDto>();
        var tree = new DetailTree(detailId, await GetChildForTree(detailId, context, true));

        tree.GetNext();
        if (tree.Next is null)
            return details;
        
        while (tree.Next is not null || tree.Back is not null)
        {
            if (tree.Next is null && tree.Back is not null)
            {
                tree = tree.Back;
                tree.GetNext();
            }

            if (tree.Next is null) continue;
            
            tree.GeneratePositionNumbers();

            // переходим во внутренний узел
            tree = new(
                tree,
                tree.Next,
                await GetChildForTree(tree.Next.Id, context, true),
                tree.Next.Count);
            
            tree.GetNext();

            details.Add(new()
            {
                DetailId = tree.Id,
                PositionNumber = tree.PositionNumber,
                Usability = tree.Count
            });
        }

        return details;
    }

    /// <summary>
    /// Получаем список уникальных изделий, в которых применяется данная деталь с общим количеством деталей в этом изделии 
    /// </summary>
    /// <param name="dto">Информация о детали, чей список изделий мы хотим получить</param>
    /// <returns></returns>
    public async Task<List<DetailProductsDto>?> GetAllProductsAsync(int detailId, DbContext context)
    {
        List<DetailProductsDto> products = new();
        // Ищем информацию о детали
        var baseDetail = await new DetailRequests(context, _dataMapper).FindBaseInfoAsync(detailId);
        if (baseDetail is null)
        {
            _mapper.AddErrors("Не удалось обнаружить информацию о детали");
            return null;
        }
        // Создаем корневой узел
        DetailTree tree = new(detailId, await GetAllFathersAsync(detailId, context) ?? new())
        {
            SerialNumber = baseDetail.SerialNumber,
            Title = baseDetail.Title,
        };
        // Переходим в первый внутренний узел, начинаем перебор дерева
        tree.GetNext();
        if (tree.Next is null)
            return products;
        while (tree.Next is not null || tree.Back is not null)
        {
            // если дошли до терминального узла
            if (tree.Next is null && tree.Back is not null)
            {
                tree = tree.Back;
                tree.GetNext();
            }
            // если есть внутренний узел
            if (tree.Next is not null)
            {
                var count = tree.GetMultiplyCount(tree.Next?.Count ?? 1);

                // переходим во внутренний узел
                tree = new(
                    tree,
                    tree.Next,
                    await GetAllFathersAsync(tree.Next.Id, context) ?? new(),
                    count
                );

                tree.GetNext();
            }
            // если нет внутренних узлов
            if (tree.Items.Count == 0)
            {
                if (products.Where(d => d.SerialNumber == tree.SerialNumber)?.FirstOrDefault() is null)
                    products.Add(new DetailProductsDto { Title = tree.Title, SerialNumber = tree.SerialNumber, Count = tree.Count, Unit = baseDetail.Unit });
                else
                    products[products.FindIndex(p => p.SerialNumber == tree.SerialNumber)].Count += tree.Count;
                tree = tree.Back;
                tree.GetNext();
            }
        }
        return products;
    }

    /// <summary>
    /// Получаем список деталей, в чью составы входит деталь childId и суммируем к каждому его полю "Count" количество из предыдущей детали
    /// </summary>
    /// <param name="childId">Id детали, чей верхний уровень применяемости</param>
    /// <returns>Список, куда входит деталь childId</returns>
    private async Task<List<DetailTree>?> GetAllFathersAsync(int childId, DbContext context) => await new DetailChildRequests(context, _dataMapper).GetAllFatherAsync(childId, 1);

    /// <summary>
    /// Получаем состав детали productId уровня ниже
    /// </summary>
    /// <param name="productId">Id детали, чей состав мы хотим получить</param>
    /// <returns>Список состава</returns>
    private async Task<List<DetailTree>> GetChildForTree(int productId, DbContext context, bool addCount = false)
    {
        var detailChildren = await new DetailChildRequests(context, _dataMapper).GetAllChildrenAsync(new() { FatherId = productId }) ?? new();

        detailChildren = detailChildren.OrderBy(dc => dc.Number).ToList();

        return detailChildren.Select(detail => new DetailTree(detail.Id) { SerialNumber = detail.SerialNumber, Title = detail.Title, Count = addCount ? detail.Count : 0, }).ToList();
    }
}