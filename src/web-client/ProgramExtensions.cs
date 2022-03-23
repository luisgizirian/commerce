using Serilog;

public static class ProgramExtensions
{
    private const string AppName = "Web Client";

    public static void AddCustomSerilog(this WebApplicationBuilder builder)
    {
        // var seqServerUrl = builder.Configuration["SeqServerUrl"];
        var seqServerUrl = "http://seq";

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .WriteTo.Console()
            .WriteTo.Seq(seqServerUrl)
            .Enrich.WithProperty("ApplicationName", AppName)
            .CreateLogger();

        builder.Host.UseSerilog();
    }
}