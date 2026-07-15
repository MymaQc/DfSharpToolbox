namespace Toolbox.Diagnostics;

public static class ToolboxLogger
{
    private const string DefaultFileName = "toolbox-errors.log";
    private static readonly Lock Sync = new();
    private static bool _initialized;
    private static string _logDirectory = Path.Combine(Environment.CurrentDirectory, "logs");
    private static string _logFileName = DefaultFileName;

    public static void Initialize(string? logDirectory = null, string? logFileName = null)
    {
        lock (Sync)
        {
            if (!string.IsNullOrWhiteSpace(logDirectory))
            {
                _logDirectory = logDirectory;
            }

            if (!string.IsNullOrWhiteSpace(logFileName))
            {
                _logFileName = logFileName;
            }

            if (_initialized)
            {
                return;
            }

            AppDomain.CurrentDomain.UnhandledException += (_, args) =>
            {
                if (args.ExceptionObject is Exception exception)
                {
                    Error(exception, "Unhandled exception");
                    return;
                }

                Error($"Unhandled exception object: {args.ExceptionObject}", "Unhandled exception");
            };

            TaskScheduler.UnobservedTaskException += (_, args) =>
            {
                Error(args.Exception, "Unobserved task exception");
                args.SetObserved();
            };

            _initialized = true;
        }
    }

    public static string GetLogFilePath()
    {
        Initialize();
        lock (Sync)
        {
            return Path.Combine(_logDirectory, _logFileName);
        }
    }

    public static void Info(string message, string context = "Toolbox")
    {
        Write("INFO", context, message);
    }

    public static void Warning(string message, string context = "Toolbox")
    {
        Write("WARN", context, message);
    }

    public static void Error(string message, string context = "Toolbox")
    {
        Write("ERROR", context, message);
    }

    public static void Error(Exception exception, string context = "Toolbox")
    {
        ArgumentNullException.ThrowIfNull(exception);
        Write("ERROR", context, exception.ToString());
    }

    public static bool Try(Action action, string context = "Toolbox")
    {
        ArgumentNullException.ThrowIfNull(action);

        try
        {
            action();
            return true;
        }
        catch (Exception exception)
        {
            Error(exception, context);
            return false;
        }
    }

    public static Action<T> Wrap<T>(Action<T> action, string context = "Toolbox")
    {
        ArgumentNullException.ThrowIfNull(action);
        return value => Try(() => action(value), context);
    }

    private static void Write(string level, string context, string message)
    {
        Initialize();

        var timestamp = DateTimeOffset.Now.ToString("yyyy-MM-dd HH:mm:ss.fff zzz");
        var line = $"[{timestamp}] [{level}] [{context}] {message}";

        Console.Error.WriteLine(line);

        lock (Sync)
        {
            try
            {
                Directory.CreateDirectory(_logDirectory);
                File.AppendAllText(Path.Combine(_logDirectory, _logFileName), line + Environment.NewLine);
            }
            catch (Exception fileException)
            {
                Console.Error.WriteLine($"[{timestamp}] [ERROR] [ToolboxLogger] Unable to write log file: {fileException}");
            }
        }
    }
}
