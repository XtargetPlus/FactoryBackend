using DatabaseLayer.Helper;
using DatabaseLayer.IDbRequests;
using DB.Model.SubdivisionInfo;

namespace BizLayer.Repositories.SubdivisionR;

public static class SubdivisionReadWithValidations
{
    public static async Task<Subdivision?> GetAsync(BaseModelRequests<Subdivision> repository, int subdivisionId, ErrorsMapper mapper)
    {
        var subdivision = await repository.FindByIdAsync(subdivisionId);
        if (subdivision is null)
            mapper.AddErrors("Не удалось получить подразделение");
        return subdivision;
    }
}