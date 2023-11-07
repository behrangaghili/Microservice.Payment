namespace Postex.Payment.Application.Features.Payment;

public class CreatePaymentRefundCommand :  IRequest<BaseResponse<CreatePaymentRefundResponse>>
{
    public CreatePaymentRefundCommand() { }

    public CreatePaymentRefundCommand(Guid correlationID, Guid userID, Guid paymentRequestId, int amount, string remark)
    {
        CorrelationID = correlationID;
        UserID = userID;
        PaymentRequestId = paymentRequestId;
        Amount = amount;
        Remark = remark;
    }

    [JsonIgnore]
    public Guid CorrelationID { get; set; }
    [JsonIgnore]
    public Guid UserID { get; set; }
    public Guid PaymentRequestId { get; set; }
    public int Amount { get; set; }
    public string Remark { get; set; }
}