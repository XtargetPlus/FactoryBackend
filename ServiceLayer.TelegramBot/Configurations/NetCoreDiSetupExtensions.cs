using Microsoft.Extensions.DependencyInjection;
using NetCore.AutoRegisterDi;

namespace ServiceLayer.TelegramBot.Configurations;

public static class NetCoreDiSetupExtensions
{
    public static void RegisterNonGenericTelegramBotDi(this IServiceCollection services)
    {
        services.RegisterAssemblyPublicNonGenericClasses()
            .Where(c => c.Name.EndsWith("Service"))
            .AsPublicImplementedInterfaces();
    }
}
