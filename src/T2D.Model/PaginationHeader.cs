using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using T2D.Helpers;

namespace T2D.Model
{
	public class PaginationHeader
	{
		public int CurrentPage { get; set; }
		public int PageSize { get; set; }
		public long TotalCount { get; set; }
		public bool MorePages { get; set; }
		public override string ToString()
		{
			return this.ToJson();
		}
	}
}
