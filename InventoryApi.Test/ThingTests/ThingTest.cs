using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using T2D.Model;
using T2D.Model.InventoryApi;
using Xunit;
using Xunit.Abstractions;

namespace InventoryApi.Test
{
	public class ThingTest : InventoryApiBase
	{
		private string _url = "api/inventory/core";
		private string cfqdn = "inv1.sovelto.fi";

		public ThingTest(ITestOutputHelper output) : base(output){ }

		[Fact]
		public async void CreateLocalThing_ShouldBeOK()
		{
			var jsonContent = new JsonContent(new CreateLocalThingRequest {
				Session = "00000000-0000-0000-0000-000000000001",
				ThingId =  $"{cfqdn}/Test@{DateTime.Now.ToString()}",
				Title = "joku title",
				ThingType = T2D.Model.Enums.ThingType.RegularThing,
				OmnipotentThingId = $"{cfqdn}/M100"
			});
			var response = await _client.PostAsync($"{_url}/CreateLocalThing", jsonContent );

			response.EnsureSuccessStatusCode();
		}


		[Fact]
		public async void SetRoleRights_NewThing_ShouldBeOK()
		{
			string thingId = await CreateATestThing();
			var jsonContent = new JsonContent(new SetRoleAccessRighsRequest
			{
				Session = "00000000-0000-0000-0000-000000000001",
				ThingId = thingId,
				Role = "Owner",
				AttributeRoleRights = new List<AttributeRoleRight>
				{
					new AttributeRoleRight
					{
						Attribute = "Description",
						RoleAccessRights = new string[] { "Create", "Read", "Update" },
					},
					new AttributeRoleRight
					{
						Attribute = "Created",
						RoleAccessRights = new string[] { "Create", "Read" },
					},
					new AttributeRoleRight
					{
						Attribute = "Location",
						RoleAccessRights = new string[] { "Create", "Update" },
					},
				}
			});
			var response = await _client.PostAsync($"{_url}/SetRoleAccessRight", jsonContent);

			response.EnsureSuccessStatusCode();
		}

		//		[Fact]
		//		public async void GetT2_ShouldReturnData()
		//		{
		//			//using (var client = _server.CreateClient().AcceptJson())
		//			//{
		//			//	var response = await client.GetAsync($"{_url}/id?c={cfqdn}&u=T2");
		//			//	var result = await response.Content.ReadAsJsonAsync<BaseThing>();

		//			//	Assert.NotNull(result);
		//			//	Assert.Equal("inv1.sovelto.fi/T2", result.Id);
		//			//	Assert.NotNull(result.Title);
		//			//}
		//		}


		//		[Fact]
		//		public async void Post_ShouldAddOneThing()
		//		{
		//			//using (var client = _server.CreateClient().AcceptJson())
		//			//{
		//			//	var created = await this.CreateATestThing(client);
		//			//	await this.DeleteATestThing(created.Id, client);
		//			//}
		//		}

		//		[Fact]
		//		public async void Put_ShouldModifyAllFields()
		//		{

		//			//using (var client = _server.CreateClient().AcceptJson())
		//			//{
		//			//	var newThing= await this.CreateATestThing(client);

		//			//	//test PUT
		//			//	newThing.Title = "Modified";
		//			//	var putResponse = await client.PutAsJsonAsync($"{_url}", newThing);
		//			//	var modified = await putResponse.Content.ReadAsJsonAsync<BaseThing>();
		//			//	Assert.NotNull(modified);
		//			//	Assert.Equal(newThing.Id, modified.Id);
		//			//	Assert.Equal("Modified", modified.Title);


		//			//	//and then delete it
		//			//	await this.DeleteATestThing(newThing.Id, client);
		//			//}
		//		}

		//		[Fact]
		//		public async void Patch_ShouldModify()
		//		{
		//			string patchContent = @"
		//[
		// {
		//	'value': 'Modified',
		//   'path': 'title',
		//   'op': 'replace'
		//		}
		//]";

		//			//using (var client = _server.CreateClient().AcceptJson())
		//			//{
		//			//	var newThing = await this.CreateATestThing(client);

		//			//	//test PATCH
		//			//	var req = new HttpRequestMessage
		//			//	{
		//			//		Method = new HttpMethod("PATCH"),
		//			//		RequestUri = new Uri($"{_url}{GetThingIdQueryString(newThing.Id)}", UriKind.Relative),
		//			//		Content = new StringContent(patchContent, Encoding.UTF8, "application/json")
		//			//	};
		//			//	var response = await client.SendAsync(req);
		//			//	var modified = await response.Content.ReadAsJsonAsync<BaseThing>();
		//			//	Assert.NotNull(modified);
		//			//	Assert.Equal(newThing.Id, modified.Id);
		//			//	Assert.Equal("Modified", modified.Title);


		//			//	//and then delete it
		//			//	await this.DeleteATestThing(newThing.Id, client);
		//			//}
		//		}


		private async Task<string> CreateATestThing()
		{
			string thingId = $"{cfqdn}/Test@{DateTime.Now.ToString()} - {Guid.NewGuid()}";
			var jsonContent = new JsonContent(new CreateLocalThingRequest
			{
				Session = "00000000-0000-0000-0000-000000000001",
				ThingId = thingId,
				Title = "Test thing",
				ThingType = T2D.Model.Enums.ThingType.RegularThing,
				OmnipotentThingId = $"{cfqdn}/M100"
			});
			var response = await _client.PostAsync($"{_url}/CreateLocalThing", jsonContent);
			response.EnsureSuccessStatusCode();
			return thingId;
		}
		
		//		private async Task<System.Net.HttpStatusCode> DeleteATestThing(string id, HttpClient client)
		//		{
		//			var deleteResponse = await client.DeleteAsync($"{_url}{GetThingIdQueryString(id)}");
		//			var result = deleteResponse.StatusCode;
		//			Assert.Equal(System.Net.HttpStatusCode.OK, result);

		//			return result;
		//		}

		//		private string GetThingIdQueryString(string thingId)
		//		{
		//			return $"?c={cfqdn}&u={thingId.Split(new char[] { '/' })[1]}";
		//		}
	}
}
