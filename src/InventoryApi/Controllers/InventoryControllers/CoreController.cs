using InventoryApi.Controllers.BaseControllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using T2D.Entities;
using T2D.Helpers;
using T2D.InventoryBL;
using T2D.Model;
using T2D.Model.Helpers;
using T2D.Model.InventoryApi;

namespace InventoryApi.Controllers.InventoryControllers
{
	/// <summary>
	/// Core 1 operations, currently only MOCS
	/// </summary>
	[Route("api/inventory/[controller]/[action]")]
	public class CoreController : ApiBaseController
	{
		/// <summary>
		/// Query my roles.
		/// </summary>
		/// <param name="value">Request argument</param>
		/// <returns>Available roles.</returns>
		[HttpPost, ActionName("QueryMyRoles")]
		[Produces(typeof(QueryMyRolesResponse))]
		public IActionResult QueryMyRoles([FromBody]QueryMyRolesRequest value)
		{
			var session = this.GetSession(value.Session, true);

			T2D.Entities.BaseThing thing = 
				this.Find(value.ThingId) 
				.Include(t=>t.ThingRoles)
				.FirstOrDefault()
				;

			if (thing == null)
				return BadRequest($"Thing '{value.ThingId}' do not exists.");

			//TODO: check that session has right to 
			if (!AttributeSecurity.QueryMyRolesRight(thing, session))
				return BadRequest($"Not enough priviledges to query roles for thing {value.ThingId}.");


			// explicit loading - select those thingRoleMembers, where ThingRoleMember is one of things in session
			// have to use Lists
			List<Guid> sessionThings = new List<Guid> { session.EntryPoint_ThingId };
			foreach (var item in session.SessionAccesses)
			{ 
				if (!sessionThings.Contains(item.ThingId))
					sessionThings.Add(item.ThingId);
			}
			List<Guid> thingRoles = new List<Guid>();
			thingRoles.AddRange(thing.ThingRoles.Select(tr => tr.Id));

			var thingRoleMembers = 
				dbc.ThingRoleMembers
					.Where(trm => sessionThings.Contains(trm.ThingId) && thingRoles.Contains(trm.ThingRoleId))
					.ToList()
					;
										
			var ret = new QueryMyRolesResponse
			{
				Roles = new List<string>(),
			};
			var roleIds = new List<int>();

			// add roles and add to SessionAccess
			foreach (var item in thingRoleMembers)
			{
				if (item.ThingRole != null)
				{
					int roleId = item.ThingRole.RoleId;
					if (!roleIds.Contains(roleId)) roleIds.Add(roleId);
					if (!session.SessionAccesses.Any(sa=>sa.RoleId==roleId && sa.ThingId == thing.Id))
					{
						dbc.SessionAccesses.Add(new SessionAccess { RoleId = roleId, SessionId = session.Id, ThingId = thing.Id });
					}
				}	
			}
			dbc.SaveChanges();
			ret.Roles.AddRange(
				dbc.Roles
					.Where(r => roleIds.Contains(r.Id))
					.Select(r => r.Name)
					.ToList()
				);


			return Ok(ret);
		}

		[HttpPost, ActionName("GetRelations")]
		[Produces(typeof(GetRelationsResponse))]
		public IActionResult GetRelations([FromBody]GetRelationsRequest value)
		{
			var session = this.GetSession(value.Session, true);

			T2D.Entities.BaseThing thing =
				this.Find(value.ThingId)
				.Include(t => t.ThingAttributes)
				.Include(t => t.ThingRelations)
				.FirstOrDefault()
				;

			if (thing == null)
				return BadRequest($"Thing '{value.ThingId}' do not exists.");

			var role = this.RoleMapper.EnumToEntity(value.Role);

			//TODO: check that session has right to 
			if (!AttributeSecurity.QueryRelationsRight(thing, session, role))
				return BadRequest($"Not enough priviledges to query relations for thing {value.ThingId}.");


			var ret = new GetRelationsResponse();
			ret.RelationThings = new List<GetRelationsResponse.RelationsThings>();

			foreach (var group in thing.ThingRelations.GroupBy(tr=>tr.RelationId))
			{
				var rt = new GetRelationsResponse.RelationsThings
				{
					Relation = RelationMapper.FromEntityId(group.Key).ToString(),
					Things = new List<GetRelationsResponse.RelationsThings.IdTitle>(),
				};
				foreach(var th in group)
				{
					var thingIdTitle = new GetRelationsResponse.RelationsThings.IdTitle { ThingId = ThingIdHelper.Create(th.Thing2_Fqdn, th.Thing2_US) };
					//TODO: it should be local in this version
					if (th.Thing2IsLocal || !th.Thing2IsLocal)
					{
						var thing2 = this.Find(th.Thing2_Fqdn, th.Thing2_US);
						if (thing2 != null)
							thingIdTitle.Title = thing2.Title;
					}
					rt.Things.Add(thingIdTitle);
				}
				ret.RelationThings.Add(rt);
			}
			return Ok(ret);
		}

		[HttpPost, ActionName("GetAttribute")]
		[Produces(typeof(GetAttributeResponse))]
		public IActionResult GetAttribute([FromBody]GetAttributeRequest value)
		{
			var session = this.GetSession(value.Session, true);

			T2D.Entities.BaseThing thing =
				this.Find(value.ThingId)
				.Include(t => t.ThingAttributes)
				.FirstOrDefault()
				;

			if (thing == null)
				return BadRequest($"Thing '{value.ThingId}' do not exists.");

			var role = this.RoleMapper.EnumToEntity(value.Role);
			var attribute = this.AttributeMapper.EnumToEntity(value.Attribute);

			GetAttributeResponse ret = new GetAttributeResponse
			{
				Attribute = attribute.Name,
				TimeStamp = DateTime.UtcNow,
				Value=GetPropertyValue(thing,attribute.Name)
			};
			return Ok(ret);
		}

		[NonAction]
		private bool CheckSession(string sessionId)
		{
			Guid guid;
			if (!Guid.TryParse(sessionId, out guid)) return false;

			//find session from entities
			Session session = dbc.Sessions.SingleOrDefault(s => s.Id == guid);
			return session != null;
		}
		[NonAction]
		private Session GetSession(string sessionId, bool alsoSessionAccess)
		{
			Guid guid;
			if (!Guid.TryParse(sessionId, out guid)) throw new Exception("Session is invalid.");

			//find session from entities
			var q = dbc.Sessions.Where(s => s.Id == guid);
			if (alsoSessionAccess)
				q = q.Include(s => s.SessionAccesses);

			Session session = q.SingleOrDefault();
			if (session==null) throw new Exception("Session is invalid.");

			return session;
		}


		[NonAction]
		private T2D.Entities.IThing Find(string c, string u)
		{
			return dbc.Things.FirstOrDefault(t => t.Fqdn == c && t.US == u);
		}

		[NonAction]
		private IQueryable<T2D.Entities.BaseThing> Find(string thingId)
		{
			return dbc.Things.Where(t => t.Fqdn == ThingIdHelper.GetFQDN(thingId) && t.US == ThingIdHelper.GetUniqueString(thingId));
		}

		[NonAction]
		public static object GetPropertyValue(object obj, string propertyName)
		{
			var prop =  obj.GetType().GetProperties()
				 .SingleOrDefault(pi => pi.Name == propertyName)
				 ;
			if (prop == null) return null;

			return prop.GetValue(obj, null);

		}

	}
}
