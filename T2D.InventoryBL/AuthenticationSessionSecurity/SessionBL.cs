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

		private SessionBL(EfContext dbc)
		{
			_dbc = dbc;
		}
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

		/// <summary>
		/// Create a new SessionBL (and Session Entity also).
		/// </summary>
		/// <param name="dbc"></param>
		/// <param name="authenticatedThingId">If Null, this will be anonymous session!</param>
		/// <returns>SessionBL</returns>
		public static SessionBL CreateSessionBLForNewSession(EfContext dbc, Guid? authenticatedThingId)
		{
			SessionBL ret = new SessionBL(dbc);

			var newSession = new Session
			{
				EntryPoint_ThingId = authenticatedThingId==null? new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1):authenticatedThingId.Value ,
				StartTime = DateTime.UtcNow,
				Token="We'll see what this secret token will be."
			};

			dbc.Sessions.Add(newSession);
			dbc.SaveChanges();
			ret.Session = newSession;
			return ret;
		}




		public bool AddSessionAccess(int roleId, Guid thingId)
		{
			if (_dbc.SessionAccesses.Any(sa => sa.RoleId == roleId && sa.ThingId == thingId))
			{
				_dbc.SessionAccesses.Add(new SessionAccess
				{
					SessionId = Session.Id,
					RoleId = roleId,
					ThingId = thingId,
				});
				_dbc.SaveChanges();
			}
			return true;
		}
	}
}
