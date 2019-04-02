using BTCGatewayAPI.Infrastructure.DB;
using System;

namespace BTCGatewayAPI.Services
{
    public abstract class BaseService : IDisposable
    {
        private bool _disposed;

        protected DBContext DBContext { get; }

        protected BaseService(DBContext dBContext)
        {
            DBContext = dBContext;
        }

        ~BaseService()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                DBContext.Dispose();
                _disposed = true;
            }
        }
    }
}