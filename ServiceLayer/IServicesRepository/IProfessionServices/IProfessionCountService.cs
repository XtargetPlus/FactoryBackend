namespace ServiceLayer.IServicesRepository.IProfessionServices;

public interface IProfessionCountService : IBaseCountService
{
    Task<int?> GetAllAsync(string text = "");
}
