namespace Postex.Payment.Infrastructure.EntityConfigurations;

class PaymentRequestRefundEntityTypeConfiguration : IEntityTypeConfiguration<PaymentRequestRefund>
{
    public void Configure(EntityTypeBuilder<PaymentRequestRefund> builder)
    {
        builder.Property(p => p.CreatedBy).IsRequired();
        builder.Property(p => p.RowVersion).IsRowVersion();
    }
}