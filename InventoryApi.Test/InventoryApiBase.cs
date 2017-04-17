using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using T2D.Model.InventoryApi;
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

		protected string _cfqdn = "inv1.sovelto.fi";


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



		protected async Task<string> CreateATestThing()
		{
			string thingId = $"{_cfqdn}/Test@{DateTime.Now.ToString()} - {Guid.NewGuid()}";
			var jsonContent = new JsonContent(new CreateLocalThingRequest
			{
				Session = "00000000-0000-0000-0000-000000000001",
				ThingId = $"{_cfqdn}/M100",
				Role = "Omnipotent",
				NewThingId = thingId,
				Title = "Test thing",
				ThingType = T2D.Model.Enums.ThingType.RegularThing,
			});
			var response = await _client.PostAsync($"api/inventory/core/CreateLocalThing", jsonContent);
			response.EnsureSuccessStatusCode();
			return thingId;
		}
	}
}
