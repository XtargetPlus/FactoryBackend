using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;

namespace DatabaseLayer.Helper;

/// <summary>
/// Интерфейс для обработки ошибок
/// </summary>
public interface IErrorsMapper
{
    /// <summary>
    /// Неизменяемый список ошибок
    /// </summary>
    IImmutableList<ValidationResult> Errors { get; }
    /// <summary>
    /// Неизменяемый словарь предупреждений
    /// </summary>
    IImmutableDictionary<string, string> Warnings { get; }
    /// <summary>
    /// Есть ли ошибки
    /// </summary>
    bool HasErrors { get; }
    /// <summary>
    /// Есть ли предупреждения
    /// </summary>
    bool HasWarnings { get; }
    /// <summary>
    /// Есть ли предупреждения или ошибки
    /// </summary>
    bool HasWarningsOrErrors { get => HasErrors || HasWarnings; }
}