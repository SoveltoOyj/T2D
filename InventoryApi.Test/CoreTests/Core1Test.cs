using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using T2D.Model;
using Xunit;

namespace InventoryApi.Test
{
	public class Core1Test : InventoryApiBase
	{
		private string _url = "api/metadata/version";

		[Fact]
		public async void GetApiVersion_ShouldReturnData()
		{
			var response = await _client.GetAsync($"{_url}/ApiVersion");
			var result = await response.Content.ReadAsStringAsync();

			response.EnsureSuccessStatusCode();
			Assert.NotNull(result);
			Assert.NotEmpty(result);
			Assert.Equal<string>("1.0.0.0", result);
		}
	}
}
