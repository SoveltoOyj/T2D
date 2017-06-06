using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using T2D.Model.InventoryApi;
using Xunit;
using Xunit.Abstractions;

namespace InventoryApi.Test
{

	public class InventoryBLBase
	{
		protected readonly ITestOutputHelper _output;

		protected string _cfqdn = "inv1.sovelto.fi";
		protected string _authenticatedSession = "00000000-0000-0000-0000-000000000001";
		protected string _anonymousSession = "00000000-0000-0000-0000-000000000002";

		public IConfigurationRoot Configuration { get; set; }


		public InventoryBLBase(ITestOutputHelper output)
		{
			_output = output;
			var builder = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
			 .AddJsonFile("appsettings.json");

			Configuration = builder.Build();

		}
		public void Dispose()
		{
		}

	}
}
