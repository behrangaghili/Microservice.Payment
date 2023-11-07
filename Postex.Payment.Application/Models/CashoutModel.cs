using Newtonsoft.Json;

namespace Postex.Payment.Application
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
        public int IBanNumber { get; set; }
        [JsonProperty("customerId")]
        public string CustomerId { get; set; }
    }
}
