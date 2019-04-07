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

        private static readonly Lazy<ILogger> LoggerLazy = new Lazy<ILogger>(LoggerFactory.GetLogger);
        private static ILogger Logger => LoggerLazy.Value;

        public HotWalletsService(DBContext dbContext, BitcoinClientFactory clientFactory) : base(dbContext)
        {
            this.clientFactory = clientFactory;
        }

        public Task<IEnumerable<HotWalletDTO>> GetAllWalletsAsync()
        {
            return DBContext.GetAllHotWalletDTOs();
        }

        public async Task SyncWalletsInformationAsync()
        {
            using (var tx = await DBContext.BeginTransaction(System.Data.IsolationLevel.Serializable))
            {
                try
                {
                    var allHotWallets = await DBContext.GetAllHotWallets();
                    var result = false;

                    foreach (var wallet in allHotWallets)
                    {
                        var bitcoinClient = clientFactory.Create(new Uri(wallet.RPCAddress), wallet.RPCUsername, wallet.RPCPassword);
                        var walletInfo = await bitcoinClient.GetWalletInfo();

                        if ((Math.Abs(wallet.Amount - walletInfo.Balance) > 0.00000001M) || (wallet.TxCount != walletInfo.TxCount))
                        {
                            wallet.Amount = walletInfo.Balance;
                            wallet.UpdatedAt = DateTime.Now;
                            wallet.TxCount = walletInfo.TxCount;

                            (result, _) = await TryToPerform(
                                action: () => DBContext.Update(wallet),
                                onError: (exc) => Logger.Error(exc, "Failed to update wallet informations, trying again."),
                                triesCount: 3);

                            if (!result)
                            {
                                Logger.Error("Can not update wallet with id:" + wallet.Id);
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