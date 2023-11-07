namespace Postex.Parcel.Application.Configuration;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationCore(this IServiceCollection services, IConfiguration configuration)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());
        services.AddAutoMapper(assembly);
        services.AddValidatorsFromAssembly(assembly);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehaviours<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionPipelineBehaviours<,>));

        return services;
    }
}
