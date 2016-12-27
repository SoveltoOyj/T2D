using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T2D.Helpers;

namespace T2D.Model
{
	public class BaseThing:IThing
	{
		public string Id { get; set; }
		public string Title { get; set; }
		public DateTime? Created { get; set; }
		public DateTime? Published { get; set; }
		public DateTime? Modified { get; set; }
		public string Creator { get; set; }
		public string Status { get; set; }

		public override string ToString()
		{
			return this.ToJson();
		}
	}
}
