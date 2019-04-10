using System.ComponentModel.DataAnnotations.Schema;

namespace BTCGatewayAPI.Models
{
    [Table("outcome_tx")]
    public class OutcomeTransaction : Transaction
    {
        public const string WithdrawState = "withdraw";

        public const string CompleteState = "complete";
        
        [Column("state")]
        public string State { get; set; }

        [Column("fee")]
        public decimal Fee { get; set; }
    }
}