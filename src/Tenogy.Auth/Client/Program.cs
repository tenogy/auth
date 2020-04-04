using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
namespace Client
{
	class Program
	{
		static async Task Main(string[] args)
		{
			var client = new HttpClient();
			var disco = await client.GetDiscoveryDocumentAsync("https://localhost:44350");
			if (disco.IsError)
			{
				Console.WriteLine(disco.Error);
				return;
			}
			// request token
			var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
			{
				Address = disco.TokenEndpoint,

				ClientId = "AF8754D8-796C-456F-91DF-1B0D21EB085E",
				ClientSecret = "testtest",
				Scope = "Tenogy.Api:Access2"
			});

			if (tokenResponse.IsError)
			{
				Console.WriteLine(tokenResponse.Error);
				return;
			}

			Console.WriteLine(tokenResponse.Json);


			// call api
			client = new HttpClient();
			client.SetBearerToken(tokenResponse.AccessToken);

			await CallApi(client, "access");

			await CallApi(client, "scopes/scope");
			await CallApi(client, "scopes/scope1");
			await CallApi(client, "scopes/scope2");
		}

		private static async Task CallApi(HttpClient client, string endpoint)
		{
			Console.WriteLine($"Call {endpoint}");
			var response = await client.GetAsync($"https://localhost:44360/{endpoint}");
			if (!response.IsSuccessStatusCode)
			{
				Console.WriteLine(response.StatusCode);
			}
			else
			{
				var content = await response.Content.ReadAsStringAsync();
				Console.WriteLine(content);
			}
		}
	}
}
