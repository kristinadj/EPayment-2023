using System.Text.Json.Serialization;

namespace WebShop.DTO.Input
{
    public class PaymentMethodSubscribeIDTO
    {
        public string UserId { get; set; }
        public int PaymentMethodId { get; set; }
        public string StrCode { get; set; }
        public int Code { get; set; }
        public string Secret { get; set; }

        public PaymentMethodSubscribeIDTO(string userId, string secret)
        {
            UserId = userId;
            Secret = secret;
            StrCode = Code.ToString();
        }

        public PaymentMethodSubscribeIDTO()
        {
            UserId = string.Empty;
            Secret = string.Empty;
            StrCode = string.Empty;
        }
    }
}
