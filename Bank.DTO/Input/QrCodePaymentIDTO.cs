namespace Bank.DTO.Input
{
    public class QrCodePaymentIDTO
    {
        public string PayerId { get; set; }
        public string ScannedQrCode { get; set; }

        public QrCodePaymentIDTO(string payerId, string scannedQrCode)
        {
            PayerId = payerId;
            ScannedQrCode = scannedQrCode;
        }
    }
}
