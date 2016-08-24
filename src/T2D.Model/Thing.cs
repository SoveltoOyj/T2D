using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
namespace T2D.Model
{
	public class Thing:IThingModel
	{
		public ThingId Id { get; set; }

		public float Height { get; set; }
		public float Width { get; set; }

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
}
