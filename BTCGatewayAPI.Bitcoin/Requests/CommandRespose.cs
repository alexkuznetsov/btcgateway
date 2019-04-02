using Newtonsoft.Json;

namespace BTCGatewayAPI.Bitcoin.Requests
{
    public class CommandResponse<T>
    {
        [JsonProperty("result", Order = 0)]
        public T Result { get; set; }

        [JsonProperty("error", Order = 1)]
        public CommandResponseError Error { get; set; }

        [JsonProperty("id", Order = 2)]
        public string Id { get; set; }
    }
}
