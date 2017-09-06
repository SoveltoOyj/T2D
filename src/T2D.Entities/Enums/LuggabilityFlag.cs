using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace T2D.Entities
{
	[Flags]
	public enum LuggabilityFlag
	{
		Easy = 1,
		RequiresPackaging = 2,
		RequiresExtraCare = 4,
		SecurityConsiderations = 8,
		HygienicConsiderations = 16
	}
}
