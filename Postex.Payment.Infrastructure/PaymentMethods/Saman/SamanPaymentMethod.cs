using Postex.Parcel.Domain;
using Postex.Payment.Application.Contracts.Jibit.Models;
using Postex.Payment.Application.Contracts.Saman;
using Postex.Payment.Infrastructure.PaymentMethods.Jibit;
using Postex.SharedKernel.Api;
using RestSharp;
using System.Net;
using static Postex.Payment.Infrastructure.PaymentMethods.Jibit.RestTools;


namespace Postex.Payment.Infrastructure.PaymentMethods.Saman
{
    public class SamanPaymentMethod : IPaymentMethod
    {
        #region field
        private readonly ILogger<SamanPaymentMethod> _logger;
        private readonly IWriteRepository<PaymentRequest> _writeRepository;
        private readonly IReadRepository<PaymentRequest> _readRepository;
        private readonly string PaymentMethodName;
        private readonly bool PaymentMethodActive;
        private readonly RestTools _restTools;
        private readonly string _merchantId;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _baseurl;
        private readonly string _payEndpoint;
        private readonly string _VeryfyEndposint;
        public string Name => this.PaymentMethodName;

        public bool IsActive => this.PaymentMethodActive;
        #endregion

        #region Ctor
        public SamanPaymentMethod(ILogger<SamanPaymentMethod> logger,
  IConfiguration configuration,
  IWriteRepository<PaymentRequest> writeRepository,
  IReadRepository<PaymentRequest> readRepository,
  IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _writeRepository = writeRepository;
            _readRepository = readRepository;
            PaymentMethodName = configuration["PaymentMethod:Saman:PaymentMethodName"];
            PaymentMethodActive = Convert.ToBoolean(configuration["PaymentMethod:Saman:IsActive"]);
            _merchantId = configuration["PaymentMethod:Saman:TerminalId"];
            _baseurl = configuration["PaymentMethod:Saman:BaseUrl"];
            _payEndpoint = configuration["PaymentMethod:Saman:PayEndpoint"];
            _VeryfyEndposint = configuration["PaymentMethod:Saman:VeryfyEndposint"];
            _restTools = new RestTools();
            _httpContextAccessor = httpContextAccessor;
        }
        #endregion

        #region methods
        public async Task PayAsync(PaymentRequest paymentRequest, CancellationToken cancellationToken)
        {
            paymentRequest.SetAsProcessing();
            _logger.LogInformation(" Payment Request is Processing - PaymentRequestId: {@Id}", paymentRequest.Id);
            paymentRequest.SetPaymentMethod(Name);
            await SavePayment(paymentRequest, cancellationToken);
            var httpRequest = _httpContextAccessor.HttpContext.Request;
            string callBackUrl = $"{httpRequest.Scheme}://{httpRequest.Host}/{paymentRequest.GID}/{Name}/webhook";

            long sentOrderNumber = DateTime.Now.Ticks;
            paymentRequest.SetSentOrderNumber(sentOrderNumber);
            var result = await SendViaToken(new SamanTokenModel
            {
                TerminalId = _merchantId,
                Amount = paymentRequest.Amount,
                RedirectUrl = callBackUrl,
                ResNum = paymentRequest.OrderNumber,
                CellNumber = long.Parse(paymentRequest.MobileNo)
            }
           );

            var response = _httpContextAccessor.HttpContext.Response;
            response.Clear();

            if (!result.success || result.data == null || result.data.status != 1)
            {
                paymentRequest.SetAsFailed("Error in get payment token", result.data == null ? "empty" : result.data.errorCode.ToString());
                await SavePayment(paymentRequest, cancellationToken);
                _logger.LogInformation(" Payment Request is Processing - PaymentRequestId: {@Id}", paymentRequest.Id);

                // Redirect To cancel Page
                string errorMessage = $"Error In Get Bank Token.";
                string cancelUrl = $"{paymentRequest.CancelUrl}?errorMessage={errorMessage}&TransactionId={paymentRequest.Id}";
                response.Redirect(cancelUrl, true);
                await response.Body.FlushAsync(cancellationToken);
                await response.CompleteAsync();
                return;
            }


            string url = $@"https://{_httpContextAccessor.HttpContext.Request.Host}/";
            string endpoint = $@"samanconnect?ResNum={paymentRequest.OrderNumber}&MerchantId={_merchantId}&Amount={paymentRequest.Amount.ToString()}&Token={result.data.token}&RedirectUrl={System.Net.WebUtility.UrlEncode(callBackUrl)}&MobileNo={paymentRequest.MobileNo}";

            url += endpoint;
            _logger.LogInformation("Saman IPG URL:" + url);

            response.Redirect(url, true);
            await response.Body.FlushAsync(cancellationToken);
            await response.CompleteAsync();
        }
        public async Task<PaymentWebhookResult> HandleWebhookAsync(PaymentRequest paymentRequest, CancellationToken cancellationToken)
        {
            var query = _httpContextAccessor.HttpContext?.Request.Form;
            var state = query["State"].ToString();
            var stateCode = query["Status"].ToString();
            var resNum = query["ResNum"].ToString();
            var mID = _merchantId;
            var refNum = query["RefNum"].ToString();
            var RRN = query["RRN"].ToString();
            var tRACENO = query["TraceNo"].ToString();
            var Amount = query["Amount"].ToString();
            var securePan = query["SecurePan"].ToString();
            string data = $@"State:{state},stateCode:{stateCode},resNum:{resNum},mID:{mID}
                    ,refNum:{refNum},RRN:{RRN},TraceNo:{tRACENO},securePan:{securePan}";

            _logger.LogInformation($"Returned Data  from Saman :{data}");
            string errorMessage = "";
            if (stateCode != "2")
            {
                errorMessage = SamanResult.SamanPaymentResult(stateCode);
                paymentRequest.SetAsFailed(state, stateCode);
            }
            else
            {
                var VerifyRequestModel = new SamanVerifyInputModel
                {
                    RefNum = refNum,
                    TerminalNumber = mID
                };
                var verifyModel = await _restTools.SendRequest<SamanVerifyModel>(VerifyRequestModel, _baseurl, _VeryfyEndposint, Method.Post, null);
                if (verifyModel != null)
                {
                    stateCode = verifyModel.data.ResultCode.ToString();
                    if (verifyModel.success)
                    {
                        paymentRequest.SetAsCompleted();
                    }
                    else
                    {
                        errorMessage = SamanResult.SamanVerifyResult(verifyModel.data.ResultCode.ToString());
                        paymentRequest.SetAsFailed(errorMessage, verifyModel.data.ResultCode.ToString());
                    }
                }
                else
                {
                    paymentRequest.SetAsFailed(state, stateCode);
                }
            }
            await SavePayment(paymentRequest, cancellationToken);

            return CreateWebhookResult(paymentRequest, stateCode, errorMessage);
        }
        public Task RefundAsync(PaymentRequestRefund refund, PaymentRequest payment, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult> RequestCashout(CashoutBatchRequest Request, CancellationToken cancellationToken)
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

        #region tools

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

        private async Task SavePayment(PaymentRequest? paymentRequest, CancellationToken cancellationToken)
        {
            _writeRepository.Update(paymentRequest);
            await _writeRepository.SaveChangeAsync(cancellationToken);
        }
        public async Task<ResponseWarpper<SamanTokenResultModel>> SendViaToken(SamanTokenModel token)
        {
            token.Action = "token";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            return await _restTools.SendRequest<SamanTokenResultModel>(token, _baseurl, _payEndpoint, Method.Post, null);
        }
        #endregion
    }
}
