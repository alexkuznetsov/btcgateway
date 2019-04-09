using System.Threading.Tasks;
using BTCGatewayAPI.Models;
using BTCGatewayAPI.Models.Requests;

namespace BTCGatewayAPI.Services
{
    public abstract class FundTransactionStrategy
    {
        protected readonly BitcoinClient bitcoinClient;
        protected readonly Infrastructure.GlobalConf conf;

        protected FundTransactionStrategy(BitcoinClient bitcoinClient, Infrastructure.GlobalConf conf)
        {
            this.bitcoinClient = bitcoinClient;
            this.conf = conf;
        }

        public abstract Task<(string, decimal)> CreateAndSignTransactionAsync(HotWallet hotWallet, SendBtcRequest sendBtcRequest);
    }
}