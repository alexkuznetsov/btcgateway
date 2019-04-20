using BTCGatewayAPI.Bitcoin;
using BTCGatewayAPI.Infrastructure.Logging;
using BTCGatewayAPI.Models.DTO;
using BTCGatewayAPI.Services.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace BTCGatewayAPI.Services
{
    public sealed class HotWalletsService : BaseService
    {
        private readonly BitcoinClientFactory _clientFactory;
        private readonly Infrastructure.GlobalConf _conf;

        private static readonly Lazy<ILogger> LoggerLazy = new Lazy<ILogger>(LoggerFactory.GetLogger);
        private static readonly decimal Delta = 0.00000001M;

        private static ILogger Logger => LoggerLazy.Value;

        public HotWalletsService(DbConnection dbContext, BitcoinClientFactory clientFactory, Infrastructure.GlobalConf conf) : base(dbContext)
        {
            _clientFactory = clientFactory;
            _conf = conf;
        }

        public async Task<IEnumerable<HotWalletDTO>> GetAllWalletsAsync()
        {
            if (DbCon.State != System.Data.ConnectionState.Open)
                await DbCon.OpenAsync();

            return await DbCon.GetAllHotWalletDTOsAsync();
        }

        public async Task SyncWalletsInformationAsync()
        {
            if (DbCon.State != System.Data.ConnectionState.Open)
                await DbCon.OpenAsync();

            var allHotWallets = await DbCon.GetAllHotWalletsAsync();
            var tasks = new List<Task>();

            foreach (var wallet in allHotWallets)
            {
                tasks.Add(ProcessHotWallet(wallet));
            }

            await Task.WhenAll(tasks);
        }

        private async Task ProcessHotWallet(Models.HotWallet wallet)
        {
            var result = false;
            var bitcoinClient = _clientFactory.Create(new Uri(wallet.RPCAddress), wallet.RPCUsername, wallet.RPCPassword);
            var walletInfo = await bitcoinClient.GetWalletInfoAsync();

            if ((Math.Abs(wallet.Amount - walletInfo.Balance) > Delta) || (wallet.TxCount != walletInfo.TxCount))
            {
                wallet.Amount = walletInfo.Balance;
                wallet.UpdatedAt = DateTime.Now;
                wallet.TxCount = walletInfo.TxCount;

                using (var tx = DbCon.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        (result, _) = await TryToPerformAsync(
                            action: () => DbCon.UpdateWalletAsync(tx, wallet),
                            onError: (exc) => Logger.Error(exc, Messages.ErrUpdateWalletInfo),
                            triesCount: _conf.RetryActionCnt);
                        tx.Commit();
                    }
                    catch (Exception)
                    {
                        tx.Rollback();
                        throw;
                    }
                    if (!result)
                    {
                        Logger.Error(Messages.ErrUpdateWallet, wallet.Id);
                    }
                }
            }
        }
    }
}