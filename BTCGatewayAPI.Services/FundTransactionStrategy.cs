using System.Threading.Tasks;
using BTCGatewayAPI.Models;
using BTCGatewayAPI.Models.Requests;

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

        protected readonly BitcoinClient bitcoinClient;
        protected readonly Infrastructure.GlobalConf conf;

        protected FundTransactionStrategy(BitcoinClient bitcoinClient, Infrastructure.GlobalConf conf)
        {
            this.bitcoinClient = bitcoinClient;
            this.conf = conf;
        }

        public abstract Task<FundTransactionStrategyResult> CreateAndSignTransactionAsync(HotWallet hotWallet, SendBtcRequest sendBtcRequest);
    }
}