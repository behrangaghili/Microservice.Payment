using MellatService;
using Postex.Parcel.Domain;
using Postex.Payment.Application.Contracts.Jibit.Models;
using Postex.SharedKernel.Api;

namespace Postex.Payment.Infrastructure.PaymentMethods.Mellat;

public class MalletPaymentMethod : IPaymentMethod
{
    private readonly ILogger<MalletPaymentMethod> _logger;
    private readonly IWalletServiceClient _walletServiceClient;

    private readonly IWriteRepository<PaymentRequest> _writeRepository;
    private readonly IReadRepository<PaymentRequest> _readRepository;
    private readonly IReadRepository<PaymentRequestRefund> _readRefundRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private PaymentGatewayClient mellatClient = new();
    // config fields
    private readonly long TerminalId;
    private readonly string UserName;
    private readonly string UserPassword;
    private readonly string PaymentMethodName;
    private readonly bool PaymentMethodActive;

    public MalletPaymentMethod(ILogger<MalletPaymentMethod> logger, IWalletServiceClient walletServiceClient,
        IConfiguration configuration, IWriteRepository<PaymentRequest> writeRepository,
        IReadRepository<PaymentRequestRefund> readRefundRepository,
        IReadRepository<PaymentRequest> readRepository, IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _walletServiceClient = walletServiceClient;
        _writeRepository = writeRepository ?? throw new ArgumentNullException(nameof(writeRepository));
        _readRepository = readRepository ?? throw new ArgumentNullException(nameof(readRepository));
        _readRefundRepository = readRefundRepository ?? throw new ArgumentNullException(nameof(readRefundRepository));
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));

        // Read Mellet Configuration Information
        TerminalId = Convert.ToInt64(configuration["PaymentMethod:Mellet:TerminalId"]);
        UserName = configuration["PaymentMethod:Mellet:UserName"];
        UserPassword = configuration["PaymentMethod:Mellet:UserPassword"];
        PaymentMethodName = configuration["PaymentMethod:Mellet:PaymentMethodName"];
        PaymentMethodActive = Convert.ToBoolean(configuration["PaymentMethod:Mellet:IsActive"]);
    }

    public string Name => PaymentMethodName;
    public bool IsActive => PaymentMethodActive;

    public async Task PayAsync(PaymentRequest paymentRequest, CancellationToken cancellationToke)
    {
        paymentRequest.SetAsProcessing();
        _logger.LogInformation(" Payment Request is Processing - PaymentRequestId: {@Id}", paymentRequest.Id);
        paymentRequest.SetPaymentMethod(Name);
        await SavePayment(paymentRequest, cancellationToke);
        var httpRequest = _httpContextAccessor.HttpContext.Request;
        string callBackUrl = $"{httpRequest.Scheme}://{httpRequest.Host}/{paymentRequest.GID}/{Name}/webhook";

        MellatService.PaymentGatewayClient payment = new();
        string? localDate = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0') +
                   DateTime.Now.Day.ToString().PadLeft(2, '0');
        string? localTime = DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') +
                   DateTime.Now.Second.ToString().PadLeft(2, '0');
        //PayerId is stored in the database with each request. However, due to the bank's strict formatting requirements for the PayerId, I am only sending a value of zero.
        long payerId = 0;
        long sentOrderNumber = DateTime.Now.Ticks;
        paymentRequest.SetSentOrderNumber(sentOrderNumber);

        var authorization = await payment.bpPayRequestAsync(TerminalId, UserName, UserPassword, sentOrderNumber, paymentRequest.Amount,
            localDate, localTime, "", callBackUrl, payerId);

        if (authorization != null && !string.IsNullOrEmpty(authorization.Body.@return))
        {
            var resCode = authorization.Body.@return.Split(",")[0];

            var response = _httpContextAccessor.HttpContext.Response;
            response.Clear();

            if (resCode == "0")
            {
                var refid = authorization.Body.@return.Split(",")[1];
                paymentRequest.SetPaymentToken(refid);
                await SavePayment(paymentRequest, cancellationToke);

                string url = $"https://{_httpContextAccessor.HttpContext.Request.Host}/mellatconnect/{refid}?mobileNo={paymentRequest.MobileNo}";
                response.Redirect(url, true);
                await response.Body.FlushAsync(cancellationToke);
                await response.CompleteAsync();
            }
            else
            {
                paymentRequest.SetAsFailed("Error in get payment token", resCode);
                await SavePayment(paymentRequest, cancellationToke);
                _logger.LogInformation(" Payment Request is Processing - PaymentRequestId: {@Id}", paymentRequest.Id);

                // Redirect To cancel Page
                string errorMessage = $"Error In Get Bank Token.";
                string cancelUrl = $"{paymentRequest.CancelUrl}?errorMessage={errorMessage}&TransactionId={paymentRequest.Id}";
                response.Redirect(cancelUrl, true);
                await response.Body.FlushAsync(cancellationToke);
                await response.CompleteAsync();
            }
        }
    }

    public async Task<PaymentWebhookResult> HandleWebhookAsync(PaymentRequest paymentRequest, CancellationToken cancellationToken)
    {
        IFormCollection? form = _httpContextAccessor.HttpContext?.Request.Form;
        string? resultCode = form?["ResCode"][0];
        string verifyResCode = "0";
        string settleResCode = "0";
        string errorMessage = null;

        _logger.LogInformation(" Payment Request Webhook is Processing - PaymentRequestId: {@Id}", paymentRequest.Id);

        if (paymentRequest.PaymentState == PaymentRequestState.Completed)
        {
            return new PaymentWebhookResult
            {
                CallbackUrl = paymentRequest.CancelUrl,
                IsSuccess = false,
                ResCode = "-1"
            };
        }


        if (IsSuccessful(resultCode))
        {
            string? saleOrderId = form?["SaleOrderId"][0];
            long saleReferenceId = Convert.ToInt64(form?["SaleReferenceId"][0]);
            string? cardHolderPan = form?["CardHolderPan"][0];

            paymentRequest.SetCardNumber(cardHolderPan);
            paymentRequest.SetSaleReferenceId(saleReferenceId.ToString());

            verifyResCode = await VerifyRequest(resultCode, saleOrderId, saleReferenceId);

            if (IsSuccessful(verifyResCode))
            {
                settleResCode = await SettlePayment(resultCode, saleOrderId, saleReferenceId);
            }

        }

        if (!IsSuccessful(verifyResCode)) resultCode = verifyResCode;
        if (!IsSuccessful(settleResCode)) resultCode = settleResCode;

        if (!IsSuccessful(resultCode))
        {
            errorMessage = MellatPaymentResult.MellatResult(resultCode);
            paymentRequest.SetAsFailed(errorMessage, resultCode);
        }
        else
            paymentRequest.SetAsCompleted();

        await SavePayment(paymentRequest, cancellationToken);

        return CreateWebhookResult(paymentRequest, resultCode, errorMessage);
    }

    private static bool IsSuccessful(string resCode)
    {
        return resCode == "0";
    }

    private static PaymentWebhookResult CreateWebhookResult(PaymentRequest paymentRequest, string resCode, string errorMessage)
    {
        var result = new PaymentWebhookResult
        {
            ResCode = resCode,
            IsSuccess = IsSuccessful(resCode),
            ErrorMessage = errorMessage
        };

        result.CallbackUrl = result.IsSuccess ? $"{paymentRequest.ReturnUrl}?id={paymentRequest.GID}" : $"{paymentRequest.CancelUrl}?message={HttpUtility.UrlEncode(errorMessage)}&id={paymentRequest.GID}";

        return result;
    }

    private async Task<string> SettlePayment(string? resCode, string? saleOrderId, long saleReferenceId)
    {
        var settleResponse = await mellatClient.bpSettleRequestAsync(TerminalId, UserName, UserPassword, Convert.ToInt64(saleOrderId), Convert.ToInt64(saleOrderId), saleReferenceId);
        resCode = settleResponse.Body.@return.Split(",")[0];
        return resCode;
    }

    private async Task<string> VerifyRequest(string? resCode, string? saleOrderId, long saleReferenceId)
    {
        var verifyRequest = await mellatClient.bpVerifyRequestAsync(TerminalId, UserName, UserPassword, Convert.ToInt64(saleOrderId), Convert.ToInt64(saleOrderId), saleReferenceId);
        resCode = verifyRequest.Body.@return.Split(",")[0];
        return resCode;
    }

    public async Task RefundAsync(PaymentRequestRefund requestRefund, PaymentRequest paymentRequest, CancellationToken cancellationToken)
    {
        MellatService.PaymentGatewayClient paymentGateway = new();
        _logger.LogInformation(" Payment Request Refund is Processing - PaymentRequestId: {@Id}", paymentRequest.Id);

        long orderNumber = await GenerateSentOrderNumberForRefund(cancellationToken);
        var authorization = await paymentGateway.bpRefundRequestAsync(TerminalId, UserName, UserPassword, orderNumber, paymentRequest.SentOrderNumber, Convert.ToInt64(paymentRequest.SaleReferenceId), requestRefund.Amount);
        if (authorization != null && !string.IsNullOrEmpty(authorization.Body.@return))
        {
            var ResCode = authorization.Body.@return.Split(",")[0];
            if (ResCode == "0")
            {
                requestRefund.SetAsCompleted();
                await SavePayment(paymentRequest, cancellationToken);
            }
            else
            {
                string errorMessage = MellatPaymentResult.MellatResult(ResCode);
                requestRefund.SetAsFailed(errorMessage, ResCode);
                await SavePayment(paymentRequest, cancellationToken);
                throw new ApplicationException($"error in Payment Request Refund with RefundId:{requestRefund.Id}. Message: {errorMessage} ");
            }
        }
        // Local Functions
        async Task<long> GenerateSentOrderNumberForRefund(CancellationToken cancellationToken)
        {
            long orderNumber = 0;

            long? num = await _readRefundRepository.TableNoTracking.MaxAsync(x => x.SentOrderNumber, cancellationToken: cancellationToken);
            if (num.HasValue)
            {
                orderNumber = num.Value;
                orderNumber++;
            }
            requestRefund.SetSentOrderNumber(orderNumber);
            await SavePayment(paymentRequest, cancellationToken);
            return orderNumber;
        }
    }

    #region helper
    private async Task SavePayment(PaymentRequest? paymentRequest, CancellationToken cancellationToken)
    {
        _writeRepository.Update(paymentRequest);
        await _writeRepository.SaveChangeAsync(cancellationToken);
    }



    Task<ApiResult> IPaymentMethod.RequestCashout(CashoutBatchRequest Request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<ApiResult<TrackTransactionResponseModel>> TrackTransaction(Guid? BatchId, Guid? TransferId)
    {
        throw new NotImplementedException();
    }

    public Task<ApiResult<TransferHistoreyResponse>> TransactionHistory(TransferHistoryRequest request)
    {
        throw new NotImplementedException();
    }
    #endregion
}