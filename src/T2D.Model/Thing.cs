using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
namespace T2D.Model
{
	public class Thing:IModel
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public double Version { get; set; }

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
