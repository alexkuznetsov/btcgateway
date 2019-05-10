using BTCGatewayAPI.Bitcoin;
using BTCGatewayAPI.Common;
using BTCGatewayAPI.Models;
using BTCGatewayAPI.Models.Requests;
using System.Threading.Tasks;

namespace BTCGatewayAPI.Services.Payments.FundStrategy
{
    internal abstract class FundTransactionStrategy
    {
        protected BitcoinClient BitcoinClient { get; }

        protected GlobalConf Conf { get; }

        protected FundTransactionStrategy(BitcoinClient bitcoinClient, Common.GlobalConf conf)
        {
            BitcoinClient = bitcoinClient;
            Conf = conf;
        }

        public abstract Task<FundTransactionStrategyResult> CreateAndSignTransactionAsync(HotWallet hotWallet, SendBtcRequest sendBtcRequest);
    }
}