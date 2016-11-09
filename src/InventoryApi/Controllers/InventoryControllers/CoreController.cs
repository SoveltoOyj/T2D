using InventoryApi.Controllers.BaseControllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using T2D.Entities;
using T2D.Model;
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
		/// Query my roles by which I can get attributes
		/// </summary>
		/// <param name="value">Request argument</param>
		/// <returns>Available roles. Later propably also rights that role has.</returns>
		[HttpPost, ActionName("QueryMyRoles")]
		[Produces(typeof(QueryMyRolesResponse))]
		public IActionResult QueryMyRoles([FromBody]QueryMyRolesRequest value)
		{
			var ret = new QueryMyRolesResponse();
			ret.Roles = new List<string> { RoleEnum.Owner.ToString(), RoleEnum.Anonymous.ToString() };
			return Ok(ret);
		}

		[HttpPost, ActionName("GetRelations")]
		[Produces(typeof(GetRelationsResponse))]
		public IActionResult GetRelations([FromBody]GetRelationsRequest value)
		{
			var ret = new GetRelationsResponse();
			ret.RoleThings = new List<GetRelationsResponse.RoleThingsClass> {
				new GetRelationsResponse.RoleThingsClass {Role= RelationEnum.Belongings.ToString(),
				Things=new List<GetRelationsResponse.RoleThingsClass.ThingIdTitle> {
					new GetRelationsResponse.RoleThingsClass.ThingIdTitle {
						ThingId = "inventory1.sovelto.fi/T1" ,
						Title = "My suitcase" },
					new GetRelationsResponse.RoleThingsClass.ThingIdTitle {
						ThingId = "inventory1.sovelto.fi/T2" ,
						Title = "Container"
					}
				 }
				},
				new GetRelationsResponse.RoleThingsClass {Role= RelationEnum.RoleIn.ToString(),
				Things=new List<GetRelationsResponse.RoleThingsClass.ThingIdTitle> {
					new GetRelationsResponse.RoleThingsClass.ThingIdTitle {
						ThingId = "inventory1.sovelto.fi/T1" ,
						Title = "My suitcase" },
					new GetRelationsResponse.RoleThingsClass.ThingIdTitle {
						ThingId = "inventory1.sovelto.fi/T2" ,
						Title = "Container"
					}
				 }
				},
				};
			return Ok(ret);
		}

		[HttpPost, ActionName("GetAttribute")]
		[Produces(typeof(GetAttributeResponse))]
		public IActionResult GetAttribute([FromBody]GetAttributeRequest value)
		{
			var ret = new GetAttributeResponse
			{
				Attribute = value.Attribute.ToString(),
				TimeStamp = DateTime.UtcNow,
				Value = "{\"jokin\":\"jotakin\", \"jokintoinen\":123}",
			};
			return Ok(ret);
		}
	}
}
