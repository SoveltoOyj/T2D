using System;
using System.Collections.Generic;
using System.Text;

namespace T2D.Entities
{

	public enum AttributeEnum
	{
		//artifical "attributes"
		Relations = 1,  // contains both ConnectTO and ConnectFROM
		ServiceDefinition,
		Activity,  //both assignTo "nakitus" and object "kohde" 

		//attributes
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
		LocationType,
		Location_Timestamp,
		Location_GPS,
		Location_StreetAddress,
		Location,
		PreferredLocationType,
		PreferredLocation_Timestamp,
		PreferredLocation_GPS,
		PreferredLocation_StreetAddress,
		PreferredLocation,
	};

	public enum AttributeType
	{
		T2DAttribute,
		Extension,
	}
}
