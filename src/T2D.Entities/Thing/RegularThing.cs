using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace T2D.Entities
{
	public class RegularThing : GenericThing
	{
		public string Description { get; set; }
		public bool Logging { get; set; }
		public bool IsLocalOnly { get; set; }
        public int StatusId { get; set; }
        public Status Status { get; set; }
        public int LocationTypeId { get; set; }
        public LocationType LocationType { get; set; }
        public DateTime? Location_Timestamp { get; set; }
        [StringLength(1024)]
		public string Location_Gps { get; set; }
		public bool IsGpsPublic { get; set; }
        [StringLength(256)]
        public string Location_StreetAddress { get; set; }
		[StringLength(256)]
		public string Location_Fqdn { get; set; }
		[StringLength(512)]
		public string Location_US { get; set; }
        public long? Preferred_LocationTypeId { get; set; }
		public DateTime? PreferredLocation_Timestamp { get; set; }
        [StringLength(1024)]
        public string PreferredLocation_GPS { get; set; }
        [StringLength(256)]
        public string PreferredLocation_StreetAddress { get; set; }
		[StringLength(256)]
		public string PreferredLocation_Fqdn { get; set; }
		[StringLength(512)]
		public string PreferredLocation_US { get; set; }

	}
}
