using Postex.Parcel.Domain.AggregatesModel.PaymentAggregate.Enums;

namespace Postex.Parcel.Domain.AggregatesModel.PaymentAggregate
{
    [Table("CashoutItemRequest", Schema = "Payment")]
    public class CashoutItemRequest : BaseEntity<Guid>
    {
        public CashoutItemRequest()
        {
            this.CashoutState = CashoutState.INITIALIZED;
            CreatedOn = DateTime.Now;
        }
        public int Amount { get; set; }
        public string IBanNumber { get; set; }
        public CashoutState CashoutState { get; set; }
        public string? FailReason { get; set; }
        public int? FeeAmount { get; set; }
        public string? DestinationFirstName { get; set; }
        public string? DestinationLastName { get; set; }
        public CashoutBatchRequest CashoutBatchRequest { get; set; }
        public Guid CustomerId { get; set; }
        public string Description { get; set; }
    }
}
