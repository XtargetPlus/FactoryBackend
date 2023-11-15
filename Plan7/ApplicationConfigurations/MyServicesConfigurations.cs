using DatabaseLayer.Configurations;
using ServiceLayer.Configurations;
using ServiceLayer.Equipments.Configurations;
using ServiceLayer.Details.Configurations;
using ServiceLayer.Graphs.Configurations;
using ServiceLayer.TechnologicalProcesses.Configurations;
using ServiceLayer.TelegramBot.Configurations;
using ServiceLayer.Tools.Configurations;

namespace Plan7.ApplicationConfigurations;

/// <summary>
/// 
/// </summary>
public static class MyServicesConfigurations
{
    /// <summary>
    /// Подключение сервисов
    /// </summary>
    /// <param name="services">Конфигурация подключенных сервисов приложения</param>
    /// <returns>Конфигурация подключенных сервисов приложения</returns>
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.RegisterServiceLayerDi();
        services.RegisterDatabaseLayerDi();
        services.RegisterNonGenericEquipmentDi();
        services.RegisterNonGenericDetailDi();
        services.RegisterNonGenericTechProcessDi();
        services.RegisterNonGenericTelegramBotDi();
        services.RegisterNonGenericGraphDi();
        services.RegisterNonGenericToolDi();

        return services;
    }
}
