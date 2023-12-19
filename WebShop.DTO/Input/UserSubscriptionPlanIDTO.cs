namespace WebShop.DTO.Input
{
    public class UserSubscriptionPlanIDTO
    {
        public string UserId { get; set; }
        public int SubscriptionPlanId { get; set; }

        public UserSubscriptionPlanIDTO(string userId)
        {
            UserId = userId;  
        }
    }
}
