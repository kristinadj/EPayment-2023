namespace Bank1.WebApi.AppSettings
{
    public class BankSettings
    {
        public string BankPaymentUrl { get; set; }
        public string CardStartNumbers { get; set; }
        public int PccBankId { get; set; }
        public string PccUrl { get; set; }
        public BankSettings()
        {
            BankPaymentUrl = string.Empty;
            CardStartNumbers = string.Empty;
            PccUrl = string.Empty;
        }
    }
}
