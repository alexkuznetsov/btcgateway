using BTCGatewayAPI.Models;
using Dapper;
using System.Data.Common;
using System.Threading.Tasks;

namespace BTCGatewayAPI.Services.Extensions
{
    public static class OutcomeTransactionDBContextExtensions
    {
        private const string FindSendTransactionByTxidAndAddressSQL = @"select 
    o.id as Id
  , o.created_at as CreatedAt
  , o.updated_at as UpdatedAt
  , o.wallet_id as WalletId
  , o.tx_hash as TxHash
  , o.[address] as [Address]
  , o.amount as Amount
  , o.state as State
  , o.fee as Fee
  , o.confirmation as Confirmations
  , o.tx_id as Txid
  
   from outcome_tx o where tx_id=@tx_hash and address=@address";

        private const string UpdateSendTransactionSQL = @"UPDATE [dbo].[outcome_tx]
   SET [created_at] = @CreatedAt
      ,[updated_at] = @UpdatedAt
      ,[wallet_id] = @WalletId
      ,[tx_hash] = @TxHash
      ,[address] = @Address
      ,[amount] = @Amount
      ,[state] = @State
      ,[fee] = @Fee
      ,[confirmation] = @Confirmations
      ,[tx_id] = @Txid
 WHERE id = @Id";

        private const string AddSendTransactionSQL = @"INSERT INTO [dbo].[outcome_tx]
           ([created_at]
           ,[updated_at]
           ,[wallet_id]
           ,[tx_hash]
           ,[address]
           ,[amount]
           ,[state]
           ,[fee]
           ,[confirmation]
           ,[tx_id])
     VALUES
           (@CreatedAt
           ,@UpdatedAt
           ,@WalletId
           ,@TxHash
           ,@Address
           ,@Amount
           ,@State
           ,@Fee
           ,@Confirmations
           ,@Txid); SELECT CAST(SCOPE_IDENTITY() as int)";

        private const string DeleteSendTransactionSQL = @"DELETE FROM [dbo].[outcome_tx] WHERE id = @Id";

        public static Task<OutcomeTransaction> FindSendTransactionByTxidAndAddressAsync(this DbConnection dbContext, DbTransaction dbtx, string txid, string address)
            => dbContext.QueryFirstOrDefaultAsync<OutcomeTransaction>(FindSendTransactionByTxidAndAddressSQL,
                                new { tx_hash = txid, address }, dbtx);

        public static Task<int> UpdateSendTransactionAsync(this DbConnection dbConnection, DbTransaction tx, OutcomeTransaction model)
            => dbConnection.ExecuteAsync(UpdateSendTransactionSQL, model, tx);

        public static Task<int> DeleteSendTransactionAsync(this DbConnection dbConnection, DbTransaction tx, OutcomeTransaction model)
           => dbConnection.ExecuteAsync(DeleteSendTransactionSQL, new { model.Id }, tx);

        public static Task<int> AddSendTransactionAsync(this DbConnection dbConnection, DbTransaction tx, OutcomeTransaction model)
            => dbConnection.QueryFirstOrDefaultAsync<int>(AddSendTransactionSQL, model, tx);
    }
}