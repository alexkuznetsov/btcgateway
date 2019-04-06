using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTCGatewayAPI.Models
{

    public class IncomeTransactionId
    {
        [Column("id")]
        public int Id { get; set; }
    }

    [Table("income_tx")]
    public class IncomeTransaction : Transaction
    {
        [Column("sender")]
        public string Sender { get; set; }

        [Column("view_cnt")]
        public int ViewCount { get; set; }
    }
}