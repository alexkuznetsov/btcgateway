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
        static readonly string TXSql = @"select i.* from [dbo].[income_tx] i
left join [dbo].[income_tx_hist] ih on i.id = ih.income_tx_id where ih.id is null or i.confirmation<@max_cnt

INSERT INTO [dbo].[income_tx_hist] ([income_tx_id]) 
select i.id
from [dbo].[income_tx] i
left join [dbo].[income_tx_hist] ih on i.id = ih.income_tx_id where ih.id is null";

        public static async Task<LastTransactionDTO[]> GetNewIncomeTransactionsAsync(this DBContext dbConetx, int maxCnt)
        {
            var transactiins = await dbConetx.GetManyAsync<IncomeTransaction>(TXSql, new KeyValuePair<string, object>("max_cnt", maxCnt));

            return transactiins.Select(x => new LastTransactionDTO
            {
                Address = x.Sender,
                Amount = x.Amount,
                Confirmation = x.Confirmations,
                Date = x.CreatedAt
            }).ToArray();
        }

        public static Task<Models.IncomeTransactionId> FindIncomeTxIdByTxidAsync(this DBContext dbContext, string txid)
        {
            return dbContext.FindAsync<Models.IncomeTransactionId>("select id from [income_tx] where tx_hash=@tx_hash",
                                new KeyValuePair<string, object>("tx_hash", txid));
        }
    }
}