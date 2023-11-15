namespace Shared.Enums;

/// <summary>
/// Выбор сортировки
/// </summary>
public enum KindOfOrder
{
	Base,
	Up,
	Down
}

/// <summary>
/// Фильтры для объектов содержащих серийный номер и наименование
/// </summary>
public enum SerialNumberOrTitleFilter
{
	Base,
	ForSerialNumber,
	ForTitle
}

/// <summary>
/// Настройка глобальных фильтров
/// </summary>
public enum QueryFilterOptions
{
	TurnOff,
	TurnOn
}

/// <summary>
/// В каком виде получать данные (отслеживать или нет)
/// </summary>
public enum TrackingOptions
{
	WithTracking,
	AsNoTracking,
	AsNoTrackingWithIdentityResolution
}

public enum DeleteOrHide
{
	Delete,
	Hide
}
