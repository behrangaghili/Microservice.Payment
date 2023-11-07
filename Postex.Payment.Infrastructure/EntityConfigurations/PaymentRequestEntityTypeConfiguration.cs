namespace Postex.Payment.Infrastructure.EntityConfigurations;

class PaymentRequestEntityTypeConfiguration : IEntityTypeConfiguration<PaymentRequest>
{
    public void Configure(EntityTypeBuilder<PaymentRequest> builder)
    {
        builder.Property(p => p.CancelUrl).IsRequired();
        builder.Property(p => p.ReturnUrl).IsRequired();
        builder.Property(p => p.CreatedBy).IsRequired();
        builder.Property(p => p.AppName).IsRequired();
        builder.Property(p => p.RowVersion).IsRowVersion();

        var navigation = builder.Metadata.FindNavigation(nameof(PaymentRequest.PaymentRequestRefunds));
        // DDD Patterns comment:
        //Set as field (New since EF 1.1) to access the OrderItem collection property through its field
        navigation.SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
