using System.Collections;
using Microsoft.EntityFrameworkCore;
using DB.db.Options;
using DatabaseLayer.Helper;

namespace DatabaseLayer.DatabaseChange;

public static class ExtensionsForChange
{
    public static async Task<int> SaveChangesWithValidationsAsync(this DbContext context, ErrorsMapper mapper, bool validationBeforeSaving = true)
    {
        try
        {
            if (validationBeforeSaving)
            {
                mapper.AddWarnings(context.ExecuteValidation());
                if (mapper.HasWarnings)
                    return 0;
            }

            var changesCount = await context.SaveChangesAsync();
            if (changesCount < 1)
                mapper.AddErrors("Не удалось сохранить изменения");
            return changesCount;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            Console.WriteLine($"Ошибка обновления (произошло параллельное обновление): {ex.Message}");
            mapper.AddErrors("Не удалось сохранить изменения, конфликт версий");
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine($"Ошибка обновления: {ex.Message}");
            mapper.AddErrors("Не удалось сохранить изменения, ошибка обновления");
        }
        catch (OperationCanceledException ex)
        {
            Console.WriteLine($"Отмена операции: {ex.Message}");
            mapper.AddErrors("Операция была отменена");
        }
        return 0;
    }

    public static async Task<TEntity?> AddWithValidationsAndSaveAsync<TEntity>(this DbContext context, TEntity entity, ErrorsMapper mapper)
        where TEntity : class
    {
        try
        {
            if (entity is IEnumerable) await context.AddRangeAsync(entity);
            else await context.AddAsync(entity);

            mapper.AddWarnings(context.ExecuteValidation());
            if (mapper.HasWarnings)
                return null;

            var changesCount = await context.SaveChangesAsync();
            if (changesCount < 1)
                mapper.AddErrors("Не удалось сохранить изменения");
            return entity;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            Console.WriteLine($"Ошибка сохранения (произошло параллельное обновление): {ex.Message}");
            mapper.AddErrors("Не удалось сохранить изменения, конфликт версий");
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine($"Ошибка сохранения: {ex.Message}");
            mapper.AddErrors("Не удалось сохранить изменения, ошибка обновления");
        }
        catch (OperationCanceledException ex)
        {
            Console.WriteLine($"Отмена операции: {ex.Message}");
            mapper.AddErrors("Операция была отменена");
        }
        return null;
    }

    public static async Task RemoveWithValidationAndSaveAsync<TEntity>(this DbContext context, TEntity? entity, ErrorsMapper mapper) 
        where TEntity : class
    {
        try
        {
            if (entity is null)
            {
                mapper.AddErrors("Попытка удаления пустой записи");
                return;
            }

            context.Remove(entity);

            var changesCount = await context.SaveChangesAsync();
            if (changesCount < 1)
                mapper.AddErrors("Не удалось сохранить изменения");
        }
        catch (DbUpdateConcurrencyException ex)
        {
            Console.WriteLine($"Ошибка сохранения (произошло параллельное обновление): {ex.Message}");
            mapper.AddErrors("Не удалось сохранить изменения, конфликт версий");
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine($"Ошибка сохранения: {ex.Message}");
            mapper.AddErrors("Не удалось сохранить изменения, ошибка обновления");
        }
        catch (OperationCanceledException ex)
        {
            Console.WriteLine($"Отмена операции: {ex.Message}");
            mapper.AddErrors("Операция была отменена");
        }
    }

    public static async Task RemoveRangeWithValidationAndSaveAsync<TEntity>(this DbContext context, List<TEntity>? entity, ErrorsMapper mapper)
        where TEntity : class
    {
        try
        {
            if (entity is null)
            {
                mapper.AddErrors("Попытка удаления пустой записи");
                return;
            }

            context.RemoveRange(entity);

            var changesCount = await context.SaveChangesAsync();
            if (changesCount < 1)
                mapper.AddErrors("Не удалось сохранить изменения");
        }
        catch (DbUpdateConcurrencyException ex)
        {
            Console.WriteLine($"Ошибка сохранения (произошло параллельное обновление): {ex.Message}");
            mapper.AddErrors("Не удалось сохранить изменения, конфликт версий");
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine($"Ошибка сохранения: {ex.Message}");
            mapper.AddErrors("Не удалось сохранить изменения, ошибка обновления");
        }
        catch (OperationCanceledException ex)
        {
            Console.WriteLine($"Отмена операции: {ex.Message}");
            mapper.AddErrors("Операция была отменена");
        }
    }
}