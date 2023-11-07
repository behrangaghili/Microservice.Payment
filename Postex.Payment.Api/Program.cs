namespace Postex.Payment.Api;

public class Program
{
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File("logs/log-.log", rollingInterval: RollingInterval.Hour)
            .CreateLogger();

        try
        {
            Log.Information("Application starting...", Program.AppName);
            var host = CreateHostBuilder(args).Build();
            Log.Information("Applying migrations ({ApplicationContext})...", Program.AppName);
            // Apply Migrations
            host.MigrateDataBase<ApplicationDBContext>((services) =>
            {
                var logger = services.GetService<ILogger<ApplicationDBContextSeed>>();
                var context = services.GetRequiredService<ApplicationDBContext>();
                new ApplicationDBContextSeed().MaigrateAndSeedAsync(context, logger).Wait();
            });

            Log.Information("Starting web host ({ApplicationContext})...", Program.AppName);
            host.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "one or more error occurred, see details to solve it");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
    public static string? Namespace = typeof(Startup).Namespace;
    public static string? AppName = "Payment.API";
    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            })
            .UseSerilog();
}

