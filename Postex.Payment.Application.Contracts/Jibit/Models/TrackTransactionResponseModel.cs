using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Postex.Payment.Application.Contracts.Jibit.Models
{ 
    public class TrackTransactionResponseModel
    {
        public List<Transfer> transfers { get; set; }
        public string batchID { get; set; }
        public class Transfer
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
            public object bankSubmittedTime { get; set; }
            public DateTime createdAt { get; set; }
            public DateTime modifiedAt { get; set; }
            public string transferID { get; set; }
            public object notifyURL { get; set; }
            public object paymentID { get; set; }
            public string bankTransferID { get; set; }
        }

    }
}
