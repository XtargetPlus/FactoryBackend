using Shared.Enums;

namespace ServiceLayer.Equipments.Services.Interfaces;

public interface IEquipmentCountService : IDisposable
{
    Task<int?> GetAllAsync();
    Task<int?> GetAllAsync(int subdivId = 0, string text = "", SerialNumberOrTitleFilter searchOptions = SerialNumberOrTitleFilter.ForSerialNumber);
}
