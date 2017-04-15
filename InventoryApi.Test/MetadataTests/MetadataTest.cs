using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using T2D.Model;
using Xunit;
using Xunit.Abstractions;

namespace InventoryApi.Test
{
	public class MetadataTest : InventoryApiBase
	{
		public MetadataTest(ITestOutputHelper output) : base(output){	}

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
			var result = await response.Content.ReadAsJsonAsync<List<string>>();

			response.EnsureSuccessStatusCode();
			Assert.NotNull(result);
			Assert.NotEmpty(result);
			Assert.True(result.Count > 5);
			Assert.True(result.Contains("Role"));
		}

		[Fact]
		public async void GetEnumValues_ShouldReturnData()
		{
			var response = await _client.GetAsync($"{_url}/EnumNames");
			var enumNames = await response.Content.ReadAsJsonAsync<List<string>>();

			foreach (var enumName in enumNames)
			{
				response = await _client.GetAsync($"{_url}/EnumValues?enumName={enumName}");
				var result = await response.Content.ReadAsJsonAsync<List<ModelEnum>>();
				response.EnsureSuccessStatusCode();
				Assert.NotNull(result);
				Assert.NotEmpty(result);
				if (string.Compare(enumName, "AttributeType", true) == 0)
				{
					Assert.True(result.Any(m=>m.Id== 1 && m.Name == "T2DAttribute" ));
				}
				if (string.Compare(enumName, "FunctionalStatus", true) == 0)
				{
					Assert.True(result.Any(m => m.Id == 1 && m.Name == "NormalOperational"));
				}
				if (string.Compare(enumName, "Attribute", true) == 0)
				{
					Assert.True(result.Any(m => m.Id == 1 && m.Name == "Relations"));
				}

				_output.WriteLine(enumName);
				
			}
		}


	}
}
