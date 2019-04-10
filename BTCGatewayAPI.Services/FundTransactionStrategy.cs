using BTCGatewayAPI.Infrastructure;
using BTCGatewayAPI.Models;
using BTCGatewayAPI.Models.Requests;
using System.Threading.Tasks;

namespace BTCGatewayAPI.Services
{
    public abstract class FundTransactionStrategy
    {
        public class FundTransactionStrategyResult
        {
            public FundTransactionStrategyResult(string hex, decimal fee, string txid)
            {
                Hex = hex;
                Fee = fee;
                Txid = txid;
            }

            public string Hex { get; set; }
            public string Txid { get; set; }
            public decimal Fee { get; set; }
        }

        protected BitcoinClient BitcoinClient { get; }

        protected GlobalConf Conf { get; }

        protected FundTransactionStrategy(BitcoinClient bitcoinClient, Infrastructure.GlobalConf conf)
        {
            BitcoinClient = bitcoinClient;
            Conf = conf;
        }

        public abstract Task<FundTransactionStrategyResult> CreateAndSignTransactionAsync(HotWallet hotWallet, SendBtcRequest sendBtcRequest);
    }
}