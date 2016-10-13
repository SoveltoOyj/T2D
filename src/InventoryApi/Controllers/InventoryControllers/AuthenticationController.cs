using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using T2D.InventoryBL.Mappers;
using InventoryApi.Controllers.BaseControllers;
using T2D.Model.InventoryApi;
using T2D.Model.Helpers;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace InventoryApi.Controllers.InventoryControllers
{

	//https://docs.asp.net/en/latest/mvc/models/formatting.html#content-negotiation
	//https://wildermuth.com/2016/05/10/Writing-API-Controllers-in-ASP-NET-MVC-6

	[Route("api/inventory/[controller]")]
	public class AuthenticationController : ApiBaseController
	{
		public AuthenticationController() : base()
		{
		}

		[HttpPost]
		public IActionResult Post([FromBody]AuthenticationRequest value)
		{
			// mock, AuthenticationThing is created if not exists
			if (string.IsNullOrWhiteSpace(value.ThingId)) value.ThingId = "inventory1.sovelto.fi/Teemu Testaaja";
			var T0 = dbc.AuthenticationThings.SingleOrDefault(t => t.Id_CreatorUri == ThingIdHelper.GetFQHN(value.ThingId) && t.Id_UniqueString == ThingIdHelper.GetUniqueString(value.ThingId));
			if (T0 == null)
			{
				dbc.AuthenticationThings.Add(new T2D.Entities.AuthenticationThing
				{
					Id_CreatorUri = ThingIdHelper.GetFQHN(value.ThingId),
					Id_UniqueString = ThingIdHelper.GetUniqueString(value.ThingId),
					Title=$"User {ThingIdHelper.GetUniqueString(value.ThingId)}",
				});
				dbc.SaveChanges();
			}

			var ret = new AuthenticationResponse
			{
				Session = Guid.NewGuid().ToString(),
			};
			return Ok(ret);
		}
	}
}
