using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using System.Threading.Tasks;

namespace T2D.Log
{
	public static class AttributeLogRepository<T> where T : class
	{
		private static readonly string Endpoint = "https://t2dlog.documents.azure.com:443/";
		private static readonly string Key = "z8D8QXrOKq2tQiTov5jWeknTsXf6u81KlYp0vxRqp8N54sB1wx6Vxdvx8EUIlGNOy0RmRxgNNw48Kl4Mxzf4Fw==";
		private static readonly string DatabaseId = "AttributeLog";
		private static readonly string CollectionId = "Attributes";
		private static DocumentClient client;


//		public static void Initialize()
		static AttributeLogRepository()
		{
			client = new DocumentClient(new Uri(Endpoint), Key);
			CreateDatabaseIfNotExistsAsync().Wait();
			CreateCollectionIfNotExistsAsync().Wait();
		}

		public static async Task<Document> CreateItemAsync(T item)
		{
			return await client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId), item);
		}

		private static async Task CreateDatabaseIfNotExistsAsync()
		{
			try
			{
				await client.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(DatabaseId));
			}
			catch (DocumentClientException e)
			{
				if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
				{
					await client.CreateDatabaseAsync(new Database { Id = DatabaseId });
				}
				else
				{
					throw;
				}
			}
		}

		private static async Task CreateCollectionIfNotExistsAsync()
		{
			try
			{
				await client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId));
			}
			catch (DocumentClientException e)
			{
				if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
				{
					await client.CreateDocumentCollectionAsync(
							UriFactory.CreateDatabaseUri(DatabaseId),
							new DocumentCollection { Id = CollectionId },
							new RequestOptions { OfferThroughput = 1000 });
				}
				else
				{
					throw;
				}
			}
		}
	}
}
