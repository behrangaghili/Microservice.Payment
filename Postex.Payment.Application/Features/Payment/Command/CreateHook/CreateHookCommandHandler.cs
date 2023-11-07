using Postex.Parcel.Domain;
using Postex.Parcel.Domain.AggregatesModel.Wallet;
using Postex.SharedKernel.Common.Enums;

namespace Postex.Payment.Application.Features.Payment;

public partial class CreateHookCommandHandler : IRequestHandler<CreateHookCommand, PaymentWebhookResult>
{
    private readonly IPaymentMethodResolver _paymentMethodResolver;
    private readonly ILogger<CreateHookCommandHandler> _logger;
    private readonly IReadRepository<PaymentRequest> _readRepository;
    private readonly IWalletServiceClient _walletServiceClient;

    public CreateHookCommandHandler(IReadRepository<PaymentRequest> readRepository, IPaymentMethodResolver paymentMethodResolver, ILogger<CreateHookCommandHandler> logger, IWalletServiceClient walletServiceClient)
    {
        _paymentMethodResolver = paymentMethodResolver ?? throw new ArgumentNullException(nameof(paymentMethodResolver));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _readRepository = readRepository ?? throw new ArgumentNullException(nameof(readRepository)); ;
        _walletServiceClient = walletServiceClient;
    }

    public async Task<PaymentWebhookResult> Handle(CreateHookCommand request, CancellationToken cancellationToken)
    {
        var payment = await _readRepository.Table.FirstOrDefaultAsync(p => p.GID == request.PaymentRequestId);

        if (payment == null) throw new EntityNotFoundException($"{nameof(payment)} is null");

        _logger.LogInformation(" Web hook Process is Starting - PaymentRequestInfo: {@request}", request);

        //verify payment result and settle
        var paymentService = _paymentMethodResolver.Resolve(request.PaymentMethod);
        var webhookResult = await paymentService.HandleWebhookAsync(payment, cancellationToken);

        //deposit wallet after successful payment
        if (webhookResult.IsSuccess == true)
        {
            await _walletServiceClient.DepositAsync(new Domain.WalletRequest
            {
                Amount = payment.Amount,
                UserId = payment.UserID,
                TypeId = (int)TransactionTypes.OnlineCharge,
                Tag = request.PaymentRequestId.ToString(),
                Description = "شارژ کیف پول ",
                SubWallet = string.Empty
            }, request.CorrelationId);
        }

        _logger.LogInformation("Webhook Process is done Successfully - PaymentRequestInfo: {@request}", request);

        return webhookResult;
    }
}
