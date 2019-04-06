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
        static readonly string TXSql = @"update [income_tx] set view_cnt = view_cnt+1
SELECT id
      ,created_at
      ,updated_at
      ,wallet_id
      ,tx_hash
      ,sender
      ,amount
      ,confirmation
	  ,view_cnt
  FROM [income_tx]
  WHERE view_cnt-1<1 or [confirmation]<=@max_cnt";

        public static async Task<LastTransactionDTO[]> GetNewIncomeTransactions(this DBContext dbConetx, int maxCnt)
        {
            var transactiins = await dbConetx.GetMany<IncomeTransaction>(TXSql, new KeyValuePair<string, object>("max_cnt", maxCnt));

            return transactiins.Select(x => new LastTransactionDTO
            {
                Address = x.Sender,
                Amount = x.Amount,
                Confirmation = x.Confirmations,
                Date = x.CreatedAt
            }).ToArray();
        }

        public static async Task<Models.IncomeTransactionId> FindIncomeTxIdByTxid(this DBContext dbContext, string txid)
        {
            return await dbContext.Find<Models.IncomeTransactionId>("select id from [income_tx] where tx_hash=@tx_hash",
                                new KeyValuePair<string, object>("tx_hash", txid));
        }
    }
}