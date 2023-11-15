using Microsoft.Extensions.DependencyInjection;
using NetCore.AutoRegisterDi;

namespace ServiceLayer.Equipments.Configurations;

public static class NetCoreDiSetupExtensions
{
    public static void RegisterNonGenericEquipmentDi(this IServiceCollection services)
    {
        services.RegisterAssemblyPublicNonGenericClasses()
            .Where(c => c.Name.EndsWith("Service"))
            .AsPublicImplementedInterfaces();
    }
}
