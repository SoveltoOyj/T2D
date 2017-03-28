using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using T2D.InventoryBL.Mappers;
using InventoryApi.Controllers.BaseControllers;
using T2D.Model.InventoryApi;
using T2D.Model.Helpers;
using T2D.Entities;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace InventoryApi.Controllers.InventoryControllers
{

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
			if (string.IsNullOrWhiteSpace(value.ThingId)) value.ThingId = "inv1.sovelto.fi/Teemu Testaaja";
			var T0 = dbc.AuthenticationThings.SingleOrDefault(t => t.Fqdn == ThingIdHelper.GetFQDN(value.ThingId) && t.US == ThingIdHelper.GetUniqueString(value.ThingId));
			if (T0 == null)
			{
				T0 = new T2D.Entities.AuthenticationThing
				{
					Fqdn = ThingIdHelper.GetFQDN(value.ThingId),
					US = ThingIdHelper.GetUniqueString(value.ThingId),
					Title = $"User {ThingIdHelper.GetUniqueString(value.ThingId)}",
				};
				dbc.AuthenticationThings.Add(T0);
				dbc.SaveChanges();
			}

			//Create SessionEntity
			var session = new Session
			{
				EntryPoint_ThingId = T0.Id,
				StartTime=DateTime.UtcNow,
			};
			dbc.Sessions.Add(session);
			dbc.SaveChanges();

			var ret = new AuthenticationResponse
			{
				Session = session.Id.ToString(),
			};
			return Ok(ret);
		}
	}
}
