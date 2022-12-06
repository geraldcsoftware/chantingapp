using Serilog;
using Serilog.Templates;
using Serilog.Templates.Themes;

namespace ChantingApp.Api;

public static class LoggerConfiguration
{
    private const string LogTemplate = """
                           {@t:yyyy/MM/dd HH:mm:ss} [{@l} - {SourceContext}] {@m}
                           {#if IsDefined(@x)}{@x}
                           {#end}
                           {NewLine}
                           """;

    public static void ConfigureLogging(this WebApplicationBuilder appBuilder)
    {
        ArgumentNullException.ThrowIfNull(appBuilder);

        appBuilder.Host.UseSerilog((context, loggerConfig) =>
        {
            loggerConfig.ReadFrom.Configuration(context.Configuration);
            loggerConfig.WriteTo.Console(new ExpressionTemplate(LogTemplate, theme: TemplateTheme.Code));
        });
    }
}