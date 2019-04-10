using BTCGatewayAPI.Infrastructure.DB;
using BTCGatewayAPI.Infrastructure.Logging;
using BTCGatewayAPI.Models.DTO;
using BTCGatewayAPI.Services.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BTCGatewayAPI.Services
{
    public class HotWalletsService : BaseService
    {
        private readonly BitcoinClientFactory clientFactory;
        private readonly Infrastructure.GlobalConf conf;

        private static readonly Lazy<ILogger> LoggerLazy = new Lazy<ILogger>(LoggerFactory.GetLogger);
        private static readonly decimal Delta = 0.00000001M;

        private static ILogger Logger => LoggerLazy.Value;

        public HotWalletsService(DBContext dbContext, BitcoinClientFactory clientFactory, Infrastructure.GlobalConf conf) : base(dbContext)
        {
            this.clientFactory = clientFactory;
            this.conf = conf;
        }

        public Task<IEnumerable<HotWalletDTO>> GetAllWalletsAsync()
        {
            return DBContext.GetAllHotWalletDTOsAsync();
        }

        public async Task SyncWalletsInformationAsync()
        {
            using (var tx = await DBContext.BeginTransactionAsync(System.Data.IsolationLevel.Serializable))
            {
                try
                {
                    var allHotWallets = await DBContext.GetAllHotWalletsAsync();
                    var result = false;

                    foreach (var wallet in allHotWallets)
                    {
                        var bitcoinClient = clientFactory.Create(new Uri(wallet.RPCAddress), wallet.RPCUsername, wallet.RPCPassword);
                        var walletInfo = await bitcoinClient.GetWalletInfoAsync();

                        if ((Math.Abs(wallet.Amount - walletInfo.Balance) > Delta) || (wallet.TxCount != walletInfo.TxCount))
                        {
                            wallet.Amount = walletInfo.Balance;
                            wallet.UpdatedAt = DateTime.Now;
                            wallet.TxCount = walletInfo.TxCount;

                            (result, _) = await TryToPerformAsync(
                                action: () => DBContext.UpdateAsync(wallet),
                                onError: (exc) => Logger.Error(exc, Messages.ErrUpdateWalletInfo),
                                triesCount: conf.RetryActionCnt);

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