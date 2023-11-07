namespace Postex.Payment.Domain
{
    public class WalletResponse
    {
        public decimal Amount { get; set; }
        public int TypeId { get; set; }
        public string UserId { get; set; }
        public string Tag { get; set; }
        public string ReferenceNo { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}