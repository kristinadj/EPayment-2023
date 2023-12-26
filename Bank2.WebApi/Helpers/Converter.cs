using Bank2.WebApi.Models;
using Base.DTO.NBS;
using System.Security.Cryptography;
using System.Text;

namespace Bank2.WebApi.Helpers
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
                R = transaction.ReceiverAccount!.AccountNumber.Replace("-", string.Empty),
                N = $"{transaction.ReceiverAccount!.Owner!.FirstName} {transaction.ReceiverAccount.Owner.LastName}\r\n{transaction.ReceiverAccount.Owner.Address}",
                I = $"{currencyCode}{amount:N2}".Replace(".", ","),
                SF = "189"
            };

            return qrCodeGenIDTO;
        }

        public static string ConvertToQrCodoeGenerateIDTO(Transaction transaction, double amount, string currencyCode)
        {
            return $"K:PR|V:01|C:1|R:{transaction.ReceiverAccount!.AccountNumber.Replace("-", string.Empty)}" +
                $"|N:{transaction.ReceiverAccount!.Owner!.FirstName} {transaction.ReceiverAccount.Owner.LastName}\r\n{transaction.ReceiverAccount.Owner.Address}" +
                $"|I:{currencyCode}{amount.ToString().Replace(".", ",")}|SF:189";
        }

        public static string HashPanNumber(string panNumber)
        {
            using var sha256Hash = SHA256.Create();
            byte[] hashedBytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(panNumber));

            var builder = new StringBuilder();
            for (int i = 0; i < hashedBytes.Length; i++)
            {
                builder.Append(hashedBytes[i].ToString("x2"));
            }

            return builder.ToString();
        }
    }
}
