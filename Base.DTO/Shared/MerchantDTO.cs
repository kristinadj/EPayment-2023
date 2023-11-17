using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.DTO.Shared
{
    public class MerchantDTO
    {
        public int MerchantId { get; set; }
        public int MerchantExternalId { get; set; }
        public string Name { get; set; }
        public string ServiceName { get; set; }
        public string TransactionSuccessUrl { get; set; }
        public string TransactionFailureUrl { get; set; }
        public string TransactionErrorUrl { get; set; }

        public MerchantDTO(string name, string serviceName, string transactionSuccessUrl, string transactionFailureUrl, string transactionErrorUrl)
        {
            Name = name;
            ServiceName = serviceName;
            TransactionSuccessUrl = transactionSuccessUrl;
            TransactionFailureUrl = transactionFailureUrl;
            TransactionErrorUrl = transactionErrorUrl;
        }
    }
}
