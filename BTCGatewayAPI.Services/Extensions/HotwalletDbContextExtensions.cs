using BTCGatewayAPI.Infrastructure.DB;
using BTCGatewayAPI.Models;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BTCGatewayAPI.Services.Extensions
{
    public static class HotwalletDbContextExtensions
    {
        public static async Task<HotWallet> GetFirstWithBalanceMoreThan(this DBContext dbConetx, decimal balance)
        {
            return await dbConetx.Find<HotWallet>("select * from [hot_wallets] where amount>=@amount",
                new KeyValuePair<string, object>("balance", balance))
                .ConfigureAwait(false);
        }
    }
}