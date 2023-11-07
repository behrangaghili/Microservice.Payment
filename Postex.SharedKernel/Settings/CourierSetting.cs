namespace Postex.SharedKernel.Settings
{
    public class CourierSetting
    {
        public CourierConfig Link { get; set; }
        public CourierConfig Taroff { get; set; }
        public CourierConfig Peykhub { get; set; }
        public CourierConfig Speed { get; set; }
        public CourierConfig Mahex { get; set; }
        public CourierConfig Bsw { get; set; }
        public CourierConfig Kbk { get; set; }
        public CourierConfig Chapar { get; set; }
        public CourierConfig Ubaar { get; set; }
        public CourierConfig Persia { get; set; }
        public CourierConfig Tinex { get; set; }
        public CourierConfig Badpa { get; set; }
        public CourierConfig PishroPost { get; set; }
        public CourierConfig Yarbox { get; set; }
        public CourierConfig Pde { get; set; }
        public CourierConfig Post { get; set; }
        public CourierConfig DigikalaPudo { get; set; }
        public CourierConfig EcoPeyk { get; set; }
    }

    public class CourierConfig
    {
        public int CourierId { get; set; }
        public string CourierName { get; set; }
        public string BaseUrl { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Scope { get; set; }
        public string GrantType { get; set; }
    }
}
