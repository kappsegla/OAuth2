using IdentityModel.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        public async Task DoSomeCalls()
        {
            // discover endpoints from metadata
            var disco = await DiscoveryClient.GetAsync("http://localhost:5000");
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return;
            }

            //request token
            var tokenClient = new TokenClient(disco.TokenEndpoint, "client", "secret");
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync("api1");

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }
            Console.WriteLine(tokenResponse.Json);   // https://jwt.io/

            //// request token username, password
            //var tokenClient = new TokenClient(disco.TokenEndpoint, "ro.client");
            //var tokenResponse = await tokenClient.RequestResourceOwnerPasswordAsync("bob", "password", "api1");

            //if (tokenResponse.IsError)
            //{
            //    Console.WriteLine(tokenResponse.Error);
            //    return;
            //}

            //Console.WriteLine(tokenResponse.Json);
            //Console.WriteLine("\n\n");

            // call api
            var client = new HttpClient();
            client.SetBearerToken(tokenResponse.AccessToken);

            //var response = await client.GetAsync("http://localhost:5001/api/Identity");
            var response = await client.GetAsync("http://localhost:5000/api/Identity");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var jasoncontent = await response.Content.ReadAsStringAsync();
                Console.WriteLine(JArray.Parse(jasoncontent));
            }
        }
        
        public static async Task Main(string[] args)
        {
             await new Program().DoSomeCalls();
        }
    }
}
