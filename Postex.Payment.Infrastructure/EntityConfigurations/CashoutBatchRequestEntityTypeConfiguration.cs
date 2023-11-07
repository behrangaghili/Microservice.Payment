namespace Postex.Payment.Infrastructure.EntityConfigurations;

class CashoutBatchRequestEntityTypeConfiguration : IEntityTypeConfiguration<CashoutBatchRequest>
{
    public void Configure(EntityTypeBuilder<CashoutBatchRequest> builder)
    {
        builder.ToTable("CashoutBatchRequest", "Payment");
        builder.Property(p => p.PaymentMethod).IsRequired();
        builder.Property(p => p.TotalAmount).IsRequired();
        builder.Property(p => p.CreatedBy).IsRequired();
        builder.Property(p => p.RowVersion).IsRowVersion();

        var navigation = builder.Metadata.FindNavigation(nameof(CashoutBatchRequest.CashoutItemRequests));
        navigation.SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
