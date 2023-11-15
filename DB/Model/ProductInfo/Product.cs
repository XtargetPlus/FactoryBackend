using DB.Model.DetailInfo;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DB.Model.ProductInfo;

/// <summary>
/// Продукт (изделие)
/// </summary>
public class Product : BaseModel, IValidatableObject
{
    /// <summary>
    /// Цена
    /// </summary>
    public float Price { get; set; }
    /// <summary>
    /// Деталь
    /// </summary>
    public int DetailId { get; set; }
    public Detail? Detail { get; set; } 
    public List<ClientProduct>? ClientProducts { get; set; }

    /// <summary>
    /// Валидация данных на низком уровне
    /// </summary>
    /// <param name="validationContext">Текущий контекст запроса</param>
    /// <returns>Список ошибок с названием полей, которые не прошли валидацию</returns>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Price <= 0)
            yield return new ValidationResult("Цена имеет недопустимое значение", new[] { nameof(Price) });

        if (validationContext.GetService(typeof(DbContext)) is not DbContext context) yield break;
        
        if (!context.Set<Detail>().Any(d => d.Id == DetailId))
            yield return new ValidationResult("Не удалось найти деталь", new[] { nameof(DetailId) });
        if (context.Set<Product>().Any(p => p.Id != Id && Math.Abs(p.Price - Price) == 0 && p.DetailId == DetailId))
            yield return new ValidationResult("Данное изделие с такой ценой", new[] { nameof(DetailId) });
    }
}