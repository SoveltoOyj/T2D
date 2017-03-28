using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InventoryApi.Extensions;
using T2D.InventoryBL.Mappers;
using T2D.Entities;
using Microsoft.EntityFrameworkCore;
using T2D.Model.Helpers;
using System.Reflection;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace InventoryApi.Controllers.BaseControllers
{
	[WebApiExceptionFilterAttribute]
	public class ApiBaseController : Controller
	{

		protected T2D.Infra.EfContext dbc = new T2D.Infra.EfContext();
		protected EnumMapper<T2D.Entities.RoleEnum, T2D.Entities.Role> RoleMapper = new EnumMapper<T2D.Entities.RoleEnum, T2D.Entities.Role>();
		protected EnumMapper<T2D.Entities.RelationEnum, T2D.Entities.Relation> RelationMapper = new EnumMapper<T2D.Entities.RelationEnum, T2D.Entities.Relation>();
		protected EnumMapper<T2D.Entities.AttributeEnum, T2D.Entities.Attribute> AttributeMapper = new EnumMapper<T2D.Entities.AttributeEnum, T2D.Entities.Attribute>();
		protected EnumMapper<T2D.Entities.StateEnum, T2D.Entities.State> StateMapper = new EnumMapper<T2D.Entities.StateEnum, T2D.Entities.State>();

		protected override void Dispose(bool disposing)
		{
			dbc.Dispose();
			base.Dispose(disposing);
		}


		[NonAction]
		protected bool CheckSession(string sessionId)
		{
			Guid guid;
			if (!Guid.TryParse(sessionId, out guid)) return false;

			//find session from entities
			Session session = dbc.Sessions.SingleOrDefault(s => s.Id == guid);
			return session != null;
		}
		[NonAction]
		protected Session GetSession(string sessionId, bool alsoSessionAccess)
		{
			Guid guid;
			if (!Guid.TryParse(sessionId, out guid)) throw new Exception("Session is invalid.");

			//find session from entities
			var q = dbc.Sessions.Where(s => s.Id == guid);
			if (alsoSessionAccess)
				q = q.Include(s => s.SessionAccesses);

			Session session = q.SingleOrDefault();
			if (session == null) throw new Exception("Session is invalid.");

			return session;
		}


		[NonAction]
		protected T2D.Entities.IThing Find(string c, string u)
		{
			return dbc.Things.FirstOrDefault(t => t.Fqdn == c && t.US == u);
		}

		[NonAction]
		protected IQueryable<T2D.Entities.BaseThing> Find(string thingId)
		{
			return dbc.Things.Where(t => t.Fqdn == ThingIdHelper.GetFQDN(thingId) && t.US == ThingIdHelper.GetUniqueString(thingId));
		}

		[NonAction]
		protected static object GetPropertyValue(object obj, string propertyName)
		{
			var prop = obj.GetType().GetProperties()
				 .SingleOrDefault(pi => pi.Name == propertyName)
				 ;
			if (prop == null) return null;

			return prop.GetValue(obj, null);

		}

	}
}
