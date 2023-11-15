namespace Plan7.ApplicationConfigurations;

/// <summary>
/// 
/// </summary>
public static class CorsPolicies
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddCorsPolicies(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.SetIsOriginAllowed(_ => true)
                .AllowAnyMethod()
                .AllowCredentials()
                .AllowAnyHeader();
            });
        });

        services.AddCors(options =>
        {
            options.AddPolicy(name: configuration.GetValue<string>("CORSPolicyNames:React")!,
                policy =>
                {
                    policy
                        .WithOrigins("http://localhost:3000",
                                     "http://localhost:5173",
                                     "http://10.10.5.154:3000",
                                     "http://10.10.5.154")   
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .AllowAnyMethod();
                });
        });

        services.AddCors(options =>
        {
            options.AddPolicy(name: configuration.GetValue<string>("CORSPolicyNames:TelegramBot")!,
                policy =>
                {
                    policy
                        .WithOrigins("http://localhost:5050",
                                     "https://localhost:5051")  
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .AllowAnyMethod();
                });
        });

        return services;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="app"></param>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static WebApplication AddCorses(this WebApplication app, WebApplicationBuilder builder)
    {
        app.UseCors(builder.Configuration.GetValue<string>("CORSPolicyNames:React")!);
        app.UseCors(builder.Configuration.GetValue<string>("CORSPolicyNames:TelegramBot")!);
        app.UseCors();

        return app;
    }
}
