namespace Postex.Parcel.Domain.AggregatesModel.PaymentAggregate;

[Table("PaymentRequests",Schema = "Payment")]
public class PaymentRequest : BaseEntity<long>
{
    public PaymentRequest()
    {
        PaymentState = PaymentRequestState.Pending;
        SentOrderNumber = 1;
        _paymentRequestRefunds = new List<PaymentRequestRefund>();
        CreatedOn = DateTime.UtcNow;
    }

    public Guid GID { get; set; }
    
    public Guid CorrelationID { get; private set; }

    public string? MobileNo { get; private set; }

    public Guid? PayerId { get; private set; }

    public Guid UserID { get; private set; }

    [MaxLength(30)]
    public string OrderNumber { get; private set; }

    public long SentOrderNumber { get; private set; }

    public PaymentRequestState PaymentState { get; private set; }

    [MaxLength(100)]
    public string? FailReason { get; private set; }

    [MaxLength(250)]
    public string? Description { get; private set; }

    [MaxLength(30)]
    public string? PaymentMethod { get; private set; }

    [MaxLength(30)]
    public string? PaymentToken { get; private set; }

    public int Amount { get; private set; }

    [MaxLength(250)]
    public string ReturnUrl { get; private set; }

    [MaxLength(30)]
    public string? CardNumber { get; private set; }

    [MaxLength(250)]
    public string CancelUrl { get; private set; }

    [MaxLength(50)]
    public string AppName { get; private set; }

    [MaxLength(250)]
    public string? Remark { get; private set; }

    [MaxLength(250)]
    public string? SaleReferenceId { get; private set; }

    [MaxLength(250)]
    public string? ErrorGatewayCode { get; private set; }

    // Navigations
    private readonly List<PaymentRequestRefund> _paymentRequestRefunds;

    public virtual IReadOnlyCollection<PaymentRequestRefund> PaymentRequestRefunds => _paymentRequestRefunds;


    // Domain Services
    public PaymentRequestRefund AddRefund(Guid userId, int amount, string? remark)
    {
        // check for refund
        if (amount > this.Amount)
        {
            throw new PaymentDomainException("Refund amount must be less than Payment amount");
        }

        var sumRefundAmount = _paymentRequestRefunds.Where(p=>p.RefundState==RefundState.Completed).Sum(p => p.Amount);
        if (sumRefundAmount > 0 && sumRefundAmount + amount > this.Amount)
        {
            throw new PaymentDomainException("sum of refund amounts must be less than Payment amount");
        }
        if (string.IsNullOrEmpty(SaleReferenceId))
        {
            throw new PaymentDomainException("SaleReferenceId must not be null");
        }
        if (this.PaymentState != PaymentRequestState.Completed)
        {
            throw new PaymentDomainException("Payment State is not completed");
        }

        PaymentRequestRefund refund = new(this.Id, userId, amount, remark);
        this._paymentRequestRefunds.Add(refund);

        this.ModifiedBy= userId;
        this.ModifiedOn=DateTime.UtcNow;
        return refund;
    }

    public virtual void SetPaymentMethod(string paymentMethod)
    {
        if (string.IsNullOrEmpty(paymentMethod))
        {
            throw new PaymentDomainException("Payment Method is Empty");
        }

        PaymentMethod = paymentMethod;
    }

    public virtual void SetCardNumber(string cardNumber)
    {
        if (string.IsNullOrEmpty(cardNumber))
        {
            throw new PaymentDomainException("Card Number is Empty");
        }
        CardNumber = cardNumber;
    }

    public virtual void SetSaleReferenceId(string saleReferenceId)
    {
        if (string.IsNullOrEmpty(saleReferenceId))
        {
            throw new PaymentDomainException("Sale ReferenceId is Empty");
        }
        SaleReferenceId = saleReferenceId;
    }

    public virtual void SetSentOrderNumber(long sentOrderNumber)
    {
        SentOrderNumber = sentOrderNumber;
    }

    public virtual void SetPaymentToken(string token)
    {
        if (string.IsNullOrEmpty(token))
        {
            throw new PaymentDomainException("Payment Token is Empty");
        }

        PaymentToken = token;
    }

    public virtual void SetFailReasoned(string failReason)
    {
        if (string.IsNullOrEmpty(failReason))
        {
            throw new PaymentDomainException("Fail Reason  is Empty");
        }

        FailReason = failReason;
    }

    public virtual void SetAsProcessing()
    {
        if (PaymentState == PaymentRequestState.Processing)
        {
            return;
        }

        PaymentState = PaymentRequestState.Processing;
    }

    public virtual void SetAsCompleted()
    {
        if (PaymentState == PaymentRequestState.Completed)
        {
            return;
        }

        PaymentState = PaymentRequestState.Completed;
        FailReason = null;


    }

    public virtual void SetAsFailed(string failReason, string? errorGatewayCode)
    {
        if (PaymentState == PaymentRequestState.Completed)
        {
            return;
        }

        ErrorGatewayCode = errorGatewayCode;
        PaymentState = PaymentRequestState.Failed;
        FailReason = failReason;
    }
}