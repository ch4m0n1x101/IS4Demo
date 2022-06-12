using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;

namespace IS4Demo.Auth
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("YourAppScopeHere", "YourAppScopeHere")
            };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientName = "YourWebsiteIdentifierHere",
                    ClientId = "YourWebsiteIdentifierHere",
                    ClientSecrets = { new Secret("YourWebsiteIdentifierHere".Sha256()) },
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = true,
                    AlwaysSendClientClaims = true,
                    UpdateAccessTokenClaimsOnRefresh = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    RedirectUris = { "https://localhost:7251/signin-oidc" }, //Replace this with website Uri
                    FrontChannelLogoutUri = "https://localhost:7251/signout-oidc", //Replace this with website Uri
                    PostLogoutRedirectUris = { "https://localhost:7251/signout-callback-oidc" }, //Replace this with website Uri
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "YourAppScopeHere"
                    }
                },
                new Client
                {
                    ClientName = "YourApiIdentifierHere",
                    ClientId = "YourApiIdentifierHere",
                    ClientSecrets = { new Secret("YourApiIdentifierHere".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    RedirectUris = { "https://localhost:7268/signin-oidc" }, //Replace this with Api Uri
                    PostLogoutRedirectUris = { "https://localhost:7268/signout-callback-oidc" }, //Replace this with Api Uri 
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "YourAppScopeHere"
                    }
                }
            };

        public static IEnumerable<ApiResource> ApiResources =>
            new List<ApiResource>()
            {
                new ApiResource("IdentityServer4Demo.Auth")
                {
                    Scopes = { "YourAppNameHere" },
                    UserClaims =
                    {
                        JwtClaimTypes.Name,
                        JwtClaimTypes.Subject,
                        JwtClaimTypes.Role,
                    }
                },
                new ApiResource("IdentityServer4Demo.Blazor")
                {
                    Scopes = { "YourAppNameHere" },
                    UserClaims =
                    {
                        JwtClaimTypes.Name,
                        JwtClaimTypes.Subject,
                        JwtClaimTypes.Role,
                    }
                },
                new ApiResource("IdentityServer4Demo.Api")
                {
                    Scopes = { "YourAppNameHere" },
                    UserClaims =
                    {
                        JwtClaimTypes.Name,
                        JwtClaimTypes.Subject,
                        JwtClaimTypes.Role,
                    }
                }
            };
    }
}
