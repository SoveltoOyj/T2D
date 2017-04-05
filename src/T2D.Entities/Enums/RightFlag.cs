using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace T2D.Entities
{
	[Flags]
	public enum RightFlag
	{
		Create = 1,
		Read = 2,
		Update = 4,
		Delete = 8,
		Execute = 16,
	}
}
