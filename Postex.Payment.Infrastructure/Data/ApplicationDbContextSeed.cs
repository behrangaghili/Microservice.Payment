namespace Postex.Payment.Infrastructure.Data;


public class ApplicationDBContextSeed
{

  public async Task MaigrateAndSeedAsync(ApplicationDBContext context, ILogger<ApplicationDBContextSeed> logger)
  {
    

    await context.Database.MigrateAsync();

    try
    {
      // Seed Data 
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "EXCEPTION ERROR while migrating {DbContextName}", nameof(ApplicationDBContext));
    }
  }
}
