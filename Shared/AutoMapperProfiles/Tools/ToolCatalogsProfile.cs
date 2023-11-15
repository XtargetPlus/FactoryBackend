using AutoMapper;
using DB.Model.ToolInfo;
using Shared.Dto.Tools;

namespace Shared.AutoMapperProfiles.Tools;

public class ToolCatalogsProfile : Profile
{
    public ToolCatalogsProfile()
    {
        CreateMap<ToolCatalog, GetToolCatalogDto>();
    }
}