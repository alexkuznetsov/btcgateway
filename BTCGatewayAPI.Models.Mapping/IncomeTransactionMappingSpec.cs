using BTCGatewayAPI.Infrastructure.DB;

namespace BTCGatewayAPI.Models.Mapping
{
    public sealed class IncomeTransactionMappingSpec : DbReaderMappingSpec<IncomeTransaction>
    {
        protected override void MapProperties()
        {
            Map(x => x.Address, Transform.GetString/*, false*/);
            Map(x => x.Amount, Transform.GetDecimal/*, false*/);
            Map(x => x.Confirmations, Transform.GetInt/*, false*/);
            Map(x => x.CreatedAt, Transform.GetDateTime/*, false*/);
            Map(x => x.Id, Transform.GetInt/*, false*/);
            Map(x => x.Txid, Transform.GetString/*, false*/);
            Map(x => x.TxHash, Transform.GetString/*, false*/);
            Map(x => x.UpdatedAt, Transform.GetDateTimeOrNull);
            Map(x => x.WalletId, Transform.GetInt/*, false*/);
            Map(x => x.ViewCount, Transform.GetInt);
        }
    }
}