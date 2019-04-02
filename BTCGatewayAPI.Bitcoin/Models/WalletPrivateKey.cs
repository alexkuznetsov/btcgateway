using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTCGatewayAPI.Bitcoin.Models
{
    public class WalletPrivateKey
    {
        //[JsonProperty("privateKey", Order = 0)]
        public string PrivateKey { get; set; }

        //[JsonProperty("address", Order = 1)]
        public string Address { get; set; }
    }
}
