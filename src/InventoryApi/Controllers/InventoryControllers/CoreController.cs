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
		/// <response code="200">Returns available roles.</response>
		/// <response code="400">Bad request, like Thing do not exists or not enough priviledges.</response>
		[HttpPost, ActionName("QueryMyRoles")]
		[Produces(typeof(QueryMyRolesResponse))]
		public IActionResult QueryMyRoles([FromBody]QueryMyRolesRequest value)
		{
			var session = this.GetSession(value.Session, true);

			T2D.Entities.BaseThing thing = 
				this.Find<T2D.Entities.BaseThing>(value.ThingId) 
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
			List<Guid> sessionThings = new List<Guid>();
				sessionThings.Add(session.EntryPoint_ThingId);

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
				this.Find<T2D.Entities.BaseThing>(value.ThingId)
				.Include(t => t.ThingAttributes)
				.Include(t => t.ToThingRelations)
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

			foreach (var group in thing.ToThingRelations.GroupBy(tr=>tr.RelationId))
			{
				var rt = new GetRelationsResponse.RelationsThings
				{
					Relation = RelationMapper.FromEntityId(group.Key).ToString(),
					Things = new List<GetRelationsResponse.RelationsThings.IdTitle>(),
				};
				foreach(var th in group)
				{
					var thing2 = this.Find<T2D.Entities.BaseThing>(th.ToThingId);
					var thingIdTitle = new GetRelationsResponse.RelationsThings.IdTitle {
						ThingId = ThingIdHelper.Create(thing2.Fqdn, thing2.US)
					};
						if (thing2 != null && thing2 is IInventoryThing)
							thingIdTitle.Title = ((IInventoryThing)thing2).Title;
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
				this.Find<T2D.Entities.BaseThing>(value.ThingId)
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

	}
}
