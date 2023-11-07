namespace Postex.Payment.Application.Contracts.Saman
{
    public class SamanTokenResultModel
    {
        public int status { get; set; }
        public int errorCode { get; set; }
        public string errorDesc { get; set; }
        public string token { get; set; }
    }
}
