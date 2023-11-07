using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Postex.Payment.Application.Contracts.Saman
{
    public class SamanVerifyModel
    {
        public SamanVerifyInfo VerifyInfo { get; set; }
        public int ResultCode { get; set; }
        public string ResultDescription { get; set; }
        public bool Success { get; set; }
        public class SamanVerifyInfo
        {
            public string RRN { get; set; }
            public string RefNum { get; set; }
            public string MaskedPan { get; set; }
            public string HashedPan { get; set; }
            public long TerminalNumber { get; set; }
            public long OrginalAmount { get; set; }
            public string AffectiveAmount { get; set; }
            public string StraceDate { get; set; }
            public string StraceNo { get; set; }
        }
    }
}
