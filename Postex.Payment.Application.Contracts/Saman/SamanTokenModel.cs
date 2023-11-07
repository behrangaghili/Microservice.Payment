namespace Postex.Payment.Application.Contracts.Saman
{
    public class SamanTokenModel
    {
        public string Action { get; set; }
        public string TerminalId { get; set; }
        public string RedirectUrl { get; set; }
        public string ResNum { get; set; }
        public long Amount { get; set; }
        public long CellNumber { get; set; }
    }
}

