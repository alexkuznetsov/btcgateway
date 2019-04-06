using BTCGatewayAPI.Infrastructure.DB;
using BTCGatewayAPI.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BTCGatewayAPI.Services.Extensions
{

    public static class HotwalletDbContextExtensions
    {
        public static async Task<HotWallet> GetFirstWithBalanceMoreThan(this DBContext dbConetx, decimal balance)
        {
            var wallet = await dbConetx.Find<HotWallet>(@"select * from [hot_wallets] 
where amount>=@amount and rpc_address is not null and len(rpc_address)>1",
                new KeyValuePair<string, object>("amount", balance));

            if (wallet != null)
            {
                return wallet;
            }

            throw new InvalidOperationException($"No any wallet with the cache balance more or equal {balance}");
        }

        public static async Task<IEnumerable<HotWallet>> GetAllHotWallets(this DBContext dbConetx)
        {
            return await dbConetx.GetMany<Models.HotWallet>("select * from [hot_wallets]")
                .ConfigureAwait(false);
        }
    }
}