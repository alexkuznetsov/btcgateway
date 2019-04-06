using System.ComponentModel.DataAnnotations.Schema;

namespace BTCGatewayAPI.Models
{
    public class OutcomeTransactionId
    {
        [Column("id")]
        public int Id { get; set; }
    }

    [Table("outcome_tx")]
    public class OutcomeTransaction : Transaction
    {
        public const string WithdrawState = "withdraw";

        public const string CompleteState = "complete";

        [Column("recipient")]
        public string Recipient { get; set; }

        [Column("state")]
        public string State { get; set; }

        [Column("fee")]
        public decimal Fee { get; set; }
    }
}