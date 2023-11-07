using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Postex.Payment.Application.Contracts.Saman
{
    public class SamanPayModel
    {
        public string Amount { get; set; }

        public string MerchantId { get; set; }

        public string ResNum { get; set; }

        public string RedirectUrl { get; set; }
        public string Token { get; set; }
        public string MobileNo { get; set; }
    }
}
