using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using T2D.Helpers;

namespace T2D.Model
{
	public class Attribute : IEnumModel
	{
		public long Id { get; set; }
		public string Name { get; set; }
		public string Pattern { get; set; }
		public string MinValue { get; set; }
		public string MaxValue { get; set; }
		public override string ToString()
		{
			return this.ToJson();
		}
	}
}
