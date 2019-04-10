using BTCGatewayAPI.Infrastructure.DB;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BTCGatewayAPI.Services.Extensions
{
    public static class OutcomeTransactionDBContextExtensions
    {
        public static Task<Models.OutcomeTransaction> FindSendTransactionByTxidAndAddress(this DBContext dbContext, string txid, string address)
        {
            return dbContext.FindAsync<Models.OutcomeTransaction>(
                "select * from [outcome_tx] where tx_id=@tx_hash and address=@address",
                                new KeyValuePair<string, object>("tx_hash", txid),
                                new KeyValuePair<string, object>("address", address));
        }
    }
}