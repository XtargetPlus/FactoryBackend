using Shared.Static;

using System.Diagnostics.CodeAnalysis;

namespace Shared.Dto.Profession.Filters;

public class GetAllProfessionFilters : BasePagination
{
    /// <summary>
    /// Текст поиска
    /// </summary>
    [MaybeNull]
    public string Text { get; set; }
}
