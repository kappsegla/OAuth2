using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Stores;

namespace AuthorizationServer
{
    internal class MyClientStore : IClientStore
    {
        public Task<Client> FindClientByIdAsync(string clientId)
        {

            //TODO:Should get this information from database

            Client c = new Client
            {
                ClientId = "client",

                // no interactive user, use the clientid/secret for authentication
                AllowedGrantTypes = GrantTypes.ClientCredentials,

                // secret for authentication
                ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                // scopes that client has access to
                AllowedScopes = { "api1" }
            };
            return Task.FromResult(c);
        }
    }
}