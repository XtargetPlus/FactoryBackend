using DatabaseLayer.Helper;
using DB.Model.ToolInfo;
using Shared.BasicStructuresExtensions;
using Shared.Dto.Tools;

namespace BizLayer.Repositories.ToolR.ToolCatalogR;

public static class ToolCatalogValidations
{
    public static void ChangeFatherCatalogValidations(List<Tool>? tools, List<ToolCatalog>? catalogs, ChangeToolAndCatalogChild dto, ErrorsMapper errors)
    {
        if (tools.IsNullOrEmpty() && catalogs.IsNullOrEmpty())
            errors.AddErrors("Не найдены инструменты и каталоги");

        if (tools?.Count != dto.Tools.Count)
            errors.AddErrors("Количество полученных инструментов не равно количеству переданных");

        if (catalogs?.Count != dto.Catalogs.Count)
            errors.AddErrors("Количество полученных каталогов не равно количеству переданных");
    }
}
