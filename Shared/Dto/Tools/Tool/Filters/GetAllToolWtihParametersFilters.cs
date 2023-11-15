using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Shared.Enums;

namespace Shared.Dto.Tools.Tool.Filters;

public class GetAllToolWtihParametersFilters
{
    [Range(1,int.MaxValue)]
    public int ParameterId { get; set; }

    [DefaultValue(ToolParameterConditionalStatements.Equal)]
    [Range((int)ToolParameterConditionalStatements.Equal,(int)ToolParameterConditionalStatements.GreaterOrEqual)]
    public ToolParameterConditionalStatements ConditionalStatement { get; set; }

    public string Value { get; set; }
}