namespace Base.DTO.Shared
{
    public class RedirectUrlDTO
    {
        public string RedirectUrl { get; set; }

        public RedirectUrlDTO(string redirectUrl)
        {
            RedirectUrl = redirectUrl;
        }
    }
}
