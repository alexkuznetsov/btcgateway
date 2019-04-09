using System;

namespace BTCGatewayAPI.Infrastructure.DB
{
    [Serializable]
    public class ObjectMapperException : Exception
    {
        public ObjectMapperException(string message) : base(message)
        {
        }
    }
}