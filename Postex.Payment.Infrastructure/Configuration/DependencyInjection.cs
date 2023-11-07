using Postex.Payment.Application;
using Postex.Payment.Application.Contracts;
using Postex.Payment.Infrastructure.PaymentMethods.Jibit;
using Postex.Payment.Infrastructure.PaymentMethods.Saman;

namespace Postex.Payment.Infrastructure.Configuration;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistance(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDBContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("Persistence"));
            options.EnableSensitiveDataLogging();
        });

        services.AddRepositories(configuration);
        return services;
    }

    public static IServiceCollection AddPaymentResolver(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<IPaymentMethod, MalletPaymentMethod>();
        services.AddScoped<IPaymentMethod, JibitPaymentMethod>();
        services.AddScoped<IPaymentMethod, SamanPaymentMethod>();
        services.AddTransient<IPaymentMethodResolver>(provider => new PaymentMethodResolver(
                provider.GetServices<IPaymentMethod>(),
                provider.GetRequiredService<ILogger<PaymentMethodResolver>>()
            ));
        return services;
    }

    public static IApplicationBuilder InitializeDatabase(this IApplicationBuilder app)
    {
        using (var scope = app.ApplicationServices.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();
        }

        return app;
    }

    private static void AddRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped(typeof(IWriteRepository<>), typeof(EFRepository<>));
        services.AddScoped(typeof(IReadRepository<>), typeof(EFRepository<>));
    }
}
