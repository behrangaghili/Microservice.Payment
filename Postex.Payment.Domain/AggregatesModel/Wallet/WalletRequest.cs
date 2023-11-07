namespace Postex.Payment.Domain
{
    public class WalletRequest
    {
        public Guid UserId { get; set; }
        public decimal Amount { get; set; }
        public int TypeId { get; set; }
        public string Description { get; set; }
        public string SubWallet { get; set; }
        public string Tag { get; set; }
    }
}