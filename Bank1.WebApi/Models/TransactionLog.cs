﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Bank1.WebApi.Enums;

namespace Bank1.WebApi.Models
{
    [Table("TransactionLogs", Schema = "dbo")]
    public class TransactionLog
    {
        [Key]
        public int TransactionLogId { get; set; }
        public int TransactionId { get; set; }

        [Column(TypeName = "nvarchar(24)")]
        public TransactionStatus TransactionStatus { get; set; }
        public DateTime Timestamp { get; set; }

        public Transaction? Transaction { get; set; }
    }
}
