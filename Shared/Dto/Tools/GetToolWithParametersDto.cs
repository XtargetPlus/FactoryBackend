using Shared.Dto.Tools.Tool.Filters;

namespace Shared.Dto.Tools;

public class GetToolWithParametersDto
{
    public List<GetAllToolWtihParametersFilters> ? ParametersFilters { get; set; }

}