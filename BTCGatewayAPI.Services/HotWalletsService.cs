using BTCGatewayAPI.Infrastructure.Logging;
using BTCGatewayAPI.Models.DTO;
using BTCGatewayAPI.Services.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace BTCGatewayAPI.Services
{
    public class HotWalletsService : BaseService
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

            using (var tx = DbCon.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
            {
                try
                {
                    var allHotWallets = await DbCon.GetAllHotWalletsAsync(tx);
                    var result = false;

                    foreach (var wallet in allHotWallets)
                    {
                        var bitcoinClient = _clientFactory.Create(new Uri(wallet.RPCAddress), wallet.RPCUsername, wallet.RPCPassword);
                        var walletInfo = await bitcoinClient.GetWalletInfoAsync();

                        if ((Math.Abs(wallet.Amount - walletInfo.Balance) > Delta) || (wallet.TxCount != walletInfo.TxCount))
                        {
                            wallet.Amount = walletInfo.Balance;
                            wallet.UpdatedAt = DateTime.Now;
                            wallet.TxCount = walletInfo.TxCount;

                            (result, _) = await TryToPerformAsync(
                                action: () => DbCon.UpdateWalletAsync(tx, wallet),
                                onError: (exc) => Logger.Error(exc, Messages.ErrUpdateWalletInfo),
                                triesCount: _conf.RetryActionCnt);

                            if (!result)
                            {
                                Logger.Error(Messages.ErrUpdateWallet, wallet.Id);
                            }
                        }
                    }

                    tx.Commit();
                }
                catch (Exception)
                {
                    tx.Rollback();
                    throw;
                }
            }
        }
    }
}