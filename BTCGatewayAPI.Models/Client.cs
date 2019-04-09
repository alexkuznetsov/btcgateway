using System.ComponentModel.DataAnnotations.Schema;

namespace BTCGatewayAPI.Models
{
    [Table("clients")]
    public class Client : Entity
    {
        [Column("client_id")]
        public string ClientId { get; set; }

        [Column("passwhash")]
        public string Passwhash { get; set; }
    }
}
