namespace WebShop.DTO.Output
{
    public class UserSubscriptionPlanODTO
    {
        public int UserSubscriptionPlanId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public SubscriptionPlanODTO? SubscriptionPlan { get; set; }
        public DateTime StartTimestamp { get; set; }
        public DateTime EndTimestamp { get; set; }
        public bool IsCanceled { get; set; }
    }
}
