using System.Runtime.Serialization;

namespace BTCGatewayAPI.Bitcoin.Models
{
    [DataContract]
    public class FundRawTransactionOptions
    {
        public const string ChangeTypeLegacy = "legacy";
        public const string ChangeTypeP2SHSegwit = "p2sh-segwit";
        public const string ChangeTypeBech32 = "bech32";

        public const string EstimateModeUnset = "UNSET";
        public const string EstimateModeEconomical = "ECONOMICAL";
        public const string EstimateModeConservative = "CONSERVATIVE";

        [DataMember(Name = "changeAddress", IsRequired = false, EmitDefaultValue = false)]
        public string ChangeAddress { get; set; }

        [DataMember(Name = "changePosition", IsRequired = false, EmitDefaultValue = false)]
        public int? ChangePosition { get; set; }

        [DataMember(Name = "change_type", IsRequired = false, EmitDefaultValue = false)]
        public string ChangeType { get; set; }

        [DataMember(Name = "includeWatching", IsRequired = false, EmitDefaultValue = false)]
        public bool? IncludeWatching { get; set; }

        [DataMember(Name = "lockUnspents", IsRequired = false, EmitDefaultValue = false)]
        public bool? LockUnspents { get; set; }

        [DataMember(Name = "subtractFeeFromOutputs", IsRequired = false, EmitDefaultValue = false)]
        public int[] SubtractFeeFromOutputs { get; set; }

        [DataMember(Name = "replaceable", IsRequired = false, EmitDefaultValue = false)]
        public bool? Replaceable { get; set; }

        [DataMember(Name = "conf_target", IsRequired = false, EmitDefaultValue = false)]
        public int? ConfTarget { get; set; }

        [DataMember(Name = "estimate_mode", IsRequired = false, EmitDefaultValue = false)]
        public string EstimateMode { get; set; }
    }
}
