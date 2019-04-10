using BTCGatewayAPI.Infrastructure.DB;
using BTCGatewayAPI.Models;
using BTCGatewayAPI.Models.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BTCGatewayAPI.Services.Extensions
{
    public static class IncomeTransactionDbContextExtensions
    {
        static readonly string TXSql = @"select i.* from income_tx i
left join income_tx_hist ih on i.id = ih.income_tx_id where ih.id is null or i.confirmation<@max_cnt

INSERT INTO income_tx_hist (income_tx_id) 
select i.id from income_tx i
left join income_tx_hist ih on i.id = ih.income_tx_id where ih.id is null";

        public static async Task<LastTransactionDTO[]> GetNewIncomeTransactionsAsync(this DBContext dbConetx, int maxCnt)
        {
            var transactiins = await dbConetx.GetManyAsync<IncomeTransaction>(TXSql, new KeyValuePair<string, object>("max_cnt", maxCnt));

            return transactiins.Select(x => new LastTransactionDTO
            {
                Address = x.Address,
                Amount = x.Amount,
                Confirmation = x.Confirmations,
                Date = x.CreatedAt
            }).ToArray();
        }

        public static Task<Models.IncomeTransaction> FindReceiveTxIdByTxidAndAddressAsync(this DBContext dbContext, string txid, string address)
        {
            return dbContext.FindAsync<Models.IncomeTransaction>(
                "select * from income_tx where tx_id=@tx_hash and address=@address",
                                new KeyValuePair<string, object>("tx_hash", txid),
                                new KeyValuePair<string, object>("address", address));
        }
    }
}