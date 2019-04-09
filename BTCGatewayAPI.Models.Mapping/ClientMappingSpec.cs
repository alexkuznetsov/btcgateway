namespace BTCGatewayAPI.Models.Mapping
{
    public sealed class ClientMappingSpec : Infrastructure.DB.DbReaderMappingSpec<Models.Client>
    {
        protected override void MapProperties()
        {
            Map(x => x.ClientId, Transform.GetString);
            Map(x => x.CreatedAt, Transform.GetDateTime);
            Map(x => x.Id, Transform.GetInt);
            Map(x => x.Passwhash, Transform.GetString);
            Map(x => x.UpdatedAt, Transform.GetDateTimeOrNull);
        }
    }
}
