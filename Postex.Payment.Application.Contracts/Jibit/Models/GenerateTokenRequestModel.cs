namespace Postex.Payment.Application.Contracts.Jibit.Models
{
    public class GenerateTokenRequestModel
    {
        [Newtonsoft.Json.JsonProperty("apiKey")]
        public string ApiKey { get; set; }
        [Newtonsoft.Json.JsonProperty("secretKey")]
        public string SecretKey { get; set; }
    }
}
