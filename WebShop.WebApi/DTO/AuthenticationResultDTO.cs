namespace WebShop.WebApi.DTO
{
    public class AuthenticationResultDTO
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }

        public AuthenticationResultDTO(string token)
        {
            Token = token;
        }
    }
}
