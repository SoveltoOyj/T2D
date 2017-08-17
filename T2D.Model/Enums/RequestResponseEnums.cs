using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace T2D.Model.Enums
{
	public enum AuthenticationType
	{
		Facebook=1,
		Google,
	}

		/// <summary>
	/// All non-abstract Thing Types (inheritance) that are used in API
	/// </summary>
	public enum ThingType
	{
		RegularThing = 1,
		AliasThing,
		ArchetypeThing,
		AuthenticationThing,
		IoTThing,
		WalletThing,
	}

	/// <summary>
	/// All non-abstract ActionTypes (inheritance) that are used in the API
	/// </summary>
	public enum ActionType
	{
		GenericAction = 1,
		//PaymentRequestAction,
		//ReceiptRequestAction,
		//IoTBotRequestAction,
		//ServiceRequestAction,
	}
}
