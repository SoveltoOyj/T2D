using System;
using System.Collections.Generic;
using System.Text;
using T2D.Helpers;

namespace T2D.Model
{
    public class State : IEnumModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public override string ToString()
		{
			return this.ToJson();
		}
	}
}
