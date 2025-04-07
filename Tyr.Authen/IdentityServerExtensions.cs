namespace Tyr.Authen;

internal static class IdentityServerExtensions
{
    public static WebApplicationBuilder AddIdentityServer(this WebApplicationBuilder builder)
    {
        builder.Services.AddIdentityServer()
            .AddInMemoryIdentityResources(Config.IdentityResources)
            .AddInMemoryApiScopes(Config.ApiScopes)
            .AddInMemoryClients(Config.Clients)
            .AddLicenseSummary();

        return builder;
    }
}
