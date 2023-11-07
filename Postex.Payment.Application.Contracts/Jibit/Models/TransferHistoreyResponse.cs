using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Postex.Payment.Application.Contracts.Jibit.Models
{
    public class Element
    {
        public string dashboardUniqueId { get; set; }
        public string transferMode { get; set; }
        public string destination { get; set; }
        public string destinationFirstName { get; set; }
        public string destinationLastName { get; set; }
        public int amount { get; set; }
        public string currency { get; set; }
        public string description { get; set; }
        public object metadata { get; set; }
        public bool cancellable { get; set; }
        public string state { get; set; }
        public object failReason { get; set; }
        public string feeCurrency { get; set; }
        public int feeAmount { get; set; }
        public string channelManagerProcessingType { get; set; }
        public DateTime bankSubmittedTime { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime modifiedAt { get; set; }
        public string transferID { get; set; }
        public object notifyURL { get; set; }
        public object paymentID { get; set; }
        public string bankTransferID { get; set; }
    }

    public class TransferHistoreyResponse
    {
        public int page { get; set; }
        public int size { get; set; }
        public List<Element> elements { get; set; }
    }
    public class TransferHistoryRequest
    {
        public int page { get; set; }
        public int size { get; set; }
        public DateTime? from { get; set; }
        public DateTime? to { get; set; }
        public string? state { get; set; }
        public string paymentMethod { get; set; }
    }

}
