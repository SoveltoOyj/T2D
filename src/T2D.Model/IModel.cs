using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace T2D.Model
{


	public struct ThingId
	{
		public string CreatorUri { get; set; }
		public string UniqueString { get; set; }

		public static ThingId Create(string creatorUri, string uniqueString)
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