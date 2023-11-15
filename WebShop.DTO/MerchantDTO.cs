namespace WebShop.DTO
{
    public class MerchantDTO
    {
        public int MerchantId { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string Email { get; set; }

        public MerchantDTO(string userId, string name, string address, string phoneNumber, string email)
        {
            UserId = userId;
            Name = name;
            Address = address;
            PhoneNumber = phoneNumber;
            Email = email;
        }
    }
}
