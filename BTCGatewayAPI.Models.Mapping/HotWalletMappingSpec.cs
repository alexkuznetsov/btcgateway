using BTCGatewayAPI.Infrastructure.DB;

namespace BTCGatewayAPI.Models.Mapping
{
    public sealed class HotWalletMappingSpec : DbReaderMappingSpec<HotWallet>
    {
        protected override void MapProperties()
        {
            Map(x => x.Address, Transform.GetString/*, false*/);
            Map(x => x.RPCAddress, Transform.GetString);
            Map(x => x.RPCUsername, Transform.GetString);
            Map(x => x.RPCPassword, Transform.GetString);
            Map(x => x.Passphrase, Transform.GetString);
            Map(x => x.Amount, Transform.GetDecimal/*, false*/);
            Map(x => x.Id, Transform.GetInt/*, false*/);
            Map(x => x.CreatedAt, Transform.GetDateTime/*, false*/);
            Map(x => x.UpdatedAt, Transform.GetDateTimeOrNull);
            Map(x => x.TxCount, Transform.GetInt);
        }
    }
}