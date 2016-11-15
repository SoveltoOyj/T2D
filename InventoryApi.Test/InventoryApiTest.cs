using Microsoft.AspNet.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace InventoryApi.Test
{
	//http://dotnetliberty.com/index.php/2015/12/17/asp-net-5-web-api-integration-testing/

	public class InventoryApiTest
	{
		private readonly TestServer _server;

		public InventoryApiTest()
		{
			_server = new TestServer(TestServer.CreateBuilder().UseStartup<Startup>());
		}


		[Fact]
		public async void Ping()
		{
			using (var client = _server.CreateClient().AcceptJson())
			{
				var response = await client.GetAsync("api/metadata/Role");
				var result = await response.Content.ReadAsJsonAsync<List<T2D.Model.Role>>();

				Assert.NotNull(result);
				Assert.Empty(result);
			}
		}
	}
}
