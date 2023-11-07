using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Postex.Payment.Application.Contracts.Jibit.Models
{
    public class TransferRequestModel
    {
        [JsonProperty("batchID")]
        public string BatchID { get; set; }

        [JsonProperty("submissionMode")]
        public string SubmissionMode { get; set; }

        [JsonProperty("transfers")]
        public List<Transfer> Transfers { get; set; }
    }

    public class Transfer
    {
        [JsonProperty("transferID")]
        public string TransferID { get; set; }

        [JsonProperty("transferMode")]
        public string TransferMode { get; set; }

        [JsonProperty("destination")]
        public string Destination { get; set; }

        [JsonProperty("destinationFirstName")]
        public object DestinationFirstName { get; set; }

        [JsonProperty("destinationLastName")]
        public object DestinationLastName { get; set; }

        [JsonProperty("amount")]
        public int Amount { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("metadata")]
        public object Metadata { get; set; }

        [JsonProperty("notifyURL")]
        public object NotifyURL { get; set; }

        [JsonProperty("cancellable")]
        public bool Cancellable { get; set; }

        [JsonProperty("paymentID")]
        public object PaymentID { get; set; }
    }




}
