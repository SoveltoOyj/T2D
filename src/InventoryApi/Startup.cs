﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
				c.IncludeXmlComments(System.IO.Path.Combine(basePath, "InventoryApi.xml"));
				c.IncludeXmlComments(System.IO.Path.Combine(basePath, "T2D.Model.xml"));
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
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "ThingToData API V1");
			});

			app.UseDefaultFiles();
			app.UseStaticFiles();
		}
	}
}
