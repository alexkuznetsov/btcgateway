using BTCGatewayAPI.Models;
using BTCGatewayAPI.Models.DTO;
using Dapper;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace BTCGatewayAPI.Services.Extensions
{
    public static class IncomeTransactionDbContextExtensions
    {
        private const string GetNewIncomeTransactionsSQL = @"select i.id Id
  , i.created_at CreatedAt
  , i.updated_at UpdatedAt
  , i.wallet_id WalletId
  , i.tx_hash TxHash
  , i.address  Address
  , i.amount Amount
  , i.confirmation Confirmations
  , i.view_cnt ViewCount
  , i.tx_id Txid from income_tx i
left join income_tx_hist ih on i.id = ih.income_tx_id where ih.id is null or i.confirmation<@max_cnt

INSERT INTO income_tx_hist (income_tx_id) 
select i.id from income_tx i
left join income_tx_hist ih on i.id = ih.income_tx_id where ih.id is null";

        private const string FindReceiveTxIdByTxidAndAddressSQL = @"select 
    i.id Id
  , i.created_at CreatedAt
  , i.updated_at UpdatedAt
  , i.wallet_id WalletId
  , i.tx_hash TxHash
  , i.address  Address
  , i.amount Amount
  , i.confirmation Confirmations
  , i.view_cnt ViewCount
  , i.tx_id Txid 
from income_tx i 
where tx_id=@tx_hash and address=@address";

        private static readonly string UpdateReceiveTransactionSQL = @"UPDATE income_tx
   SET created_at = @CreatedAt
      ,updated_at = @UpdatedAt
      ,wallet_id = @WalletId
      ,tx_hash = @TxHash
      ,address = @Address
      ,amount = @Amount
      ,confirmation = @Confirmations
      ,view_cnt = @ViewCount
      ,tx_id = @Txid
 WHERE id = @Id";

        private static readonly string AddReceiveTransactionSQL = @"INSERT INTO income_tx
           ( created_at
           , updated_at
           , wallet_id
           , tx_hash
           , address
           , amount
           , confirmation
           , view_cnt
           , tx_id)
     VALUES
           (@CreatedAt
           ,@UpdatedAt
           ,@WalletId
           ,@TxHash
           ,@Address
           ,@Amount
           ,@Confirmations
           ,@ViewCount
           ,@Txid); SELECT CAST(SCOPE_IDENTITY() as int)";

        public static Task<IEnumerable<LastTransactionDTO>> GetNewIncomeTransactionsAsync(this DbConnection dbConetx, DbTransaction dbtx, int maxCnt)
            => dbConetx.QueryAsync<IncomeTransaction>(GetNewIncomeTransactionsSQL, new { max_cnt = maxCnt }, dbtx)
                .ContinueWith(t => t.Result.Select(x => new LastTransactionDTO
                {
                    Address = x.Address,
                    Amount = x.Amount,
                    Confirmation = x.Confirmations,
                    Date = x.CreatedAt
                }));

        public static Task<IncomeTransaction> FindReceiveTxIdByTxidAndAddressAsync(this DbConnection dbContext, DbTransaction dbtx, string txid, string address)
            => dbContext.QueryFirstOrDefaultAsync<IncomeTransaction>(FindReceiveTxIdByTxidAndAddressSQL,
                                new { tx_hash = txid, address }, dbtx);

        public static Task<int> UpdateReceiveTransactionAsync(this DbConnection dbConnection, DbTransaction tx, IncomeTransaction model)
            => dbConnection.ExecuteAsync(UpdateReceiveTransactionSQL, model, tx);

        public static Task<int> AddReceiveTransactionAsync(this DbConnection dbConnection, DbTransaction tx, IncomeTransaction model)
            => dbConnection.QueryFirstOrDefaultAsync<int>(AddReceiveTransactionSQL, model, tx);
    }
}