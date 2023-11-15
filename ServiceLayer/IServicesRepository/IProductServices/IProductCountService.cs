using Shared.Enums;

namespace ServiceLayer.IServicesRepository.IProductServices;

public interface IProductCountService : IBaseCountService
{
    Task<int?> GetAllAsync(string text = "", SerialNumberOrTitleFilter searchOptions = SerialNumberOrTitleFilter.ForSerialNumber);
}
