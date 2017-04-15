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

	[Route("api/inventory/[controller]/[action]")]
	public class AuthenticationController : ApiBaseController
	{
		public AuthenticationController() : base()
		{
		}

		/// <summary>
		/// Enter authenticated session.
		/// </summary>
		/// <param name="value">Request argument</param>
		/// <response code="200">A new Session was created and its id is returned.</response>
		/// <response code="400">Bad request, like Thing Id is not OK.</response>
		[HttpPost, ActionName("EnterAuthenticatedSession")]
		[Produces(typeof(AuthenticationResponse))]
		public IActionResult EnterAuthenticatedSession([FromBody]AuthenticationRequest value)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);
			// mock, AuthenticationThing is created if not exists
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

		[HttpPost, ActionName("EnterAnonymousSession")]
		[Produces(typeof(AuthenticationResponse))]
		public IActionResult EnterAnonymousSession()
		{
			//Create SessionEntity
			var session = new Session
			{
				EntryPoint_ThingId = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1),
				StartTime = DateTime.UtcNow,
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
