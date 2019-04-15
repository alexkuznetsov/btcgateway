using System.Runtime.Serialization;

namespace BTCGatewayAPI.Bitcoin.Models
{
    public static class ChangeType
    {
        public const string Legacy = "legacy";
        public const string P2SHSegwit = "p2sh-segwit";
        public const string Bech32 = "bech32";
    }

    public static class EstimateMode
    {
        public const string Unset = "UNSET";
        public const string Economical = "ECONOMICAL";
        public const string Conservative = "CONSERVATIVE";
    }

    public class FundRawTransactionOptions
    {
        [DataMember(Name = "changeAddress", IsRequired =false, EmitDefaultValue =false)]
        public string ChangeAddress { get; set; }

        [DataMember(Name = "changePosition", IsRequired =false, EmitDefaultValue =false)]
        public int? ChangePosition { get; set; }

        [DataMember(Name = "change_type", IsRequired =false, EmitDefaultValue =false)]
        public string ChangeType { get; set; }

        [DataMember(Name = "includeWatching", IsRequired =false, EmitDefaultValue =false)]
        public bool? IncludeWatching { get; set; }

        [DataMember(Name = "lockUnspents", IsRequired =false, EmitDefaultValue =false)]
        public bool? LockUnspents { get; set; }

        [DataMember(Name = "subtractFeeFromOutputs", IsRequired =false, EmitDefaultValue =false)]
        public int[] SubtractFeeFromOutputs { get; set; }

        [DataMember(Name = "replaceable", IsRequired =false, EmitDefaultValue =false)]
        public bool? Replaceable { get; set; }

        [DataMember(Name = "conf_target", IsRequired =false, EmitDefaultValue =false)]
        public int? ConfTarget { get; set; }

        [DataMember(Name = "estimate_mode", IsRequired =false, EmitDefaultValue =false)]
        public string EstimateMode { get; set; }
    }
}
