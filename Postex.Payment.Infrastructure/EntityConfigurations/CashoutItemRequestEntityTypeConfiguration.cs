namespace Postex.Payment.Infrastructure.EntityConfigurations;

class CashoutItemRequestEntityTypeConfiguration : IEntityTypeConfiguration<CashoutItemRequest>
{
    public void Configure(EntityTypeBuilder<CashoutItemRequest> builder)
    {
        builder.ToTable("CashoutItemRequest", "Payment");
        builder.Property(p => p.Amount).IsRequired();
        builder.Property(p => p.IBanNumber).IsRequired();
        builder.Property(p => p.CustomerId).IsRequired();
        builder.Property(p => p.CreatedBy).IsRequired();
        builder.Property(p => p.RowVersion).IsRowVersion();
    }
}
