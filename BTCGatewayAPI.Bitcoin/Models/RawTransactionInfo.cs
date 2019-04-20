using System.Runtime.Serialization;

namespace BTCGatewayAPI.Bitcoin.Models
{
    [DataContract]
    public class RawTransactionInfo
    {
        [DataMember(Name = "txid")]
        public string Txid { get; set; }

        [DataMember(Name = "hash")]
        public string Hash { get; set; }

        [DataMember(Name = "version")]
        public int Version { get; set; }

        [DataMember(Name = "size")]
        public int Size { get; set; }

        [DataMember(Name = "vsize")]
        public int VSize { get; set; }

        [DataMember(Name = "weight")]
        public int Weight { get; set; }

        [DataMember(Name = "locktime")]
        public int Locktime { get; set; }

        [DataMember(Name = "vin")]
        public VIn[] VIn { get; set; }

        [DataMember(Name = "vout")]
        public VOut[] VOut { get; set; }
    }

    [DataContract]
    public class VOut
    {
        [DataMember(Name = "value")]
        public decimal Value { get; set; }

        [DataMember(Name = "n")]
        public int Num { get; set; }

        [DataMember(Name = "scriptPubKey")]
        public ScriptPubKey ScriptPubKey { get; set; }
    }

    [DataContract]
    public class ScriptPubKey
    {
        [DataMember(Name = "asm")]
        public string Asm { get; set; }

        [DataMember(Name = "hex")]
        public string Hex { get; set; }

        [DataMember(Name = "reqSigs")]
        public int ReqSigs { get; set; }

        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "addresses")]
        public string[] Addresses { get; set; }
    }

    [DataContract]
    public class VIn
    {
        [DataMember(Name = "txid")]
        public string Txid { get; set; }

        [DataMember(Name = "vout")]
        public int VOut { get; set; }

        [DataMember(Name = "scriptSig")]
        public ScriptSig ScriptSig { get; set; }
    }

    [DataContract]
    public class ScriptSig
    {
        [DataMember(Name = "asm")]
        public string Asm { get; set; }

        [DataMember(Name = "hex")]
        public string Hex { get; set; }

        [DataMember(Name = "txinwitness")]
        public string[] Txinwitness { get; set; }

        [DataMember(Name = "sequence")]
        public long Sequence { get; set; }
    }
}
