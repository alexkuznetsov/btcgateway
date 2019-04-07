using BTCGatewayAPI.Infrastructure.DB;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BTCGatewayAPI.Services.Extensions
{
    public static class OutcomeTransactionDBContextExtensions
    {
        public static Task<Models.OutcomeTransaction> FindOutputTxByTxid(this DBContext dbContext, string txid)
        {
            return dbContext.Find<Models.OutcomeTransaction>("select * from [outcome_tx] where tx_hash=@tx_hash",
                                new KeyValuePair<string, object>("tx_hash", txid));
        }
    }
}