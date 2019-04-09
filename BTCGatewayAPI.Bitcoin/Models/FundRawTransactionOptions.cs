using Newtonsoft.Json;

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
        [JsonProperty("changeAddress")]
        public string ChangeAddress { get; set; }

        [JsonProperty("changePosition")]
        public int? ChangePosition { get; set; }

        [JsonProperty("change_type")]
        public string ChangeType { get; set; }

        [JsonProperty("includeWatching")]
        public bool? IncludeWatching { get; set; }

        [JsonProperty("lockUnspents")]
        public bool? LockUnspents { get; set; }

        [JsonProperty("subtractFeeFromOutputs")]
        public int[] SubtractFeeFromOutputs { get; set; }

        [JsonProperty("replaceable")]
        public bool? Replaceable { get; set; }

        [JsonProperty("conf_target")]
        public int? ConfTarget { get; set; }

        [JsonProperty("estimate_mode")]
        public string EstimateMode { get; set; }
    }
}
