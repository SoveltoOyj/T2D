using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace T2D.Entities
{
	public class RegularThing : GenericThing
	{
		//height and width are stored in millimeters (just an example)
		public long Heightmm { get; set; }
		public long Widthmm { get; set; }
		public bool Logging { get; set; }
		public bool IsLocalOnly { get; set; }


	}
}
