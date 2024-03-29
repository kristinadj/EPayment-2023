﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebShop.WebApi.Models
{
    [Table("Merchants", Schema = "dbo")]
    public class Merchant
    {
        [Key]
        public int MerchantId { get; set; }

        public int? PspMerchantId { get; set; }
        public string UserId { get; set; }
        public bool IsMasterMerchant { get; set; }

        public User? User { get; set; }
        public ICollection<Item>? Items { get; set; }
        public ICollection<MerchantOrder>? MerchantOrders { get; set; }
        public ICollection<Invoice>? Invoices { get; set; }

        public Merchant(string userId)
        {
            UserId = userId;
        }
    }
}
