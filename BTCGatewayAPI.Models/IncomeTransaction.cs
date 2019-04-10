using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTCGatewayAPI.Models
{
    [Table("income_tx")]
    public class IncomeTransaction : Transaction
    {
        [Column("view_cnt")]
        public int ViewCount { get; set; }
    }
}