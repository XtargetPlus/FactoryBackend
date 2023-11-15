using Shared.Enums;

namespace ServiceLayer.Equipments.Services.Interfaces;

public interface IEquipmentDetailCountService : IDisposable
{
    Task<int?> GetAllAsync();
    Task<int?> GetAllAsync(string text = "", SerialNumberOrTitleFilter searchOptions = SerialNumberOrTitleFilter.ForSerialNumber);
}
