using System;
using System.Diagnostics;

namespace BTCGatewayAPI.Infrastructure.Logging
{
    public class LoggerFactory
    {
        private const string MSCorLib = "mscorlib.dll";

        /// <summary>
        /// Фабрика
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Logger GetLogger()
        {
            string sourceName = null;
            string sourceMethod = null;

            Type declaringType;
            int framesToSkip = 1;

            do
            {
                StackFrame frame = new StackFrame(framesToSkip, false);
                var method = frame.GetMethod();
                declaringType = method.DeclaringType;
                if (declaringType == null)
                {
                    sourceName = method.Name;
                    break;
                }

                framesToSkip++;
                sourceName = declaringType.FullName;
                sourceMethod = method.Name;
            } while (declaringType.Module.Name.Equals(MSCorLib, StringComparison.OrdinalIgnoreCase));

            var backend = LoggerBackendFactory.Create(sourceName, sourceMethod);

            return new Logger(backend);
        }
    }
}
