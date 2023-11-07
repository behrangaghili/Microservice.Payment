using Newtonsoft.Json;

namespace Postex.Payment.Application.Contracts.Jibit.Models
{
    public class TransferResponseOkModel
    {
        [JsonProperty("submittedCount")]
        public int SubmittedCount { get; set; }
        [JsonProperty("totalAmountTransferred")]
        public int TotalAmountTransferred { get; set; }
    }
    public class Error
    {
        public string code { get; set; }
        public string message { get; set; }
        public int httpStatusCode { get; set; }
    }

    public class TransferResponseErroModel
    {
        public string fingerprint { get; set; }
        public List<Error> errors { get; set; }
    }
  
}
