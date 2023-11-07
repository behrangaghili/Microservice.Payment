using Postex.Parcel.Domain;

namespace Postex.Payment.Application.Features.Payment;

public class CreateHookCommand : IRequest<PaymentWebhookResult>
{
    public CreateHookCommand()
    {
    }

    public CreateHookCommand(Guid paymentRequestId, string paymentMethod)
    {
        PaymentRequestId = paymentRequestId;
        PaymentMethod = paymentMethod;
    }

    public Guid CorrelationId { get; set; }

    public Guid PaymentRequestId { get; private set; }

    public string PaymentMethod { get; private set; }
}