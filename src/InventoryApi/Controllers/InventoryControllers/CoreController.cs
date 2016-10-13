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
	[Route("api/inventory/[controller]/[action]")]
	public class CoreController : ApiBaseController
	{
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
				ThingIds = new List<string> {
							"inventory1.sovelto.fi/T1" ,
							"inventory1.sovelto.fi/T2" 
						}
				},
				new GetRelationsResponse.RoleThingsClass {Role= RelationEnum.RoleIn.ToString(),
				ThingIds = new List<string> {
							"inventory1.sovelto.fi/T1",
							"inventory1.sovelto.fi/T2" 
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
