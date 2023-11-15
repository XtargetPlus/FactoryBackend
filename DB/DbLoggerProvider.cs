using Microsoft.Extensions.Logging;
using System.Text;

namespace DB;

public class DbLoggerProvider : ILoggerProvider
{
    public ILogger CreateLogger(string categoryName)
    {
        return new DbLogger();
    }

    public void Dispose() { }

    private class DbLogger : ILogger
    {
        
        public IDisposable BeginScope<TState>(TState state)
            where TState : notnull => default!;

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId,
                TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if(LogLevel.Information == logLevel)
            {
                StringBuilder sb = new($"{DateTime.Now.ToLongTimeString()}\n");
                if (state != null)
                {
                    sb.Append($"{state}\n");
                }
                if (exception != null)
                {
                    sb.Append($"{exception.Message}\n");
                }
                bool error = false;
                int i = 0;

                // Временный костыль (нужно переписать)
                // TODO: переписать логгирование
                while (!error)
                {
                    try
                    {
                        using (StreamWriter sw = new($"../DB/LogDbInfo/{DateTime.Today.Day}-{DateTime.Today.Month}-{DateTime.Today.Year}-inform.txt", true))
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
