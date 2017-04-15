using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using T2D.Entities;
using T2D.Infra;

namespace T2D.InventoryBL.Thing
{
	public class SessionBL
	{
		private readonly EfContext _dbc;
		private Session _session;

		public static SessionBL CreateSessionBL(EfContext dbc, string sessionId)
		{
			SessionBL ret = new SessionBL(dbc);

			Guid guid;
			if (!Guid.TryParse(sessionId, out guid)) return null;

			//find session from entities
			var q = dbc.Sessions
				.Include(s => s.SessionAccesses)
				.Where(s => s.Id == guid)
				;

			ret._session = q.SingleOrDefault();
			if (ret._session == null) return null;

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
				SessionId=_session.Id,
				RoleId=roleId,
				ThingId = thingId,
			});
			_dbc.SaveChanges();
			return true;
		}
	}
}
