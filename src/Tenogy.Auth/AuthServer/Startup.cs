using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Tenogy.Auth
{
	public class Startup
	{
		public Startup(IConfiguration configuration, IWebHostEnvironment environment)
		{
			Configuration = configuration;
			Environment = environment;
		}

		public IConfiguration Configuration { get; }
		public IWebHostEnvironment Environment { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			var authConfig = new AuthConfig();
			Configuration.Bind("Auth", authConfig);

			var builder = services.AddIdentityServer()
				.AddInMemoryIdentityResources(new IdentityResource[]
				{
					new IdentityResources.OpenId()
				})
				.AddInMemoryApiResources(authConfig.Apis.Select(a => new ApiResource()
				{
					Name = a.Name,
					DisplayName = a.DisplayName,
					ApiSecrets = new[]
					{
						new Secret(a.Secret.Sha256())
					},
					Scopes = new HashSet<Scope>(a.Scopes.Select(s => new Scope(s.Name, s.DisplayName)))
				}).ToArray())
				.AddInMemoryClients(authConfig.Clients.Select(c => new Client
				{
					ClientId = c.ClientId,
					AllowedGrantTypes = GrantTypes.ClientCredentials,
					ClientSecrets =
					{
						new Secret(c.Secret.Sha256())
					},
					AllowOfflineAccess = false, //disable refresh tokens
					AllowedScopes = c.Scopes.ToList()
				}).ToArray());

			
			if (Environment.IsDevelopment())
			{
				builder.AddDeveloperSigningCredential();
			}
			else
			{
				string password = authConfig.Secret;
				Debug.Assert(!String.IsNullOrEmpty(password), "IdentityServer:Secret is missing from appsettings");
				string certificate = authConfig.Certificate;
				Debug.Assert(!String.IsNullOrEmpty(certificate), "IdentityServer:Certificate is missing from appsettings");

				var cert = new X509Certificate2(
					certificate,
					password,
					X509KeyStorageFlags.MachineKeySet |
					X509KeyStorageFlags.PersistKeySet |
					X509KeyStorageFlags.Exportable
				);
				builder.AddSigningCredential(cert);
			}
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			//app.UseRouting();

			app.UseIdentityServer();

			//app.UseEndpoints(endpoints =>
			//{
			//	endpoints.MapGet("/", async context =>
			//	{
			//		await context.Response.WriteAsync("Hello World!");
			//	});
			//});
		}
	}
}
