using Bank1.WebApi.DTO.NBS;
using Bank1.WebApi.Models;
namespace Bank1.WebApi.Helpers
{
    public static class Converter
    {
        public static QrCodeGenDTO ConvertToQrCodeGenIDTO(Transaction transaction, double amount, string currencyCode)
        {
            var qrCodeGenIDTO = new QrCodeGenDTO
            {
                K = "PR",
                V = "01",
                C = "1",
                R = transaction.ReceiverAccount!.AccountNumber,
                N = $"{transaction.ReceiverAccount!.Owner!.FirstName} {transaction.ReceiverAccount.Owner.LastName}\r\n{transaction.ReceiverAccount.Owner.Address}",
                I = $"{currencyCode}{amount:N2}",
                SF = "289"
            };

            return qrCodeGenIDTO;
        }
    }
}
