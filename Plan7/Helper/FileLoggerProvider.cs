using System.Text;

namespace Plan7.Helper;

/// <summary>
/// Логирование действий пользователей
/// </summary>
public class FileLoggerProvider : ILoggerProvider
{
    public ILogger CreateLogger(string categoryName) => new FileLogger();

    public void Dispose() { }

    public class FileLogger : ILogger, IDisposable
    {
        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => this;

        public void Dispose() { }

        public bool IsEnabled(LogLevel logLevel) => logLevel == LogLevel.Information;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (IsEnabled(logLevel))
            {
                StringBuilder sb = new($"{DateTime.Now.ToLongTimeString()}\n");
                sb.AppendLine(formatter(state, exception) + Environment.NewLine);
                // TODO: переписать по нормальному логгирование
                bool error = false;
                int i = 0;
                while (!error)
                {
                    try
                    {
                        using (StreamWriter sw = new($"../DB/LogUserInfo/{DateTime.Today.Day}-{DateTime.Today.Month}-{DateTime.Today.Year}-inform.txt", true))
                        {
                            sw.WriteLine(sb.ToString(), Encoding.UTF8);
                        }
                        error = true;
                    }
                    catch (Exception ex) { }
                    if (i == 5)
                        return;
                    i++;
                }
            }
            else
            {
                Console.WriteLine(formatter(state, exception));
            }
        }
    }
}
