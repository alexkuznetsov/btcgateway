using System;

namespace BTCGatewayAPI.Services
{
    internal static class TimeUtils
    {
        public static DateTime FromUnixTime(double unixTimeStamp)
        {
            return (new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc))
                .AddSeconds(unixTimeStamp).ToLocalTime();
        }
    }
}
