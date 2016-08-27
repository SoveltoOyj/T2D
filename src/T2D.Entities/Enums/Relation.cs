using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using T2D.Helpers;

namespace T2D.Entities
{
	/// <summary>
	/// Relation names. Static list.
	/// ThingRelation will be read only from ThingId1 -> ThingId2, not vice versa.
	/// </summary>
	public class Relation:IEnumEntity
	{
		public long Id { get; set; }
		public string Name { get; set; }
		public override string ToString()
		{
			return this.ToJson();
		}
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
