using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using T2D.Entities;
using T2D.Infra;

namespace T2D.InventoryBL
{
	public class SessionBL
	{
		private readonly EfContext _dbc;
		public Session Session {get; private set;}

		public static SessionBL CreateSessionBLForExistingSession(EfContext dbc, string sessionId)
		{
			SessionBL ret = new SessionBL(dbc);

			Guid guid;
			if (!Guid.TryParse(sessionId, out guid)) return null;

			//find session from entities
			var q = dbc.Sessions
				.Include(s => s.SessionAccesses)
				.Where(s => s.Id == guid)
				;

			ret.Session = q.SingleOrDefault();
			if (ret.Session == null) return null;

			return ret;
		}

		public static SessionBL CreateSessionBLForNewSession(EfContext dbc, Guid authenticatedThingId)
		{
			SessionBL ret = new SessionBL(dbc);

			var newSession = new Session
			{
				EntryPoint_ThingId = authenticatedThingId,
				StartTime = DateTime.UtcNow,
				Token="We'll see what this secret token will be."
			};

			dbc.Sessions.Add(newSession);
			dbc.SaveChanges();
			ret.Session = newSession;
			return ret;
		}



		private SessionBL(EfContext dbc)
		{
			_dbc = dbc;
		}

		public bool AddSessionAccess(int roleId, Guid thingId)
		{
			_dbc.SessionAccesses.Add(new SessionAccess
			{
				SessionId=Session.Id,
				RoleId=roleId,
				ThingId = thingId,
			});
			_dbc.SaveChanges();
			return true;
		}
	}
}
