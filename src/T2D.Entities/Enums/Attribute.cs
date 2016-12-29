using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using T2D.Helpers;

namespace T2D.Entities
{
	public class Attribute : IEnumEntity
	{

		public int Id { get; set; }
        [StringLength(256)]
		public string Name { get; set; }
        [StringLength(1024)]
        public string Pattern { get; set; }
        [StringLength(256)]
        public string MinValue { get; set; }
        [StringLength(256)]
        public string MaxValue { get; set; }

		public override string ToString()
		{
			return this.ToJson();
		}
	}
	public enum AttributeEnum
	{
		Relations = 1,
		Id,
		Fqdn,
		US,
		Title,
		Description,
		Created,
		Published,
		Modified,
		Creator_Fqdn,
		Crator_US,
		Parted_Fqdn,
		Parted_US,
		Logging,
		IsLocalOnly,
        StatusId,
		LocationTypeId,
		Location_Timestamp,
		Location_GPS,
		Location_StreetAddress,
		Location_Id,
		PreferredLocationTypeId,
		PreferredLocation_Timestamp,
		PreferredLocation_GPS,
		PreferredLocation_StreetAddress,
		PreferredLocation_Id,
	};
}
