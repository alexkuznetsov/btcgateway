using BTCGatewayAPI.Bitcoin.Models;
using BTCGatewayAPI.Infrastructure;
using BTCGatewayAPI.Models;
using BTCGatewayAPI.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BTCGatewayAPI.Services
{
    public class ManualFundTransactionStrategy : FundTransactionStrategy
    {
        public ManualFundTransactionStrategy(BitcoinClient bitcoinClient, GlobalConf conf) : base(bitcoinClient, conf)
        {
        }

        public override async Task<FundTransactionStrategyResult> CreateAndSignTransactionAsync(HotWallet hotWallet, SendBtcRequest sendBtcRequest)
        {
            string address = hotWallet.Address;
            var privateKey = await bitcoinClient.LoadWalletPrivateKeysAsync(address
                , hotWallet.Passphrase, conf.WalletUnlockTime);
            var fee = await bitcoinClient.LoadEstimateSmartFeeAsync();
            var unspent = await GetUnspentTransactionOutputsAsync(address, sendBtcRequest.Amount + fee.Feerate);
            var parameters = CreateInputsAndOutputs(fee.Feerate, hotWallet.Address, unspent, sendBtcRequest);
            var rawTx = await bitcoinClient.CreateRawtransactionAsync(parameters.Item1, parameters.Item2);
            var signed = await bitcoinClient.SignRawTransactionWithKeyAsync(new Bitcoin.Models.Unspent[] { }
                , new string[] { privateKey }
                , rawTx);
            var txInfo = await bitcoinClient.DecodeRawTransaction(signed.Hex);

            return new FundTransactionStrategyResult(signed.Hex, fee.Feerate, txInfo.Txid);
        }

        public async Task<Unspent[]> GetUnspentTransactionOutputsAsync(string address, decimal minimalFunds)
        {
            //По хорошему, тут можно применить алгоритм "заполнения рюкзака", 
            //но я решил воспользоваться другоим методом заполнения транзации
            var unspentForWallet = await bitcoinClient.ListUnspentAsync(address);
            var lessers = unspentForWallet.Where(x => x.Amount < minimalFunds);
            var greaters = unspentForWallet.Where(x => x.Amount >= minimalFunds);

            if (greaters.Any())
            {
                return new[]
                {
                    greaters.OrderBy(x => x.Amount).First()
                };
            }

            var result = new List<Unspent>();
            var sum = 0M;

            foreach (var u in lessers.OrderByDescending(x => x.Amount))
            {
                result.Add(u);
                sum += u.Amount;

                if (sum >= minimalFunds)
                    break;
            }

            return result.ToArray();
        }

        private (TXInfo[], Dictionary<string, decimal>) CreateInputsAndOutputs(decimal feerate, string address, Bitcoin.Models.Unspent[] unspentForWallet, SendBtcRequest sendBtcRequest)
        {
            var totalUnspentSum = unspentForWallet.Sum(x => x.Amount);
            var spent = feerate;//Т.е. мы уже потратили, это комиссия на транзацкию

            if (totalUnspentSum < (sendBtcRequest.Amount + spent))
                throw new InvalidOperationException(Messages.ErrTryToWithdrawMoreThanExcists);

            var tx = new List<TXInfo>();
            var reciviers = new Dictionary<string, decimal>();
            var change = 0M;

            foreach (var u in unspentForWallet)
            {
                tx.Add(new TXInfo { Txid = u.Txid, Vout = u.Vout });
                spent += u.Amount;

                if (spent >= sendBtcRequest.Amount)
                {
                    change = spent - sendBtcRequest.Amount;
                    break;
                }
            }

            reciviers.Add(sendBtcRequest.Account, sendBtcRequest.Amount);

            if (change > 0M)
            {
                reciviers.Add(address, change);
            }

            return (tx.ToArray(), reciviers);
        }
    }
}