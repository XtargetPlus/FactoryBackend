namespace Plan7.Helper.Hubs;

/// <summary>
/// Информация изменяемых записей
/// </summary>
public class EditMapping
{
    /// <summary>
    /// Кто и какую запись редактирует
    /// </summary>
    private readonly Dictionary<string, int> _editing = new();

    /// <summary>
    /// Количество редактируемых записей
    /// </summary>
    public int Count { get {  return _editing.Count; } }

    /// <summary>
    /// Добавление записи в редактируемые
    /// </summary>
    /// <param name="key">Id подключенного к хабу пользователя</param>
    /// <param name="valueId">Id записи, которая редактируется</param>
    public void Add(string key, int valueId)
    {
        lock (_editing)
        {
            _editing[key] = valueId;
        }
    }

    /// <summary>
    /// Получаем Id записи, которая редактируется
    /// </summary>
    /// <param name="key">Id пользователя, который редактирует записи</param>
    /// <returns>Id редактируемой записи или 0, если пользователь не редактирует запись</returns>
    public int GetEdit(string key)
    {
        if (_editing.TryGetValue(key, out int edit))
        {
            return edit;
        }
        return 0;
    }

    /// <summary>
    /// Проверка, редактируется ли кем то запись
    /// </summary>
    /// <param name="id">Id записи</param>
    /// <returns>true или false</returns>
    public bool CheckEdit(int id)
    {
        foreach (var edit in _editing)
        {
            if (edit.Value == id)
                return true;
        }
        return false;
    }

    /// <summary>
    /// Получаем пользователя, который редактирует запись
    /// </summary>
    /// <param name="id">Id записи, которая редактируется</param>
    /// <returns>string.Empty или пользователь</returns>
    public string GetEditor(int id)
    {
        foreach (var edit in _editing)
        {
            if (edit.Value == id)
                return edit.Key;
        }
        return string.Empty;
    }

    /// <summary>
    /// Список редактируемых записей
    /// </summary>
    /// <returns>Список Id редактируемых записей</returns>
    public List<int> GetEdits()
    {
        List<int> edits = new();
        foreach (var key in _editing.Keys)
        {
            edits.Add(_editing[key]);
        }
        return edits;
    }

    /// <summary>
    /// Удаляем запись из редактирования
    /// </summary>
    /// <param name="key">Id пользователя, который редактирует запись</param>
    public void Remove(string key)
    {
        lock (_editing)
        {
            if (!_editing.TryGetValue(key, out int edit))
            {
                return;
            }
            _editing.Remove(key);
        }
    }
}
