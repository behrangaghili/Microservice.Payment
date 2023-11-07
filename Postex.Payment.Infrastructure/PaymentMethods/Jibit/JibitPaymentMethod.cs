using Postex.Parcel.Domain;
using Postex.Payment.Application.Contracts.Jibit.Models;
using Postex.SharedKernel.Api;
using Serilog.Parsing;

namespace Postex.Payment.Infrastructure.PaymentMethods.Jibit
{
    public class JibitPaymentMethod : IPaymentMethod
    {
        #region field
        private readonly ILogger<JibitPaymentMethod> _logger;
        private readonly IWriteRepository<PaymentRequest> _writeRepository;
        private readonly IReadRepository<PaymentRequest> _readRepository;
        private readonly string PaymentMethodName;
        private readonly bool PaymentMethodActive;
        private readonly RestTools _restTools;
        private readonly string _transferServiceApiKey;
        private readonly string _transferServiceSecretKey;
        private readonly string _baseurl;
        private readonly string _transferServiceName;
        private readonly string _transferServiceApiVer;
        private readonly string _transferbaseUrl;

        public string Name => this.PaymentMethodName;

        public bool IsActive => this.PaymentMethodActive;
        #endregion

        #region ctor
        public JibitPaymentMethod(
          ILogger<JibitPaymentMethod> logger,
          IConfiguration configuration,
          IWriteRepository<PaymentRequest> writeRepository,
          IReadRepository<PaymentRequest> readRepository
          )
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _writeRepository = writeRepository;
            _readRepository = readRepository;
            PaymentMethodName = configuration["PaymentMethod:Jibit:PaymentMethodName"];
            _transferServiceApiKey = configuration["PaymentMethod:Jibit:TransferService:ApiKey"];
            _transferServiceSecretKey = configuration["PaymentMethod:Jibit:TransferService:SecretKey"];
            _transferServiceName = configuration["PaymentMethod:Jibit:TransferService:Name"];
            _transferServiceApiVer = configuration["PaymentMethod:Jibit:TransferService:ApiVersion"];
            _baseurl = configuration["PaymentMethod:Jibit:BaseUrl"];
            PaymentMethodActive = Convert.ToBoolean(configuration["PaymentMethod:Jibit:IsActive"]);
            _restTools = new RestTools();
            _transferbaseUrl = $"{_baseurl}/{_transferServiceName}/{_transferServiceApiVer}";
        }
        #endregion

        #region Methods
        public Task<PaymentWebhookResult> HandleWebhookAsync(PaymentRequest payment, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task PayAsync(PaymentRequest paymentRequest, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task RefundAsync(PaymentRequestRefund refund, PaymentRequest payment, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResult> RequestCashout(CashoutBatchRequest Request, CancellationToken cancellationToken)
        {
            TransferRequestModel model = new TransferRequestModel()
            {
                BatchID = Request.Id.ToString(),
                SubmissionMode = "TRANSFER",
                Transfers = Request.CashoutItemRequests.Select(p => new Transfer()
                {
                    Amount = p.Amount,
                    Currency = "IRR",
                    Description = p.Description,
                    Destination = p.IBanNumber,
                    TransferID = p.Id.ToString(),
                    TransferMode = "ACH",
                    Cancellable = false,
                }).ToList()
            };
            return await Transfer(model);
        }
        private async Task<string> getToken()
        {
            Dictionary<string, string> headerItem = new Dictionary<string, string>();
            headerItem.Add("Content-Type", "application/json");
            var Tokenresponce = await _restTools.SendRequest<GenerateTokenResponseModel>(new GenerateTokenRequestModel()
            {
                ApiKey = this._transferServiceApiKey,
                SecretKey = this._transferServiceSecretKey
            },
              $"{_baseurl}/{_transferServiceName}/{_transferServiceApiVer}",
              "/tokens/generate/",
              RestSharp.Method.Post,
              headerItem);
            if (Tokenresponce.success)
            {
                //cash refresh token
                return Tokenresponce.data.AccessToken;
            }
            return null;
        }
        private async Task<ApiResult> Transfer(TransferRequestModel model)
        {
            var token = await getToken();
            Dictionary<string, string> headerItem = new Dictionary<string, string>();
            headerItem.Add("Authorization", $"Bearer {token}");
            var transferResponce = await _restTools.SendRequest(model,
              _transferbaseUrl, "/transfers/", RestSharp.Method.Post, headerItem);
            if (transferResponce.success)
            {
                Newtonsoft.Json.JsonConvert.DeserializeObject<TransferResponseOkModel>(transferResponce.data);
                return new ApiResult(true, "");
            }
            else
            {
                var errors = Newtonsoft.Json.JsonConvert.DeserializeObject<TransferResponseErroModel>(transferResponce.data);
                return new ApiResult(false, string.Join(",", errors?.errors));
            }
        }
        public async Task<ApiResult<TrackTransactionResponseModel>> TrackTransaction(Guid? BatchId, Guid? TransferId)
        {
            var token = await getToken();
            Dictionary<string, string> headerItem = new Dictionary<string, string>();
            headerItem.Add("Authorization", $"Bearer {token}");
            string queryString = "?";
            if (BatchId.HasValue && TransferId.HasValue)
                queryString += "batchID=" + BatchId.Value.ToString().ToLower() + "&" + "transferID=" + TransferId.Value.ToString().ToLower();
            if (BatchId.HasValue)
                queryString += "batchID=" + BatchId.Value.ToString().ToLower();
            else if (TransferId.HasValue)
                queryString += "transferID=" + TransferId.Value.ToString().ToLower();
            var transferResponce = await _restTools.SendRequest(null,
             _transferbaseUrl, $"/transfers{queryString}", RestSharp.Method.Get, headerItem);
            if (transferResponce.success)
            {
                var trackOkResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<TrackTransactionResponseModel>(transferResponce.data);
                return new ApiResult<TrackTransactionResponseModel>(true, trackOkResponse, "");
            }
            else
            {
                var trackErroResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<TransferResponseErroModel>(transferResponce.data);
                return new ApiResult<TrackTransactionResponseModel>(false, null, string.Join(",", trackErroResponse?.errors));
            }

        }
        public async Task<ApiResult<TransferHistoreyResponse>> TransactionHistory(TransferHistoryRequest request)
        {
            var token = await getToken();
            Dictionary<string, string> headerItem = new Dictionary<string, string>();
            headerItem.Add("Authorization", $"Bearer {token}");
            string queryString = $"?page={request.page}&size={request.size}";
            if (request.from.HasValue)
                queryString += "&from="+request.from.ToString();
            if (request.to.HasValue)
                queryString += "&to=" + request.to.ToString();
            if(!string.IsNullOrEmpty(request.state))
                queryString += $"&state{request.state}";
            var transferHistory = await _restTools.SendRequest(null,
            _transferbaseUrl, $"/transfers/filter{queryString}", RestSharp.Method.Get, headerItem);
            if (transferHistory.success)
            {
                var transferHistoryResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<TransferHistoreyResponse>(transferHistory.data);
                return new ApiResult<TransferHistoreyResponse>(true, transferHistoryResponse, "");
            }
            else
            {
                var trackErroResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<TransferResponseErroModel>(transferHistory.data);
                return new ApiResult<TransferHistoreyResponse>(false, null, string.Join(",", trackErroResponse?.errors));
            }
        }
        #endregion
    }
}
