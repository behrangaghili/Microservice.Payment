namespace Client.Models
{
    public class PaymentRequestViewModel
    {

        public string ApiVersion { get; set; }
        public Guid CorrelationId { get; set; }
        public Guid UserId { get; set; }
        public Guid PayerId { get; set; }

        public string OrderNumber { get; set; }
        public string Description { get; set; }
        public string Amount { get; set; }
        public string ReturnUrl { get; set; }
        public string CancelUrl { get; set; }
        public string AppName { get; set; }
        public string Remark { get; set; }
    }
}
