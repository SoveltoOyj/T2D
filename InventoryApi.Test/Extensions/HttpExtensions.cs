using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
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
			// I'm only accepting JSON from the server, and I don't want to add a dependency on
			// System.Runtime.Serialization.Xml which is required when using the default formatters
			return await content.ReadAsAsync<T>(GetJsonFormatters());
		}

		private static IEnumerable<MediaTypeFormatter> GetJsonFormatters()
		{
			yield return new JsonMediaTypeFormatter();
		}

	}
}
