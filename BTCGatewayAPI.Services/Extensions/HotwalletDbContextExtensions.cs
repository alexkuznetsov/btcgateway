using BTCGatewayAPI.Models;
using BTCGatewayAPI.Models.DTO;
using Dapper;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace BTCGatewayAPI.Services.Extensions
{
    public static class HotwalletDbContextExtensions
    {
        private const string FirstHWWithBalanceMoreThanSQL = @"select id Id
  , created_at CreatedAt
  , updated_at UpdatedAt
  , address Address
  , amount Amount
  , rpc_address RPCAddress
  , rpc_username RPCUsername
  , rpc_password RPCPassword
  , passphrase Passphrase
  , tx_count TxCount from hot_wallets 
where amount>=@amount and rpc_address is not null and len(rpc_address)>1";

        private const string AllHotWalletsSQL = @"select id Id
  , created_at CreatedAt
  , updated_at UpdatedAt
  , address Address
  , amount Amount
  , rpc_address RPCAddress
  , rpc_username RPCUsername
  , rpc_password RPCPassword
  , passphrase Passphrase
  , tx_count TxCount from hot_wallets";

        private const string UpdateWalletSQL = @"UPDATE hot_wallets
   SET [created_at] = @CreatedAt
      ,[updated_at] = @UpdatedAt
      ,[address] = @Address
      ,[amount] = @Amount
      ,[rpc_address] = @RPCAddress
      ,[rpc_username] = @RPCUsername
      ,[rpc_password] = @RPCPassword
      ,[passphrase] = @Passphrase
      ,[tx_count] = @TxCount
 WHERE id = @Id";

        public static Task<HotWallet> GetFirstWithBalanceMoreThanAsync(this DbConnection dBContext, decimal balance)
            => dBContext.QueryFirstAsync<HotWallet>(FirstHWWithBalanceMoreThanSQL, new { amount = balance });

        public static Task<IEnumerable<HotWallet>> GetAllHotWalletsAsync(this DbConnection dbConetx, DbTransaction dbtx = null)
            => dbConetx.QueryAsync<HotWallet>(AllHotWalletsSQL, transaction: dbtx);

        public static Task<IEnumerable<HotWalletDTO>> GetAllHotWalletDTOsAsync(this DbConnection dbConetx)
            => dbConetx.GetAllHotWalletsAsync()
                .ContinueWith((t) => t.Result.Select(x => new HotWalletDTO
                {
                    Address = x.Address,
                    Amount = x.Amount,
                    CreatedAt = x.CreatedAt,
                    Id = x.Id,
                    UpdatedAt = x.UpdatedAt
                }));

        public static Task<int> UpdateWalletAsync(this DbConnection dbConnection, DbTransaction transaction, HotWallet wallet)
            => dbConnection.ExecuteAsync(UpdateWalletSQL, wallet, transaction);
    }
}