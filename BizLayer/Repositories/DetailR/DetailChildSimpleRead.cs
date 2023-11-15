using DatabaseLayer.Helper;
using DatabaseLayer.IDbRequests;
using DB.Model.DetailInfo;

namespace BizLayer.Repositories.DetailR;

public static class DetailChildSimpleRead
{
    public static async Task<DetailsChild?> GetAsync(BaseModelRequests<DetailsChild> repository, int fatherId, int childId, ErrorsMapper mapper)
    {
        DetailsChild? detail = await repository.FindFirstAsync(filter: dc => dc.FatherId == fatherId && dc.ChildId == childId);
        if (detail is null)
            mapper.AddErrors("Не удалось получить деталь состава");
        return detail;
    }

    public static async Task<DetailsChild?> GetByNumberAsync(BaseModelRequests<DetailsChild> repository, int fatherId, int number, ErrorsMapper mapper)
    {
        DetailsChild? detail = await repository.FindFirstAsync(filter: dc => dc.FatherId == fatherId && dc.Number == number);
        if (detail is null)
            mapper.AddErrors("Не удалось получить деталь состава");
        return detail;
    }
}