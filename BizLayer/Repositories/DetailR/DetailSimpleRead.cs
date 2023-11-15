using DatabaseLayer.Helper;
using DatabaseLayer.IDbRequests;
using DB.Model.DetailInfo;
using Microsoft.EntityFrameworkCore;
using Shared.Static;

namespace BizLayer.Repositories.DetailR;

public static class DetailSimpleRead
{
    public static async Task<Detail?> GetAsync(BaseModelRequests<Detail> repository, int detailId, ErrorsMapper mapper)
    {
        var detail = await repository.FindByIdAsync(detailId);

        if (detail is null)
            mapper.AddErrors("Не удалось получить деталь");

        return detail;
    }

    public static async Task<bool> IsHardDetailAsync(DbContext context, int detailId) =>
        await context.Set<Detail>().Where(d => d.Id == detailId).Select(d => d.DetailTypeId).FirstAsync() == (int)DetailTypes.WithProductionPostparation;
}