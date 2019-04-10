using BTCGatewayAPI.Infrastructure.DB;
using BTCGatewayAPI.Models;
using BTCGatewayAPI.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BTCGatewayAPI.Services.Extensions
{
    public static class HotwalletDbContextExtensions
    {
        private const string FirstHWWithBalanceMoreThanSQL = @"select * from hot_wallets 
where amount>=@amount and rpc_address is not null and len(rpc_address)>1";

        public static async Task<HotWallet> GetFirstWithBalanceMoreThanAsync(this DBContext dbConetx, decimal balance)
        {
            var wallet = await dbConetx.FindAsync<HotWallet>(FirstHWWithBalanceMoreThanSQL,
                new KeyValuePair<string, object>("amount", balance));

            if (wallet != null)
            {
                return wallet;
            }

            throw new InvalidOperationException($"No any wallet with the cache balance more or equal {balance}");
        }

        private const string AllHotWalletsSQL = "select * from hot_wallets";

        public static async Task<IEnumerable<HotWallet>> GetAllHotWalletsAsync(this DBContext dbConetx)
            => await dbConetx.GetManyAsync<Models.HotWallet>(AllHotWalletsSQL).ConfigureAwait(false);

        public static async Task<IEnumerable<HotWalletDTO>> GetAllHotWalletDTOsAsync(this DBContext dbConetx)
        {
            var wallets = await dbConetx.GetAllHotWalletsAsync();

            return wallets.Select(x => new HotWalletDTO
            {
                Address = x.Address,
                Amount = x.Amount,
                CreatedAt = x.CreatedAt,
                Id = x.Id,
                UpdatedAt = x.UpdatedAt
            });
        }
    }
}