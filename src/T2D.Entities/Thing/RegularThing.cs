using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace T2D.Entities
{
	public class RegularThing : GenericThing
	{
		//height and width are stored in millimeters (just an example)
		public bool Logging { get; set; }
		public bool IsLocalOnly { get; set; }
		public long LocationTypeId { get; set; }
		public DateTime? Location_Timestamp { get; set; }
		public string Location_GPS { get; set; }
		public string Location_StreetAddress { get; set; }
		public long? Location_Id { get; set; }
		public long? PreferredLocationTypeId { get; set; }
		public DateTime? PreferredLocation_Timestamp { get; set; }
		public string PreferredLocation_GPS { get; set; }
		public string PreferredLocation_StreetAddress { get; set; }
		public long? PreferredLocation_Id { get; set; }


	}
}
