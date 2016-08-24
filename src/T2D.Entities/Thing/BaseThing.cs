using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace T2D.Entities
{
	public class BaseThing:IThingEntity
	{
		public string Id_CreatorUri { get; set; }
		public string Id_UniqueString { get; set; }
		public List<ThingRelation> ThingRelations { get; set; }

		public BaseThing()
		{
			ThingRelations = new List<ThingRelation>();
		}


	}
}
