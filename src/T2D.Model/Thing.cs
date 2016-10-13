using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T2D.Helpers;

namespace T2D.Model
{
	public class Thing:IThingModel
	{
		public string Id { get; set; }
		public string Title { get; set; }
		public DateTime? Created { get; set; }
		public DateTime? Published { get; set; }
		public DateTime? Modified { get; set; }
		public string Creator { get; set; }
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
		public string Status { get; set; }

		public override string ToString()
		{
			return this.ToJson();
		}
	}
}
