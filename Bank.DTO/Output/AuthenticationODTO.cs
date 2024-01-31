namespace Bank.DTO.Output
{
    public class AuthenticationODTO
    {
        public string CustomerId { get; set; }
        public string Token { get; set; }
        public DateTime Expiration { get; set; }

        public AuthenticationODTO(string customerId, string token)
        {
            CustomerId = customerId;
            Token = token;
        }
    }
}
