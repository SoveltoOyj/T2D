using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.Swagger.Model;
using Microsoft.Extensions.PlatformAbstractions;

namespace InventoryApi
{
	public class Startup
	{
		public Startup(IHostingEnvironment env)
		{
			var builder = new ConfigurationBuilder()
					.SetBasePath(env.ContentRootPath)
					.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
					.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
					.AddEnvironmentVariables();
			Configuration = builder.Build();
		}

		public IConfigurationRoot Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			var basePath = PlatformServices.Default.Application.ApplicationBasePath;

			// Add framework services.
			services.AddMvc();

			services.AddSwaggerGen();
			services.ConfigureSwaggerGen(options =>
			{
				options.SingleApiVersion(new Info
				{
					Version = "v1",
					Title = "T2D Inventory API",
					Description = "Thing to Data Inventory Api",
					TermsOfService = "NA",
					Contact = new Contact() { Name = "T2D Implementation Team", Email = "ahti.haukilehto@sovelto.fi", Url = "https://sovelto.fi" },
				});
				options.DescribeAllEnumsAsStrings();
				options.IncludeXmlComments(System.IO.Path.Combine(basePath, "InventoryApi.xml"));
				options.IncludeXmlComments(System.IO.Path.Combine(basePath, "T2D.Model.xml"));
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
			app.UseMvc();
			app.UseSwagger();
			app.UseSwaggerUi();
			app.UseDefaultFiles();
			app.UseStaticFiles();
		}
	}
}
