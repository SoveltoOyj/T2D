using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace T2D.Entities
{
	public class ThingBase:IEntity
	{
		public long Id { get; set; }
		public string Name { get; set; }

		public List<ThingRelation> ThingRelations { get; set; }

		public ThingBase()
		{
			ThingRelations = new List<ThingRelation>();
		}
	}
}
