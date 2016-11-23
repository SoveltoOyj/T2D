using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace InventoryApi.Test
{
	public class AttributeTest : InventoryApiBase
	{
		private string _url = "api/metadata/Attribute";
		[Fact]
		public async void GetAll_ShouldReturnData()
		{
			using (var client = _server.CreateClient().AcceptJson())
			{
				var response = await client.GetAsync(_url);
				var result = await response.Content.ReadAsJsonAsync<List<T2D.Model.Attribute>>();

				Assert.NotNull(result);
				Assert.NotEmpty(result);
				Assert.Contains(result, i => i.Name.ToLower() == "relations");

			}
		}

		[Fact]
		public async void GetId1_ShouldReturnData()
		{
			using (var client = _server.CreateClient().AcceptJson())
			{
				var response = await client.GetAsync($"{_url}/1");
				var result = await response.Content.ReadAsJsonAsync<T2D.Model.Attribute>();

				Assert.NotNull(result);
				Assert.Equal(1, result.Id);
				Assert.NotNull(result.Name);
			}
		}



	}
}
