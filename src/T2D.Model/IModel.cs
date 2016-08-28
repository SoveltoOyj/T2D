using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace T2D.Model
{


	public class ThingId
	{
		public Uri CreatorUri { get; set; }
		public string UniqueString { get; set; }

		public static ThingId Create(string creatorUriStr, string uniqueString)
		{
			if (string.IsNullOrWhiteSpace(creatorUriStr))
			{
				return new ThingId { CreatorUri = null, UniqueString = uniqueString };
			}
			return new ThingId { CreatorUri = new Uri(creatorUriStr, UriKind.Relative), UniqueString = uniqueString };
		}
		public static ThingId Create(Uri creatorUri, string uniqueString)
		{
			return new ThingId { CreatorUri = creatorUri, UniqueString = uniqueString };
		}

		public override string ToString()
		{
			var json = new DataContractJsonSerializer(this.GetType());
			using (var ms = new MemoryStream())
			{
				json.WriteObject(ms, this);
				ms.Position = 0;
				return Encoding.UTF8.GetString(ms.ToArray());
			};
		}
	}

	public interface IBaseModel
	{
	}

	public interface IThingModel:IBaseModel
	{
		ThingId Id { get; set; }
		string Title { get; set; }
	}

	public interface IModel : IBaseModel
	{
		string Id { get; set; }
	}

	public interface IEnumModel : IModel
	{
		string Name { get; set; }
	}


}