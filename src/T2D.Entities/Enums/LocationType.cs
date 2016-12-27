using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using T2D.Helpers;

namespace T2D.Entities
{
	public class LocationType:IEnumEntity
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public override string ToString()
		{
			return this.ToJson();
		}


	}

	public enum LocationTypeEnum
	{
		GPS = 1,
		Street,
		Thing,
	};
}
