namespace Tyr.Authen;

internal static class IdentityServerExtensions
{
    public static WebApplicationBuilder AddIdentityServer(this WebApplicationBuilder builder)
    {
        builder.Services.AddIdentityServer(options =>
            {
                options.KeyManagement.KeyPath = "/app/keys";
            })
            .AddInMemoryIdentityResources(Config.IdentityResources)
            .AddInMemoryApiScopes(Config.ApiScopes)
            .AddInMemoryClients(Config.GetClients(builder.Configuration))
            .AddLicenseSummary();

        return builder;
    }
}
