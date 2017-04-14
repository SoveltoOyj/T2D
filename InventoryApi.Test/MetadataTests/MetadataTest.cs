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
	public class MetadataTest : InventoryApiBase
	{
		private string _url = "api/metadata";

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


		[Fact]
		public async void GetEnumNames_ShouldReturnData()
		{
			var response = await _client.GetAsync($"{_url}/EnumNames");
//			var result = await response.Content.ReadAsStringAsync();
			var result = await response.Content.ReadAsJsonAsync<List<string>>();

			response.EnsureSuccessStatusCode();
			Assert.NotNull(result);
			Assert.NotEmpty(result);
			Assert.True(result.Count > 5);
			Assert.True(result.Contains("Role"));
		}

	}
}
