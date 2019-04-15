using BTCGatewayAPI.Bitcoin.Models;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BTCGatewayAPI.Bitcoin.Responses
{
    [DataContract]
    public class ListTransactionResponse : CommandResponse<List<Transaction>>
    {

    }
}
