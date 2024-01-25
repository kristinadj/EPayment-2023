namespace WebShop.DTO.Output
{
    public class AuthenticationODTO
    {
        public string UserId { get; set; }
        public string Token { get; set; }
        public DateTime Expiration { get; set; }

        public AuthenticationODTO(string userId, string token)
        {
            UserId = userId;
            Token = token;
        }
    }
}
