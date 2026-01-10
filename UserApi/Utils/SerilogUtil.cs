
using Serilog;
using Serilog.Events;
using Serilog.Filters;

namespace UserApi.Utils;

public static class SerilogUtil
{
    public static void Configure(WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((ctx, lc) =>
        {
            lc.MinimumLevel.Debug()
              .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
              .MinimumLevel.Override("System", LogEventLevel.Warning)
              .Enrich.FromLogContext()
              .Enrich.WithProperty("ApplicationName", builder.Environment.ApplicationName);
            ConfigureLevelFile(lc, LogEventLevel.Information, "info.log");
            ConfigureLevelFile(lc, LogEventLevel.Warning, "warning.log");
            ConfigureLevelFile(lc, LogEventLevel.Error, "error.log");
            ConfigureLevelFile(lc, LogEventLevel.Debug, "debug.log");

            // Text Channel logs
            lc.WriteTo.Logger(l => l
                .Filter.ByIncludingOnly(Matching.WithProperty("Channel", "Text"))
                .WriteTo.File(
                    path: Path.Combine("Logs", "Text", "text-.log"),
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 14,
                    shared: true,
                    flushToDiskInterval: TimeSpan.FromSeconds(2),
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {Message:lj}{NewLine}{Exception}"
                ));
            
            // ✅ Sentry (events + breadcrumbs)
            var dsn = ctx.Configuration["Sentry:Dsn"];
            if (!string.IsNullOrWhiteSpace(dsn))
            {
                lc.WriteTo.Sentry(o =>
                {
                    o.Dsn = dsn;

                    // logs at/above this level become Sentry "events"
                    o.MinimumEventLevel = ParseLevel(ctx.Configuration["Sentry:MinimumEventLevel"], LogEventLevel.Error);

                    // lower level logs can be attached as breadcrumbs
                    o.MinimumBreadcrumbLevel = ParseLevel(ctx.Configuration["Sentry:MinimumBreadcrumbLevel"], LogEventLevel.Information);

                    o.Environment = ctx.Configuration["Sentry:Environment"] ?? builder.Environment.EnvironmentName;
                    o.Release = ctx.Configuration["Sentry:Release"];
                });
            }

            // In Development, log to console as well
            lc.WriteTo.Console();
        });
    }

    private static LogEventLevel ParseLevel(string? value, LogEventLevel fallback)
        => Enum.TryParse(value, ignoreCase: true, out LogEventLevel lvl) ? lvl : fallback;

    private static void ConfigureLevelFile(LoggerConfiguration lc, LogEventLevel level, string fileName)
    {
        lc.WriteTo.Logger(l => l
            .Filter.ByIncludingOnly(e => e.Level == level)
            .WriteTo.File(
                path: Path.Combine("Logs", level.ToString(), fileName.Replace(".log", "-.log")),
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 14,
                shared: true,
                flushToDiskInterval: TimeSpan.FromSeconds(2),
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
            ));
    }

    public static void LogText(string message)
    {
        Log.ForContext("Channel", "Text").Information(message);
    }

    public static void LogInfo(string message)
    {
        Log.Information(message);
    }

    public static void LogWarning(string message)
    {
        Log.Warning(message);
    }

    public static void LogError(string message)
    {
        Log.Error(message);
    }

    public static void LogDebug(string message)
    {
        Log.Debug(message);
    }
}