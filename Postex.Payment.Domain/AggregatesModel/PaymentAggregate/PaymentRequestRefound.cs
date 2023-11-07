namespace Postex.Parcel.Domain.AggregatesModel.PaymentAggregate;

[Table("PaymentRequestRefunds", Schema = "Payment")]
public class PaymentRequestRefund : BaseEntity<long>
{
    // for ef mapping
    private PaymentRequestRefund()
    {
        
    }
   
    public PaymentRequestRefund(long paymentRequestId, Guid userId, int amount, string? remark)
    {
        if(amount<=0)
        {
            throw new PaymentDomainException("Refund amount must be more than 0");
        }
        PaymentRequestId = paymentRequestId;
        CreatedBy = userId;
        Amount = amount;
        Remark = remark;
        RefundState = RefundState.Pending;
        SentOrderNumber = 1;
        CreatedOn = DateTime.UtcNow;
    }

    public long PaymentRequestId { get;private set; }

    public long SentOrderNumber { get; private set; }

    public RefundState RefundState { get; private set; }

    [MaxLength(100)]
    public string? FailReason { get; private set; }

    public int Amount { get; private set; }

    [MaxLength(100)]
    public string? Remark { get; private set; }

    [MaxLength(20)]
    public string? ErrorGatewayCode { get; private set; }

    // Domain Services
    public virtual void SetSentOrderNumber(long sentOrderNumber)
    {
        SentOrderNumber = sentOrderNumber;
    }

    public virtual void SetPaymentRequestId(long paymentRequestId)
    {
        PaymentRequestId = paymentRequestId;
    }
  
  
    public virtual void SetAsCompleted()
    {
        if (RefundState == RefundState.Completed)
        {
            return;
        }
        RefundState = RefundState.Completed;
        FailReason = null;
    }

    public virtual void SetAsFailed(string failReason, string? errorGatewayCode)
    {
        if (RefundState == RefundState.Completed)
        {
            return;
        }
        ErrorGatewayCode = errorGatewayCode;
        RefundState = RefundState.Failed;
        FailReason = failReason;
    }
}