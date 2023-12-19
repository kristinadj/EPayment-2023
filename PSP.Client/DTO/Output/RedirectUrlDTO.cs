namespace PSP.Client.DTO.Output
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
