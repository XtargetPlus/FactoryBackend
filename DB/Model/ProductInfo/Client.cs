using System.ComponentModel.DataAnnotations;

namespace DB.Model.ProductInfo;

/// <summary>
/// Заказчик
/// </summary>
public class Client : BaseTitleModel, IValidatableObject
{
    public List<ClientProduct>? ClientProducts { get; set; }

    /// <summary>
    /// Валидация данных на низком уровне
    /// </summary>
    /// <param name="validationContext">Текущий контекст запроса</param>
    /// <returns>Список ошибок с названием полей, которые не прошли валидацию</returns>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Title.Length < 1)
            yield return new ValidationResult("Длина наименования минимум 1 символ", new[] { nameof(Title) });
        if (Title.Length > 255)
            yield return new ValidationResult("Длина наименования максимум 255 символ", new[] { nameof(Title) });
    }
}