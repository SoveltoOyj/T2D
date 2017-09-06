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
		public string PreferredLocation_Gps { get; set; }
		[StringLength(256)]
		public string PreferredLocation_StreetAddress { get; set; }
		[StringLength(256)]
		public Guid? PreferredLocationThingId { get; set; }

		#region Physical Properties
		public int? EAN { get; set; }
		public double? WeightKg { get; set; }
		public double? VolumeLiters { get; set; }
		public double? ContainerVolumeLiters { get; set; }
		public double? MaxSurfaceDimensions_X { get; set; }
		public double? MaxSurfaceDimensions_Y { get; set; }
		public double? MaxSurfaceDimensions_Z { get; set; }
		public double? FreeContainerDimensions_X { get; set; }
		public double? FreeContainerDimensions_Y { get; set; }
		public double? FreeContainerDimensions_Z { get; set; }
		public double? MaxFluidLevel { get; set; }
		public double? CurrentFluidLevel { get; set; }
		public double? LoadabilityOnTopKg { get; set; }
		public double? LoadabilityWithinKg { get; set; }
		// Multivalue: easy/requires packaging/requires extra care/security considerations/hygienic considerations
		public LuggabilityFlag? Luggability { get; set; }
		public int? NumberLooseParts { get; set; }
		public double? InsideTemperatureRangeMin { get; set; }
		public double? InsideTemperatureRangeMax { get; set; }
		public double? InsideTemperatureCurrent { get; set; }
		public double? OutsideTemperatureRangeMin { get; set; }
		public double? OutsideTemperatureRangeMax { get; set; }
		public double? OutsideTemperatureCurrent { get; set; }
		// Values: no/moderate/good/excellent
		public Weatherproofness Weatherproofness { get; set; }
		public double? MaxGForce { get; set; }
		[StringLength(256)]
		public string Color { get; set; }
		public DateTime? AttentionDate { get; set; }
		// Values: (biological, chemical, electromagnetic, robotic, alive, neutral
		public ObjectActivityClass ObjectActivityClass { get; set; }
		public bool HazardousCargo { get; set; }
		public string SpecialConsideration { get; set; }
		// Three IoT-fields:
		[StringLength(256)]
		public string IoTField1_Title { get; set; }
		[StringLength(256)]
		public string IoTField1_SIUnit { get; set; }
		public double? IoTField1_Data { get; set; }
		[StringLength(256)]
		public string IoTField2_Title { get; set; }
		[StringLength(256)]
		public string IoTField2_SIUnit { get; set; }
		public double? IoTField2_Data { get; set; }
		[StringLength(256)]
		public string IoTField3_Title { get; set; }
		[StringLength(256)]
		public string IoTField3_SIUnit { get; set; }
		public double? IoTField3_Data { get; set; }
		#endregion

		#region Navigation Properties
		public ArchetypeThing ArchetypeThing { get; set; }
		public BaseThing LocationThing { get; set; }
		public BaseThing PreferredLocationThing { get; set; }
		#endregion
	}





}
