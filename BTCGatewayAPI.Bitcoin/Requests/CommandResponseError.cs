﻿using Newtonsoft.Json;

namespace BTCGatewayAPI.Bitcoin.Requests
{
    public class CommandResponseError
    {
        [JsonProperty("code", Order = 0)]
        public int Code { get; set; }

        [JsonProperty("message", Order = 1)]
        public string Message { get; set; }
    }
}
