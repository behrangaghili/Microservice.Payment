using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Postex.Payment.Application.Contracts.Saman
{
    public class SamanVerifyInputModel
    {
        public string RefNum { get; set; }
        public string MerchantID { get; set; }
        public string TerminalNumber { get; set; }
        public string Action { get; set; }
    }
}
