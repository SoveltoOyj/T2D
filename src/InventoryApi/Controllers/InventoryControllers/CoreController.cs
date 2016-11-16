using InventoryApi.Controllers.BaseControllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using T2D.Entities;
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

			var thing = this.Find(value.ThingId) as T2D.Entities.BaseThing;
			if (thing == null)
				return BadRequest($"Thing '{value.ThingId}' do not exists.");

			// Explicit loading - all ThingRoles for this entity
			dbc.ThingRoles
				.Where(tr => tr.ThingId == thing.Id)
				.Load()
				;

			// explicit loading - select those thingRoleMembers, where ThingRoleMember is one of things in session
			// have to use Lists
			List<Guid> sessionThings = new List<Guid> { session.EntryPoint_ThingId };
			foreach (var item in session.SessionAccesses)
			{
				sessionThings.Add(item.ThingId);
			}
			List<Guid> thingRoles = new List<Guid>();
			thingRoles.AddRange(thing.ThingRoles.Select(tr => tr.Id));

			dbc.ThingRoleMembers
				.Where(trm => sessionThings.Contains(trm.ThingId) && thingRoles.Contains(trm.ThingRoleId))
				.Load()
				;
										
			var ret = new QueryMyRolesResponse
			{
				Roles = new List<string>(),
			};
			var roleIds = new List<int>();

			// add roles and add to SessionAccess
			foreach (var item in thing.ThingRoleMembers)
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
			if (!this.CheckSession(value.Session))
				return BadRequest("Session is not valid.");
			var fqdn = ThingIdHelper.GetFQDN(value.ThingId);
			var us = ThingIdHelper.GetUniqueString(value.ThingId);
			var ret = new GetRelationsResponse();

			if (us == "T1" && value.Role.ToLower() == RoleEnum.Owner.ToString().ToLower())
			{
				ret.RoleThings = new List<GetRelationsResponse.RoleThingsClass> {
				new GetRelationsResponse.RoleThingsClass {Role= RelationEnum.ContainedBy.ToString(),
				Things=new List<GetRelationsResponse.RoleThingsClass.ThingIdTitle> {
					new GetRelationsResponse.RoleThingsClass.ThingIdTitle {
						ThingId = "inventory1.sovelto.fi/T2" ,
						Title = "Container" },
					}
				 }
				};
			}
			else if (value.Role.ToLower() == RoleEnum.Omnipotent.ToString().ToLower() && us != "T1" && us != "T2")
			{
				ret.RoleThings = new List<GetRelationsResponse.RoleThingsClass> {
				new GetRelationsResponse.RoleThingsClass {Role= RelationEnum.Belongings.ToString(),
				Things=new List<GetRelationsResponse.RoleThingsClass.ThingIdTitle> {
					new GetRelationsResponse.RoleThingsClass.ThingIdTitle {
						ThingId = "inventory1.sovelto.fi/T1" ,
						Title = "My Suitcase" },
				}
				},
				new GetRelationsResponse.RoleThingsClass {Role= RelationEnum.RoleIn.ToString(),
					Things=new List<GetRelationsResponse.RoleThingsClass.ThingIdTitle> {
					new GetRelationsResponse.RoleThingsClass.ThingIdTitle {
						ThingId = "inventory1.sovelto.fi/T1" ,
						Title = "My Suitcase" },
				}
				}
				};
			}
			else
			{
			
			}
			return Ok(ret);
		}

		[HttpPost, ActionName("GetAttribute")]
		[Produces(typeof(GetAttributeResponse))]
		public IActionResult GetAttribute([FromBody]GetAttributeRequest value)
		{
			if (!this.CheckSession(value.Session))
				return BadRequest("Session is not valid.");
			var fqdn = ThingIdHelper.GetFQDN(value.ThingId);
			var us = ThingIdHelper.GetUniqueString(value.ThingId);
			GetAttributeResponse ret;
			if (us == "T2" && value.Role.ToLower() == RoleEnum.Belongings.ToString().ToLower())
			{
				 ret = new GetAttributeResponse
				{
					Attribute = value.Attribute.ToString(),
					TimeStamp = DateTime.UtcNow,
					Value = "{\"jokin\":\"jotakin\", \"jokintoinen\":123}",
				};
			}
			else
			{
				 ret = new GetAttributeResponse
				{
					Attribute = value.Attribute.ToString(),
					TimeStamp = DateTime.UtcNow,
					Value = "{\"jokin\":\"Mock ei tiedä vastausta\"}"
				};
			}
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
			Session session = dbc.Sessions.SingleOrDefault(s => s.Id == guid);
			if (session==null) throw new Exception("Session is invalid.");

			if (alsoSessionAccess)
			{
				dbc.SessionAccesses
					.Where(s => s.SessionId == session.Id)
					.Load()
					;
			}

			return session;
		}


		[NonAction]
		private T2D.Entities.IThing Find(string c, string u)
		{
			return dbc.Things.FirstOrDefault(t => t.Fqdn == c && t.US == u);
		}

		[NonAction]
		private T2D.Entities.IThing Find(string thingId)
		{
			return dbc.Things.FirstOrDefault(t => t.Fqdn == ThingIdHelper.GetFQDN(thingId) && t.US == ThingIdHelper.GetUniqueString(thingId));
		}


	}
}
