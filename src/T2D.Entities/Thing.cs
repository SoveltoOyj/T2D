using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace T2D.Entities
{
	public class Thing:IEntity
	{
		public long Id { get; set; }
		public string Name { get; set; }
		public double Version { get; set; }

	}
}
