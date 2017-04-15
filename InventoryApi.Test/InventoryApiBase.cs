using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace InventoryApi.Test
{
	//http://dotnetliberty.com/index.php/2015/12/17/asp-net-5-web-api-integration-testing/

	public class InventoryApiBase
	{
		protected readonly TestServer _server;
		protected HttpClient _client { get; }

		protected readonly ITestOutputHelper _output;

		public InventoryApiBase(ITestOutputHelper output)
		{
			_server = new TestServer(new WebHostBuilder()
				.UseStartup<Startup>())
				;
			_client = _server.CreateClient();
			_client.AcceptJson();
			_output = output;
		}
		public void Dispose()
		{
			_client.Dispose();
			_server.Dispose();
		}

	}
}
