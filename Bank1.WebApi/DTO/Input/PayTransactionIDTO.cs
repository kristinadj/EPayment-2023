namespace Bank1.WebApi.DTO.Input
{
    public class PayTransactionIDTO
    {
        public int TransactionId { get; set; }
        public string CardHolderName { get; set; }
        public string PanNumber { get; set; }
        public string ExpiratoryDate { get; set; }
        public int CVV { get; set; }

        public PayTransactionIDTO(string cardHolderName, string panNumber, string expiratoryDate)
        {
            CardHolderName = cardHolderName;
            PanNumber = panNumber;
            ExpiratoryDate = expiratoryDate;
        }
    }
}
