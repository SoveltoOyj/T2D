using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace T2D.Entities
{
	/// <summary>
	/// Archetype Thing.
	/// </summary>
	public class ArchetypeThing : GenericThing
	{

		#region Navigation Properties
		public List<RegularThing> InstanceThings { get; set; }
		#endregion
	}
}
