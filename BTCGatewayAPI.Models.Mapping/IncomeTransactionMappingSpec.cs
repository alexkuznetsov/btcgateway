using BTCGatewayAPI.Infrastructure.DB;

namespace BTCGatewayAPI.Models.Mapping
{
    public sealed class IncomeTransactionMappingSpec : DbReaderMappingSpec<IncomeTransaction>
    {
        protected override void MapProperties()
        {
            Map(x => x.Amount, Transform.GetDecimal/*, false*/);
            Map(x => x.ConfirmationCount, Transform.GetInt/*, false*/);
            Map(x => x.CreatedAt, Transform.GetDateTime/*, false*/);
            Map(x => x.Date, Transform.GetDateTime/*, false*/);
            Map(x => x.Id, Transform.GetInt/*, false*/);
            Map(x => x.Sender, Transform.GetString/*, false*/);
            Map(x => x.TxHash, Transform.GetString/*, false*/);
            Map(x => x.UpdatedAt, Transform.GetDateTimeOrNull);
            Map(x => x.WalletId, Transform.GetInt/*, false*/);
        }
    }
}