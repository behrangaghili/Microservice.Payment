using Postex.Payment.Application.Contracts;
using Postex.Payment.Application;

namespace Postex.Payment.Api;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddCors();
        services.AddControllers()
            .AddJsonOptions(config =>
            {
                config.JsonSerializerOptions.WriteIndented = true;
                config.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            });

        services.AddControllersWithViews();
        services.AddRazorPages();
        services.AddMvc().AddRazorRuntimeCompilation();

        services.AddPersistance(Configuration);
        services.AddPaymentResolver(Configuration);
        services.AddScoped<IWalletServiceClient, WalletServiceClient>();

        services.AddCustomVersioningSwagger();
        services.AddApplicationCore(Configuration);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseCustomExceptionHandler();
        app.UseCorrelationIdMiddleware();
        app.UseCustomSwagger();

        app.UseCors(x => x
          .AllowAnyMethod()
          .AllowAnyHeader()
          .AllowAnyOrigin());

        app.UseRouting();
        app.UseStatusCodePages();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapDefaultControllerRoute();
            endpoints.MapRazorPages();
        });
    }
}
