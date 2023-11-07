
using Postex.Parcel.Domain;
using Postex.Parcel.Domain.AggregatesModel.PaymentAggregate;
using Postex.SharedKernel.Api;
using Postex.Payment.Application.Contracts.Jibit.Models;

namespace Postex.Payment.Application.Contracts;

public interface IPaymentMethod
{
    string Name { get; }
    bool IsActive { get; }

    public Task PayAsync(PaymentRequest paymentRequest, CancellationToken cancellationToken);

    public Task RefundAsync(PaymentRequestRefund refund, PaymentRequest payment, CancellationToken cancellationToken);

    public Task<PaymentWebhookResult> HandleWebhookAsync(PaymentRequest payment, CancellationToken cancellationToken);
    Task<ApiResult> RequestCashout(CashoutBatchRequest Request, CancellationToken cancellationToken);
    Task<ApiResult<TrackTransactionResponseModel>> TrackTransaction(Guid? BatchId, Guid? TransferId);
    Task<ApiResult<TransferHistoreyResponse>> TransactionHistory(TransferHistoryRequest request);
}