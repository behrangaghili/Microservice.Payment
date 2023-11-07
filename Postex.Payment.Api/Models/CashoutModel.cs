using Newtonsoft.Json;

namespace Postex.Payment.Api.Models
{
    public class CashoutModel
    {
        public string PaymentMethodName { get; set; }
        public List<CashoutModelItem> Items { get; set; }
    }
    public class CashoutModelItem
    {
        [JsonProperty("amount")]
        public int Amount { get; set; }
        [JsonProperty("iBanNumber")]
        public string IBanNumber { get; set; }
        [JsonProperty("customerId")]
        public Guid CustomerId { get; set; }
        public string Description { get; set; }
    }
}
