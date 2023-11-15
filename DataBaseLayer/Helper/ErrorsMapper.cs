using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;

namespace DatabaseLayer.Helper;

/// <summary>
/// Хранение ошибок и предупреждение
/// </summary>
public class ErrorsMapper
{
    /// <summary>
    /// Список ошибок
    /// </summary>
    private readonly List<ValidationResult> _errors = new();
    /// <summary>
    /// Словарь предупреждений
    /// </summary>
    private readonly Dictionary<string, string> _warnings = new();
    /// <summary>
    /// Неизменяемый список ошибок
    /// </summary>
    public IImmutableList<ValidationResult> Errors => _errors.ToImmutableList();
    /// <summary>
    /// Неизменяемый словарь предупреждений: Key - поле в котором вызвано предупреждение, Value - текст предупреждения
    /// </summary>
    public IImmutableDictionary<string, string> Warnings => _warnings.ToImmutableDictionary();
    /// <summary>
    /// Есть ли ошибки
    /// </summary>
    public bool HasErrors => _errors.Any();
    /// <summary>
    /// Есть ли предупреждения
    /// </summary>
    public bool HasWarnings => _warnings.Any();

    /// <summary>
    /// Добавление ошибки
    /// </summary>
    /// <param name="errorMessage">Текст ошибки</param>
    /// <param name="propertyNames">Поля вызывающие ошибку (не обязательный параметр)</param>
    public void AddErrors(string errorMessage, params string[] propertyNames) => _errors.Add(new ValidationResult(errorMessage, propertyNames));

    /// <summary>
    /// Добавление предупреждения
    /// </summary>
    /// <param name="warnings">Неизменяемый список предупрежденияЫ</param> 
    public void AddWarnings(IImmutableList<ValidationResult>? warnings)
    {
        if (warnings == null) return;
            
        foreach (var warning in warnings)
        {
            if (warning.ErrorMessage == null)
                continue;
            foreach (var memberName in warning.MemberNames)
            {
                if (_warnings.ContainsKey(memberName))
                {
                    _warnings[memberName] += "\n" + warning.ErrorMessage;
                    continue;
                }
                _warnings.Add(memberName, warning.ErrorMessage);
            }
        }
    }

    public void AddWarnings(IImmutableDictionary<string, string> warnings)
    {
        foreach (var warning in warnings)
            _warnings.Add(warning.Key, warning.Value);
    }
}