using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Api
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

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers();

			var domain = Configuration["Auth:Authority"];
			services.AddAuthentication("Bearer")
				.AddJwtBearer("Bearer", options =>
				{
					options.Authority = domain;
					options.Audience = Configuration["Auth:Audience"];
				});


			services.AddAuthorization(options =>
			{
				options.AddPolicy("Tenogy.Api:Access", policy => policy.Requirements.Add(new HasScopeRequirement("Tenogy.Api:Access", domain)));
				options.AddPolicy("Tenogy.Api:Access1", policy => policy.Requirements.Add(new HasScopeRequirement("Tenogy.Api:Access1", domain)));
				options.AddPolicy("Tenogy.Api:Access2", policy => policy.Requirements.Add(new HasScopeRequirement("Tenogy.Api:Access2", domain)));
			});

			// register the scope authorization handler
			services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
