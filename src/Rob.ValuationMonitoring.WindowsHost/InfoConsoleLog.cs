using EventFlow.Logs;

namespace Rob.ValuationMonitoring.WindowsHost
{
    public class InfoConsoleLog : ConsoleLog
    {
        protected override bool IsDebugEnabled => false;

        protected override bool IsVerboseEnabled => false;

        protected override void Write(LogLevel logLevel, string format, params object[] args)
        {
            if (logLevel < LogLevel.Information)
            {
                return;
            }

            base.Write(logLevel, format, args);
        }
    }
}