using Microsoft.Extensions.DependencyInjection;
using NetCore.AutoRegisterDi;
using ServiceLayer.IServicesRepository;
using ServiceLayer.Services.BaseS;

namespace ServiceLayer.Configurations;

public static class NetCoreDiSetupExtensions
{
    public static void RegisterServiceLayerDi(this IServiceCollection services)
    {
        services.RegisterAssemblyPublicNonGenericClasses()
            .Where(c => c.Name.EndsWith("Service"))
            .AsPublicImplementedInterfaces();

        services.AddTransient(typeof(ISimpleGenericModelService<>), typeof(SimpleGenericModelService<>));
    }
}
