using DatabaseLayer.IDbRequests;
using Microsoft.Extensions.DependencyInjection;

namespace DatabaseLayer.Configurations;

public static class NetCoreDiSetupExtensions
{
    public static void RegisterDatabaseLayerDi(this IServiceCollection services)
    {
        services.AddTransient(typeof(ICountInformationDb<>), typeof(CountToMainForm<>));
        services.AddTransient(typeof(IGenericRequest<>), typeof(BaseModelRequests<>));
    }
}