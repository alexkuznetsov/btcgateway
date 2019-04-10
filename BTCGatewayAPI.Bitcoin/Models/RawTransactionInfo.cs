using Newtonsoft.Json;

namespace BTCGatewayAPI.Bitcoin.Models
{
    public class RawTransactionInfo
    {
        [JsonProperty("txid")]
        public string Txid { get; set; }

        [JsonProperty("hash")]
        public string Hash { get; set; }

        [JsonProperty("version")]
        public int Version { get; set; }

        [JsonProperty("size")]
        public int Size { get; set; }

        [JsonProperty("vsize")]
        public int VSize { get; set; }

        [JsonProperty("weight")]
        public int Weight { get; set; }

        [JsonProperty("locktime")]
        public int Locktime { get; set; }

        [JsonProperty("vin")]
        public VIn[] VIn { get; set; }

        [JsonProperty("vout")]
        public VOut[] VOut { get; set; }
    }

    public class VOut
    {
        [JsonProperty("value")]
        public decimal Value { get; set; }

        [JsonProperty("n")]
        public int Num { get; set; }

        [JsonProperty("scriptPubKey")]
        public ScriptPubKey ScriptPubKey { get; set; }
    }

    public class ScriptPubKey
    {
        [JsonProperty("asm")]
        public string Asm { get; set; }

        [JsonProperty("hex")]
        public string Hex { get; set; }

        [JsonProperty("reqSigs")]
        public int ReqSigs { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("addresses")]
        public string[] Addresses { get; set; }
    }

    public class VIn
    {
        [JsonProperty("txid")]
        public string Txid { get; set; }

        [JsonProperty("vout")]
        public int VOut { get; set; }

        [JsonProperty("scriptSig")]
        public ScriptSig ScriptSig { get; set; }
    }

    public class ScriptSig
    {
        [JsonProperty("asm")]
        public string Asm { get; set; }

        [JsonProperty("hex")]
        public string Hex { get; set; }

        [JsonProperty("txinwitness")]
        public string[] Txinwitness { get; set; }

        [JsonProperty("sequence")]
        public long Sequence { get; set; }
    }
}
