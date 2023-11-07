
using Postex.Payment.Domain;
using RestSharp;

namespace Postex.Payment.Application
{
    public class WalletServiceClient : IWalletServiceClient
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<WalletServiceClient> _logger;

        public WalletServiceClient(IConfiguration configuration, ILogger<WalletServiceClient> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<WalletResponse> DepositAsync(WalletRequest walletRequest, Guid correlationId)
        {
            var walletServiceUrl = _configuration["WalletService:BaseUrl"];
            var walletApiUrl = "/deposit";
            var client = new RestClient(walletServiceUrl);
            var request = new RestRequest(walletApiUrl, Method.Post);
             
            request.AddHeader("x-correlation-id", correlationId);
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(walletRequest);

            var response = client.Execute<WalletResponse>(request);

            //Log the response status code
            _logger.LogInformation($"Deposit response status code: {response.StatusCode}");

            return response.Data;
        }

        public Task<WalletResponse> WithdrawAsync(WalletRequest walletRequest, Guid correlationId)
        {
            throw new NotImplementedException();
        }
    }
}
