using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace T2D.Model
{
	public class Thing:IModel
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public double Version { get; set; }
	}
}
