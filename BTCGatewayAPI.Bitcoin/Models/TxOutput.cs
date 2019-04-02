using Newtonsoft.Json;

namespace BTCGatewayAPI.Bitcoin.Models
{
    public class TxOutput : TXInfo
    {
        //[JsonProperty("txid", Order = 0)]
        //public string Txid { get; set; }
        //[JsonProperty("vout", Order = 1)]
        //public int Vout { get; set; }
        [JsonProperty("scriptPubKey", Order = 2)]
        public string ScriptPubKey { get; set; }
        [JsonProperty("redeemScript", Order = 3)]
        public string RedeemScript { get; set; }
    }
}
