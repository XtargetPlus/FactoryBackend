namespace ServiceLayer.IServicesRepository;

public interface IBaseCountService : IDisposable
{
    Task<int?> GetAllAsync();
}
