using Duende.IdentityServer.Models;

namespace Tyr.Authen;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
    [
        new IdentityResources.OpenId()
    ];

    public static IEnumerable<ApiScope> ApiScopes => [ new("machine") ];

    public static IEnumerable<Client> GetClients(IConfiguration configuration) =>
    [
        new()
        {
            ClientId = "machine",
            AllowedGrantTypes = [ "client_credentials" ],
            AllowedScopes = [ "machine" ],
            AccessTokenLifetime = 1200,
            ClientSecrets =
            [
                new()
                {
                    Value = (configuration["MachineClientSecret"] ?? throw new InvalidOperationException("MachineClientSecret is not set.")).Sha256()
                }
            ]
        }
    ];
}
