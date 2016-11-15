using InventoryApi.Controllers.BaseControllers;
using Microsoft.AspNetCore.Mvc;
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
			if (!this.CheckSession(value.Session))
				return BadRequest("Session is not valid.");
			var ret = new QueryMyRolesResponse();
			var fqdn = ThingIdHelper.GetFQDN(value.ThingId);
			var us = ThingIdHelper.GetUniqueString(value.ThingId);

			if (us == "T1")
			{
				ret.Roles = new List<string> { RoleEnum.Owner.ToString(), RoleEnum.Anonymous.ToString() };
			}
			else if (us == "T2")
			{
				ret.Roles = new List<string> { RoleEnum.Belongings.ToString() };
			}
			else
			{
				ret.Roles = new List<string> { RoleEnum.Omnipotent.ToString() };
			}
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
			return Guid.TryParse(sessionId, out guid);
		}
	}
}
