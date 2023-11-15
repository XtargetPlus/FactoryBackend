using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DB.Model.ProductInfo;

/// <summary>
/// Количество заказанного продукта
/// </summary>
public class ClientProduct : IValidatableObject
{
    /// <summary>
    /// Количество заказанных изделий
    /// </summary>
    public int Count { get; set; }
    /// <summary>
    /// Изделие
    /// </summary>
    public int ProductId { get; set; }
    public Product? Product { get; set; }
    /// <summary>
    /// Клиент
    /// </summary>
    public int ClientId { get; set; }
    public Client? Client { get; set; }

    /// <summary>
    /// Валидация данных на низком уровне
    /// </summary>
    /// <param name="validationContext">Текущий контекст запроса</param>
    /// <returns>Список ошибок с названием полей, которые не прошли валидацию</returns>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Count < 1 || Count > int.MaxValue)
            yield return new ValidationResult("Количество имеет недопустимое значение", new[] { nameof(Count) });

        if (validationContext.GetService(typeof(DbContext)) is not DbContext context) yield break;
        
        if (!context.Set<Product>().Any(p => p.Id == ProductId))
            yield return new ValidationResult("Не удалось найти изделие", new[] { nameof(ProductId) });
        if (!context.Set<Client>().Any(c => c.Id == ClientId))
            yield return new ValidationResult("Не удалось найти клиента", new[] { nameof(ClientId) });
    }
}
