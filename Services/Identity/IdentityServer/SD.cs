using IdentityServer4;
using IdentityServer4.Models;

namespace IdentityServer
{
    public static class SD
    {
        public const string Admin = "Admin";
        public const string Customer = "Customer";

        public static IEnumerable<IdentityResource> IdentityResources =>
         new IdentityResource[]
         {
              new IdentityResources.OpenId(),
              new IdentityResources.Profile()
              
         };

        public static IEnumerable<ApiScope> ApiScopes =>
          new ApiScope[]
          {
               new ApiScope("catalog", "Catalog API"),
               new ApiScope("basket", "Basket API")
          };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                  {
                       ClientId = "catalogClient",
                       ClientName = "Catalog Api",
                       AllowedGrantTypes = GrantTypes.Code,
                       RequirePkce = false,
                       AllowRememberConsent = false,
                       RedirectUris = new List<string>()
                       {
                           "https://localhost:5001/signin-oidc"
                       },
                       PostLogoutRedirectUris = new List<string>()
                       {
                           "https://localhost:5001/signout-callback-oidc"
                       },
                       ClientSecrets = new List<Secret>
                       {
                           new Secret("secret".Sha256())
                       },
                       AllowedScopes = new List<string>
                       {
                           IdentityServerConstants.StandardScopes.OpenId,
                           IdentityServerConstants.StandardScopes.Profile,
                           "catalogAPI"
                           
                       }
                  }

            };
    }
}
