using BTCGatewayAPI.Infrastructure.DB;
using System;
using System.Threading.Tasks;

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

        protected async Task<bool> TryToPerform(Func<Task> action, Action<Exception> onError, int triesCount)
        {
            int tryNumber = 0;
            bool isSuccess = false;

            while (tryNumber < triesCount - 1)
            {
                try
                {
                    await action();
                    isSuccess = true;
                    break;
                }
                catch (Exception ex)
                {
                    onError?.Invoke(ex);
                    tryNumber++;
                }
            }

            return isSuccess;
        }

        protected async Task<(bool, TResult)> TryToPerform<TResult>(Func<Task<TResult>> action, Action<Exception> onError, int triesCount)
        {
            int tryNumber = 0;
            bool isSuccess = false;
            TResult result = default(TResult);

            while (tryNumber < triesCount - 1)
            {
                try
                {
                    result = await action();
                    isSuccess = true;
                    break;
                }
                catch (Exception ex)
                {
                    onError?.Invoke(ex);
                    tryNumber++;
                }
            }

            return (isSuccess, result);
        }
    }
}