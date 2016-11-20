using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T2D.Helpers;

namespace T2D.Model
{
	public class RegularThing:BaseThing
	{
		public string Description { get; set; }
		public string Archetype { get; set; }
		public string Parted { get; set; }
		public string LocationType { get; set; }
		public DateTime? Location_Timestamp { get; set; }
		public string Location_GPS { get; set; }
		public string Location_StreetAddress { get; set; }
		public string PreferredLocationType { get; set; }
		public DateTime? PreferredLocation_Timestamp { get; set; }
		public string PreferredLocation_GPS { get; set; }
		public string PreferredLocation_StreetAddress { get; set; }

	}
}
