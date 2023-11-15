using Plan7.Hubs.AdminHubs;
using Plan7.Hubs.DetailHubs;
using Plan7.Hubs;

namespace Plan7.ApplicationConfigurations;

/// <summary>
/// 
/// </summary>
public static class HubsBuilder
{
    /// <summary>
    /// Подключение хабов
    /// </summary>
    /// <param name="builder">Конфигурация конечных точек</param>
    /// <returns>Конфигурация конечных точек</returns>
    public static IEndpointRouteBuilder MapMyHubs(this IEndpointRouteBuilder builder)
    {
        builder.MapHub<UserHub>("/user/hub");
        builder.MapHub<SubdivisionHub>("/subdiv/hub");
        builder.MapHub<ProfessionHub>("/prof/hub");
        builder.MapHub<ProductHub>("/product/hub");
        builder.MapHub<OutsideOrganizationHub>("/out-org/hub");
        builder.MapHub<OperationHub>("/operation/hub");
        builder.MapHub<MoveTypeHub>("/move-type/hub");
        builder.MapHub<EquipmentHub>("/equipment/hub");
        builder.MapHub<ClientHub>("/client/hub");
        builder.MapHub<AccessoryTypeHub>("/acessory-type/hub");
        builder.MapHub<MaterialHub>("/material/hub");
        builder.MapHub<DetailHub>("/detail/hub");
        builder.MapHub<UnitHub>("/unit/hub");
        builder.MapHub<BlankTypeHub>("/blank-type/hub");
        builder.MapHub<StatusHub>("/status/hub");

        return builder;
    }
}
