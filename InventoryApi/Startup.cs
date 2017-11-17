//problems in Swagger Azure deployment.
// You have to do the following
// 1. Build locally, use env.Development and check that T2DConnectionString is correct
// 2. Start InventoryAPI and copy (using browser) /swagger/v1/swagger.json --> wwwroot/swagger.json
// 3. deploy to Azure
// 4. Check that environment is either Staging or Production (not Development) and T2DConnectionString is correct

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using T2D.Infra;
using Hangfire;

namespace InventoryApi
{
	public class Startup
	{
		public Startup(IHostingEnvironment env)
		{
			Env = env;
			Console.WriteLine("Käynnistyy");
			var builder = new ConfigurationBuilder()
					.SetBasePath(env.ContentRootPath)
					.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
					.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
					.AddEnvironmentVariables();

			if (env.IsDevelopment())
			{
				builder.AddUserSecrets<Startup>();
			}

			builder.AddEnvironmentVariables();

			Configuration = builder.Build();
		}

		public IConfigurationRoot Configuration { get; }
		public IHostingEnvironment Env;


		public void ConfigureServices(IServiceCollection services)
		{
			var basePath = AppContext.BaseDirectory;

			services.AddScoped<EfContext>(provider => new EfContext(Configuration.GetConnectionString("T2DConnection"))) ;

			// Add Hangfire services.  
			services.AddHangfire(x => x.UseSqlServerStorage(Configuration.GetConnectionString("T2DConnection")));


			// Add framework services.
			services.AddMvc();

			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info
				{
					Title = "ThingToData",
					Version = "v1",
					Contact = new Swashbuckle.AspNetCore.Swagger.Contact { Email = "ahti.haukilehto@sovelto.fi", Name = "Sovelto T2D Team" },
					Description = "Thing to data reference implementation",
				});
				c.DescribeAllParametersInCamelCase();
				c.DescribeAllEnumsAsStrings();
				c.OperationFilter<Extensions.AuthorizationHeaderParameterOperationFilter>();

				if (Env.IsDevelopment())
				{
					c.IncludeXmlComments(System.IO.Path.Combine(basePath, "InventoryApi.xml"));
					c.IncludeXmlComments(System.IO.Path.Combine(basePath, "T2D.Model.xml"));
				}
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
		{
			loggerFactory.AddConsole(Configuration.GetSection("Logging"));
			loggerFactory.AddDebug();
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseJwtBearerAuthentication(new JwtBearerOptions
			{
				Authority = string.Format("https://login.microsoftonline.com/tfp/{0}/{1}/v2.0/",
									 Configuration["Authentication:AzureAd:Tenant"], Configuration["Authentication:AzureAd:Policy"]),
				Audience = Configuration["Authentication:AzureAd:ClientId"],
				Events = new JwtBearerEvents
				{
					OnAuthenticationFailed = AuthenticationFailed
				}
			});


			app.UseMvc();
			app.UseSwagger();
			app.UseSwaggerUI
				(c =>
				{
					if (env.IsDevelopment())
					{
						c.SwaggerEndpoint("/swagger/v1/swagger.json", "ThingToData API V1");
					}
					else
					{
						c.SwaggerEndpoint("/swagger.json", "ThingToData API V1");
					}
				});

			//app.UseHangfireDashboard();
			app.UseHangfireDashboard("/hangfire", new DashboardOptions
			{
				Authorization = new[] { new Extensions.MyHangfireAuthorizationFilter() }
			});

			app.UseHangfireServer();

			app.UseDefaultFiles();
			app.UseStaticFiles();
		}

		private Task AuthenticationFailed(AuthenticationFailedContext arg)
		{
			// For debugging purposes only!
			var s = $"AuthenticationFailed: {arg.Exception.Message}";
			arg.Response.ContentLength = s.Length;
			arg.Response.Body.Write(Encoding.UTF8.GetBytes(s), 0, s.Length);
			return Task.FromResult(0);
		}
	}
}
