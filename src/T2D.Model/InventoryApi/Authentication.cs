using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using T2D.Model.Enums;

namespace T2D.Model.InventoryApi
{
	public class AuthenticationRequest
	{
		public string ThingId { get; set; }
		public AuthenticationType AuthenticationType { get; set; }
	}

	public class AuthenticationResponse
	{
		public string Session { get; set; }
	}

	
}
