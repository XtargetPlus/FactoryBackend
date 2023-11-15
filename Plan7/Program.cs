using Microsoft.EntityFrameworkCore;
using Plan7.Helper;
using Plan7.ApplicationConfigurations;
using DB;
using System.Reflection;
using Serilog;
using Microsoft.AspNetCore.CookiePolicy;

var builder = WebApplication.CreateBuilder(args);

ElsConfigure.ConfigureLogging();
builder.Host.UseSerilog();

builder.Services.AddDbContext<DbApplicationContext>(options => 
        options.UseMySql(
            builder.Configuration.GetConnectionString("DefaultConnection"), 
            new MySqlServerVersion(new Version(8, 0)), 
            b => b.MigrationsAssembly("DB"))
        );

builder.Services.AddAutoMapper(Assembly.Load("Shared"));
builder.Services.AddSignalR();
builder.Services.AddRepositories();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Shared.xml"));
});

builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddAuthorization();
builder.Services.AddCorsPolicies(builder.Configuration);

var app = builder.Build();

app.AddCorses(builder);

app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict, 
    HttpOnly = HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.Always
});

if (app.Environment.IsDevelopment())
{
    app.UseMiddleware<AddJwtTokenMiddleware>();
    app.UseHttpsRedirection();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapMyHubs();

app.Run();