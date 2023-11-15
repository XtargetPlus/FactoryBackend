using Microsoft.Extensions.DependencyInjection;
using NetCore.AutoRegisterDi;

namespace ServiceLayer.TechnologicalProcesses.Configurations;

public static class NetCoreDiSetupExtensions
{
    public static void RegisterNonGenericTechProcessDi(this IServiceCollection services)
    {
        services.RegisterAssemblyPublicNonGenericClasses()
            .Where(c => c.Name.EndsWith("Service"))
            .AsPublicImplementedInterfaces();
    }
}
