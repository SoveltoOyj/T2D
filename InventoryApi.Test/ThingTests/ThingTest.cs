//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Http;
//using System.Text;
//using System.Threading.Tasks;
//using T2D.Model;
//using Xunit;

//namespace InventoryApi.Test
//{
//	public class ThingTest : InventoryApiBase
//	{
//		private string _url = "api/test/Thing";
//		private string cfqdn = "inv1.sovelto.fi";

//		[Fact]
//		public async void GetAll_ShouldReturnData()
//		{
//			//using (var client = _server.CreateClient().AcceptJson())
//			//{
//			//	var response = await client.GetAsync(_url);
//			//	var result = await response.Content.ReadAsJsonAsync<List<T2D.Model.BaseThing>>();

//			//	Assert.NotNull(result);
//			//	Assert.NotEmpty(result);
//			//	Assert.Contains(result, t => t.Title.ToLower() == "mysuitcase");
//			//}
//		}

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


//		private async Task<BaseThing> CreateATestThing(HttpClient client)
//		{
//			string us = Guid.NewGuid().ToString();
//			string id = $"{cfqdn}/{us}";
//			string title = "New Thing";

//			BaseThing newThing = new BaseThing
//			{
//				Id = id,
//				Title = title
//			};

//			//			var postResponse = await client.PostAsJsonAsync($"{_url}", newThing);
//			//			var created = await postResponse.Content.ReadAsJsonAsync<BaseThing>();
//			//Assert.NotNull(created);
//			//Assert.Equal(id, created.Id);
//			//Assert.Equal(title, created.Title);
//			//return created;
//			return null;
//		}

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
//	}
//}
