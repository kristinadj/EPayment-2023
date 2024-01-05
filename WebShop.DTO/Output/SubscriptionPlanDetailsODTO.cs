namespace WebShop.DTO.Output
{
    public class SubscriptionPlanDetailsODTO
    {
        public bool IsValid { get; set; }
        public DateTime ActiveUntil { get; set; }
        public bool AutomaticRenewel { get; set; }
    }
}
