using Duende.IdentityServer.Models;

namespace Tyr.Authen;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
    [
        new IdentityResources.OpenId()
    ];

    public static IEnumerable<ApiScope> ApiScopes => [ new("foultalk") ];

    public static IEnumerable<Client> Clients =>
    [
        new()
        {
            ClientId = "foulbot",
            AllowedGrantTypes = [ "client_credentials" ],
            AllowedScopes = [ "foultalk" ],
            AccessTokenLifetime = 1200,
            ClientSecrets =
            [
                new()
                {
                    Value = "foulbot".Sha256()
                }
            ]
        }
    ];
}
