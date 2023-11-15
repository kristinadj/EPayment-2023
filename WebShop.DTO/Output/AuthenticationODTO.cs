namespace WebShop.DTO.Output
{
    public class AuthenticationODTO
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }

        public AuthenticationODTO(string token)
        {
            Token = token;
        }
    }
}
