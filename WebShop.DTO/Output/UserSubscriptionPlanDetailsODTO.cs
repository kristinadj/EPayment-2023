namespace WebShop.DTO.Output
{
    public class UserSubscriptionPlanDetailsODTO
    {
        public bool IsValid { get; set; }
        public DateTime ActiveUntil { get; set; }
        public bool AutomaticRenewel { get; set; }
        public bool IsCanceled { get; set; }
    }
}
