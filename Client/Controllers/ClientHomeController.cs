using Client.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;
using RestSharp;
using Method = RestSharp.Method;
using Newtonsoft.Json;

namespace Client.Controllers
{
    public class ClientHomeController : Controller
    {
        private readonly ILogger<ClientHomeController> _logger;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;

        public ClientHomeController(ILogger<ClientHomeController> logger, IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _logger = logger;
            _clientFactory = clientFactory; 
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Test()
        {

            string apiUrl = _configuration.GetValue<string>("PaymentAPI:Url");

            var client = new RestClient(apiUrl);
            var request = new RestRequest(apiUrl, Method.Post);
            request.AddHeader("x-api-version", "1");
            request.AddHeader("x-correlation-id", "d7c00541-f343-45c7-8da2-40ecf27f9b1b");
            request.AddHeader("x-user-id", "ad02af48-b1f9-41e2-8b83-f9c9be9b6ae1");
            request.AddParameter("application/json", "{\"PaymentMethod\":\"mellat\",\r\n  \"payerId\": \"cd02af48-b1f9-41e2-8b83-f9c9be9b6ae6\",\"mobileNo\": \"989123898058\",\r\n  \"orderNumber\": \"ORD12345\",\r\n  \"description\": \"Payment for Product XYZ\",\r\n  \"amount\": 123456,\r\n  \"returnUrl\": \"https://example.com/payment/success\",\r\n  \"cancelUrl\": \"https://example.com/payment/cancel\",\r\n  \"appName\": \"MyApp\",\r\n  \"remark\": \"Optional remark about the payment\"\r\n}", ParameterType.RequestBody);
            RestResponse response = client.Execute(request);
            ViewBag.ApiResponse = response.Content;
            return View("Test");
        }
        public IActionResult TestSaman()
        {

            string apiUrl = _configuration.GetValue<string>("PaymentAPI:Url");

            var client = new RestClient(apiUrl);
            var request = new RestRequest(apiUrl, Method.Post);
            request.AddHeader("x-api-version", "1");
            request.AddHeader("x-correlation-id", "d7c00541-f343-45c7-8da2-40ecf27f9b1b");
            request.AddHeader("x-user-id", "ad02af48-b1f9-41e2-8b83-f9c9be9b6ae1");
            request.AddParameter("application/json", "{\"PaymentMethod\":\"Saman\", \r\n  \"payerId\": \"cd02af48-b1f9-41e2-8b83-f9c9be9b6ae6\",\"mobileNo\": \"09139064053\",\r\n  \"orderNumber\": \"ORD123455\",\r\n  \"description\": \"Payment for Product XYZ\",\r\n  \"amount\": 30000,\r\n  \"returnUrl\": \"https://example.com/payment/success\",\r\n  \"cancelUrl\": \"https://example.com/payment/cancel\",\r\n  \"appName\": \"MyApp\",\r\n  \"remark\": \"Optional remark about the payment\"\r\n}", ParameterType.RequestBody);
            RestResponse response = client.Execute(request);
            ViewBag.ApiResponse = response.Content;
            return View("TestSaman");
        }
        [HttpPost]
        public IActionResult TestSamanWithParameters(PaymentRequestViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            string apiUrl = _configuration.GetValue<string>("PaymentAPI:Url");
            var client = new RestClient(apiUrl);
            var request = new RestRequest(apiUrl, Method.Post);
            request.AddHeader("x-api-version", model.ApiVersion);
            request.AddHeader("x-correlation-id", model.CorrelationId);
            request.AddHeader("x-user-id", model.UserId);
            request.AddParameter("application/json", JsonConvert.SerializeObject(model), ParameterType.RequestBody);
            RestResponse response = client.Execute(request);
            ViewBag.ApiResponse = response.Content;
            return View("TestSamanWithParameters");
        }
        public async Task<IActionResult> CashoutTestAsync()
        {

            var options = new RestClientOptions("https://localhost:59370")
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest("/api/v1/payment/cashout", Method.Post);
            request.AddHeader("x-user-id", "3b68bbad-a197-467b-8b8e-465fff2887b9");
            request.AddHeader("x-correlation-id", "44820686-99f6-48ab-8995-4b0978ee0d52");
            request.AddHeader("Content-Type", "application/json");
            var body = @"{
" + "\n" +
            @"  ""paymentMethodName"": ""Jibit"",
" + "\n" +
            @"  ""items"": [
" + "\n" +
            @"    {
" + "\n" +
            @"      ""amount"": 100000,
" + "\n" +
            @"      ""iBanNumber"": ""IR110660000000205615803009"",
" + "\n" +
            @"      ""customerId"": ""66637e78-35d8-42e4-9a75-f75be16e6c48"",
" + "\n" +
            @"      ""description"":""test""
" + "\n" +
            @"    }
" + "\n" +
            @"  ]
" + "\n" +
            @"}";
            request.AddStringBody(body, DataFormat.Json);
            RestResponse response = await client.ExecuteAsync(request);
            ViewBag.ApiResponse = response.Content;
            return View("Test");
        }
        public class CashoutInputMOdel
        {
            public int amount { get; set; }
            public string iBanNumber { get; set; }
            public Guid customerId { get; set; }
            public string description { get; set; }
        }
        [HttpPost]
        public async Task<IActionResult> CashoutTestFormAsync(CashoutInputMOdel model)
        {

            var options = new RestClientOptions("https://localhost:59370")
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest("/api/v1/payment/cashout", Method.Post);
            request.AddHeader("x-user-id", "3b68bbad-a197-467b-8b8e-465fff2887b9");
            request.AddHeader("x-correlation-id", "44820686-99f6-48ab-8995-4b0978ee0d52");
            request.AddHeader("Content-Type", "application/json");
            var body = @"{
" + "\n" +
            @"  ""paymentMethodName"": ""Jibit"",
" + "\n" +
            @"  ""items"": [
" + "\n" +
            @"    {
" + "\n" +
            @"      ""amount"": 100000,
" + "\n" +
            @"      ""iBanNumber"": ""IR110660000000205615803009"",
" + "\n" +
            @"      ""customerId"": ""66637e78-35d8-42e4-9a75-f75be16e6c48"",
" + "\n" +
            @"      ""description"":""test""
" + "\n" +
            @"    }
" + "\n" +
            @"  ]
" + "\n" +
            @"}";
            request.AddStringBody(body, DataFormat.Json);
            RestResponse response = await client.ExecuteAsync(request);
            ViewBag.ApiResponse = response.Content;
            return View("Test");
        }

        public async Task<IActionResult> CashoutHistoryTestAsync()
        {
            var options = new RestClientOptions("https://localhost:59370")
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest("/api/v1/payment/cashOutHistory?paymentMethod=Jibit&page=1&size=100", Method.Get);
            request.AddHeader("x-user-id", "2fcfe61e-b4ad-4ab3-a387-64f5cefc8ded");
            request.AddHeader("x-correlation-id", "07a1f6e4-aa28-4929-b4a5-f48904997b3a");
            RestResponse response = await client.ExecuteAsync(request);
            Console.WriteLine(response.Content); ViewBag.ApiResponse = response.Content;
            return View("TestCashout");
        }

        [HttpGet]
        public IActionResult SubmitFormWithParameters()
        {
            return View(new PaymentRequestViewModel());
        }

        [HttpPost]
        public IActionResult TestWithParameters(PaymentRequestViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            string apiUrl = _configuration.GetValue<string>("PaymentAPI:Url");
            var client = new RestClient(apiUrl);
            var request = new RestRequest(apiUrl, Method.Post);
            request.AddHeader("x-api-version", model.ApiVersion);
            request.AddHeader("x-correlation-id", model.CorrelationId);
            request.AddHeader("x-user-id", model.UserId);
            request.AddParameter("application/json", JsonConvert.SerializeObject(model), ParameterType.RequestBody);
            RestResponse response = client.Execute(request);
            ViewBag.ApiResponse = response.Content;
            return View("TestWithParameters");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
