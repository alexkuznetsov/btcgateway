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
        [JsonProperty("changeAddress", NullValueHandling = NullValueHandling.Ignore, Required = Required.AllowNull)]
        public string ChangeAddress { get; set; }

        [JsonProperty("changePosition", NullValueHandling = NullValueHandling.Ignore, Required = Required.AllowNull)]
        public int? ChangePosition { get; set; }

        [JsonProperty("change_type", NullValueHandling = NullValueHandling.Ignore, Required = Required.AllowNull)]
        public string ChangeType { get; set; }

        [JsonProperty("includeWatching", NullValueHandling = NullValueHandling.Ignore, Required = Required.AllowNull)]
        public bool? IncludeWatching { get; set; }

        [JsonProperty("lockUnspents", NullValueHandling = NullValueHandling.Ignore, Required = Required.AllowNull)]
        public bool? LockUnspents { get; set; }

        [JsonProperty("subtractFeeFromOutputs", NullValueHandling = NullValueHandling.Ignore, Required = Required.AllowNull)]
        public int[] SubtractFeeFromOutputs { get; set; }

        [JsonProperty("replaceable", NullValueHandling = NullValueHandling.Ignore, Required = Required.AllowNull)]
        public bool? Replaceable { get; set; }

        [JsonProperty("conf_target", NullValueHandling = NullValueHandling.Ignore, Required = Required.AllowNull)]
        public int? ConfTarget { get; set; }

        [JsonProperty("estimate_mode", NullValueHandling = NullValueHandling.Ignore, Required = Required.AllowNull)]
        public string EstimateMode { get; set; }
    }
}
