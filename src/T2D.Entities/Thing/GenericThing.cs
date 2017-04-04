using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace T2D.Entities
{
	/// <summary>
	/// Base Class of all real Things including ArchetypeThing
	/// </summary>
	public class GenericThing : BaseThing, IInventoryThing
	{
		[StringLength(1024)]
		public string Title { get; set; }

		[StringLength(4000)]
		public string Description { get; set; }

		public DateTime Created { get; set; }
		public DateTime? Published { get; set; }
		public DateTime Modified { get; set; }

		/// <summary>
		/// If Thing is created in this Inventory, Creator AuthenticationThingId is required.
		/// </summary>
		public Guid? CreatorThingId { get; set; }

		public Guid? PartedThingId { get; set; }

		public ThingStatus ThingStatus { get; set; }


		#region Navigation Properties
		public AuthenticationThing CreatorThing { get; set; }

		public GenericThing PartedThing { get; set; }
		public List<ServiceDefinition> ServiceDefinitions { get; set; }
		public List<GenericThing> Parts { get; set; }

		#endregion

		public GenericThing()
		{
			ServiceDefinitions = new List<ServiceDefinition>();
			Parts = new List<GenericThing>();
		}
	}
}
