using System.Text.Json.Serialization;

namespace WebShop.DTO.Input
{
    public class PaymentMethodSubscribeIDTO
    {
        public string UserId { get; set; }
        public int PaymentMethodId { get; set; }
        public string Code { get; set; }
        public string Secret { get; set; }

        public PaymentMethodSubscribeIDTO(string userId, string code, string secret)
        {
            UserId = userId;
            Secret = secret;
            Code = code;
        }

        public PaymentMethodSubscribeIDTO()
        {
            UserId = string.Empty;
            Secret = string.Empty;
            Code = string.Empty;
        }
    }
}
