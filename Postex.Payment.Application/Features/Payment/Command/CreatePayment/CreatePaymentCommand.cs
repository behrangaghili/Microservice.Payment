using Postex.Payment.Application.Contracts.Jibit.Models;

namespace Postex.Payment.Application.Features.Payment;

public class CreatePaymentCommand : IRequest<BaseResponse<CreatePaymentResponse>>
{
    [JsonIgnore]
    public Guid CorrelationID { get; set; }

    [JsonIgnore]
    public Guid UserId { get; set; }

    public Guid? PayerId { get; set; }

    public string? OrderNumber { get; set; }

    public string Description { get; set; }

    public int Amount { get; set; }

    public string? ReturnUrl { get; set; }

    public string? CancelUrl { get; set; }

    public string? AppName { get; set; }

    public string? Remark { get; set; }

    public string? MobileNo { get; set; }
    public string? IBanNumber { get; set; }
    public string PaymentMethod { get; set; }
}