using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace InventoryApi.Test
{
	public static class HttpClientExtensions
	{
		public static HttpClient AcceptJson(this HttpClient client)
		{
			client.DefaultRequestHeaders.Clear();
			client.DefaultRequestHeaders.Accept
					.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			return client;
		}
	}

	public static class HttpContentExtensions
	{
		public static async Task<T> ReadAsJsonAsync<T>(this HttpContent content)
		{
			string s = await content.ReadAsStringAsync();
			return JsonConvert.DeserializeObject<T>(s) ;
		}

	}
	public class JsonContent : StringContent
	{
		public JsonContent(object obj) :
				base(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json")
		{ }
	}

}
