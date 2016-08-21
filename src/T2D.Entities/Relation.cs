using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace T2D.Entities
{
	/// <summary>
	/// Relation names. Static list.
	/// ThingRelation will be read only from ThingId1 -> ThingId2, not vice versa.
	/// </summary>
	public class Relation:IEntity
	{
		public long Id { get; set; }
		public string Name { get; set; }
	}

	public enum RelationEnum
	{
		RoleIn = 1,
		Contains,
		ContainedBy,
		Inherits,
		Orginates,
	}	;
}
