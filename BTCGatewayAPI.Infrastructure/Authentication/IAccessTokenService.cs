namespace BTCGatewayAPI.Common.Authentication
{
    public interface IAccessTokenService
    {
        System.Threading.Tasks.Task<bool> IsTokenAlive();
        System.Threading.Tasks.Task DeactivateCurrentAsync(string userId);
        System.Threading.Tasks.Task<bool> IsActiveAsync(string token);
        System.Threading.Tasks.Task DeactivateAsync(string userId, string token);
    }
}
