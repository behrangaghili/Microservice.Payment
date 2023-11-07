using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Postex.Parcel.Domain
{
    public class PaymentWebhookResult
    {
        public bool IsSuccess { get; set; }
        public string ResCode { get; set; }
        public string CallbackUrl { get; set; }
        public string ErrorMessage { get; set; }
    }
}
