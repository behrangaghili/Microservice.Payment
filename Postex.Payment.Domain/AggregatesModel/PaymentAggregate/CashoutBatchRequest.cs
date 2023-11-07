namespace Postex.Parcel.Domain.AggregatesModel.PaymentAggregate
{
    [Table("CashoutBatchRequest", Schema = "Payment")]
    public class CashoutBatchRequest : BaseEntity<Guid>
    {
        public CashoutBatchRequest()
        {
            CreatedOn = DateTime.Now;
        }
        public Guid CorrelationID { get; set; }
        public Guid UserID { get; private set; }
        [MaxLength(30)]
        public string? PaymentMethod { get; set; }
        public int TotalAmount { get; set; }
        public List<CashoutItemRequest> CashoutItemRequests { get; set; }
    }
}
