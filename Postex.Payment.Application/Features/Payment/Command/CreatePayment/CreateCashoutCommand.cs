using Postex.Payment.Application.Contracts.Jibit.Models;

namespace Postex.Payment.Application.Features.Payment;

public class CreateCashoutCommand : IRequest<BaseResponse<CreateCashoutResponse>>
{
    public string PaymentMethodName { get; set; }
    public Guid UserId { get; set; }
    public Guid CorrelationID { get; set; }
    public List<CreateCashoutItemCommand> Items { get; set; }
}
public class CreateCashoutItemCommand 
{
    public int Amount { get; set; }
    public string IBanNumber { get; set; }
    public Guid CustomerId { get; set; }
    public string Description { get; set; }

}