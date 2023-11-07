namespace Postex.Payment.Infrastructure.Data;

public class ApplicationDBContext : DbContext
{
    public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfiguration(new PaymentRequestEntityTypeConfiguration());
        builder.ApplyConfiguration(new PaymentRequestRefundEntityTypeConfiguration());
        builder.ApplyConfiguration(new CashoutBatchRequestEntityTypeConfiguration());
        builder.ApplyConfiguration(new CashoutItemRequestEntityTypeConfiguration());
    }
}