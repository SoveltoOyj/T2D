using System;
using System.Collections.Generic;
using System.Text;
using T2D.Entities;
using T2D.Infra;
using T2D.Model.Enums;
using T2D.Model.Helpers;

namespace T2D.InventoryBL
{
	public class AuthenticationBL
	{
		private readonly EfContext _dbc;

		public static AuthenticationBL CreateAuthenticationBL(EfContext dbc)
		{
			AuthenticationBL ret = new AuthenticationBL(dbc);
			return ret;
		}

		private AuthenticationBL(EfContext dbc)
		{
			_dbc = dbc;
		}


		public SessionBL EnterAuthenticatedSession(out string errMsg, string thingId, string title, string email)
		{
			errMsg = "";

			// mock, AuthenticationThing is created if not exists
			var T0 = _dbc.FindThing<BaseThing>(thingId);
			if (T0 == null)
			{
				T0 = new T2D.Entities.AuthenticationThing
				{
					Fqdn = ThingIdHelper.GetFQDN(thingId),
					US = ThingIdHelper.GetUniqueString(thingId),
					Title = title,
					EMail= email,
				};
				_dbc.AuthenticationThings.Add(T0 as AuthenticationThing);
				_dbc.SaveChanges();
			}
			else if (!(T0 is AuthenticationThing))
			{
				errMsg = $"Authentication Thing {thingId} is not an authentication thing.";
				return null;
			}

			SessionBL sessionBL = SessionBL.CreateSessionBLForNewSession(_dbc, T0.Id);
			return sessionBL;
		}

		public SessionBL EnterAnonymousSession(out string errMsg)
		{
			errMsg = "";

			SessionBL sessionBL = SessionBL.CreateSessionBLForNewSession(_dbc, null);
			return sessionBL;

		}
	}
}
