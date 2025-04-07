using System.Globalization;
using System.Text;
using Duende.IdentityServer.Licensing;
using Tyr.Authen;
using Serilog;
using Tyr.Framework;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(outputTemplate: HostExtensions.ConsoleLogOutputTemplate)
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);
    var isDebug = false;
#if DEBUG
    isDebug = true;
#endif

    var config = TyrHostConfiguration.Default(
        builder.Configuration,
        "Tyr.Authen",
        isDebug: isDebug);

    await builder.ConfigureTyrApplicationBuilderAsync(config)
        .ConfigureAwait(false);

    builder.AddIdentityServer();

    var app = builder.Build();

    app.ConfigureTyrApplication(config);
    app.UseIdentityServer();

    app.Lifetime.ApplicationStopping.Register(() =>
    {
        var usage = app.Services.GetRequiredService<LicenseUsageSummary>();
        Log.Information(Summary(usage));
    });

    await app.RunAsync()
        .ConfigureAwait(false);
}
catch (Exception ex) when (ex is not HostAbortedException)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    await Log.CloseAndFlushAsync()
        .ConfigureAwait(false);
}

static string Summary(LicenseUsageSummary usage)
{
    var sb = new StringBuilder();
    sb.AppendLine("IdentityServer Usage Summary:");
    sb.AppendLine(CultureInfo.InvariantCulture, $"  License: {usage.LicenseEdition}");
    var features = usage.FeaturesUsed.Count > 0 ? string.Join(", ", usage.FeaturesUsed) : "None";
    sb.AppendLine(CultureInfo.InvariantCulture, $"  Business and Enterprise Edition Features Used: {features}");
    sb.AppendLine(CultureInfo.InvariantCulture, $"  {usage.ClientsUsed.Count} Client Id(s) Used");
    sb.AppendLine(CultureInfo.InvariantCulture, $"  {usage.IssuersUsed.Count} Issuer(s) Used");

    return sb.ToString();
}
