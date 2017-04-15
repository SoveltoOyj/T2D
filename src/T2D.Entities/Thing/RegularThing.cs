using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace T2D.Entities
{
	/// <summary>
	/// Things that are real Instances. That is, not ArchetypeThings.
	/// </summary>
	public class RegularThing : GenericThing
	{
		public Guid? ArchetypeThingId { get; set; }
		public bool Logging { get; set; }
		public bool IsLocalOnly { get; set; }
		public int? StatusId { get; set; }
		public FunctionalStatus Status { get; set; }
		public int? LocationTypeId { get; set; }
		public LocationType LocationType { get; set; }
		public DateTime? Location_Timestamp { get; set; }
		[StringLength(1024)]
		public string Location_Gps { get; set; }
		public bool IsGpsPublic { get; set; }
		[StringLength(256)]
		public string Location_StreetAddress { get; set; }
		[StringLength(256)]

		public Guid? LocationThingId { get; set; }
		public int? Preferred_LocationTypeId { get; set; }
		public DateTime? PreferredLocation_Timestamp { get; set; }
		[StringLength(1024)]
		public string PreferredLocation_GPS { get; set; }
		[StringLength(256)]
		public string PreferredLocation_StreetAddress { get; set; }
		[StringLength(256)]

		public Guid? PreferredLocationThingId { get; set; }

		#region Navigation Properties
		public ArchetypeThing ArchetypeThing { get; set; }
		public BaseThing LocationThing { get; set; }
		public BaseThing PreferredLocationThing{ get; set; }
		#endregion
	}

	



}
