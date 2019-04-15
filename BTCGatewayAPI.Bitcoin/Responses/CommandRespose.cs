using System.Runtime.Serialization;

namespace BTCGatewayAPI.Bitcoin.Responses
{
    public class CommandResponse<T>
    {
        [DataMember(Name = "result", Order = 0)]
        public T Result { get; set; }

        [DataMember(Name = "error", Order = 1)]
        public CommandResponseError Error { get; set; }

        [DataMember(Name = "id", Order = 2)]
        public string Id { get; set; }
    }
}
