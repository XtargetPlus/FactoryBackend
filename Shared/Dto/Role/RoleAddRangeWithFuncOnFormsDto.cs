namespace Shared.Dto.Role;

public class RoleAddRangeWithFuncOnFormsDto
{
    /// <summary>
    /// Id роли
    /// </summary>
    public int RoleId { get; set; }
    /// <summary>
    /// Функционал на форме, где key - formId, value - функционал
    /// </summary>
    public Dictionary<int, RoleClientFuncDto> FuncInForms { get; set; } = default!; 
}
