using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InventoryApi.Controllers.BaseControllers;
using T2D.Model.InventoryApi;
using T2D.Model.Helpers;
using T2D.Entities;
using T2D.InventoryBL;
using Microsoft.AspNetCore.Authorization;
using T2D.Infra;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace InventoryApi.Controllers.InventoryControllers
{

	[Route("api/inventory/[controller]/[action]")]
	public class AuthenticationController : ApiBaseController
	{
		public AuthenticationController(EfContext dbc): base(dbc) { }


		[Authorize]
		[HttpGet]
		public IEnumerable<string> Get()
		{
			List<string> claims = new List<string>();
			foreach (var item in User.Claims)
			{
				claims.Add(item.ToString());
			}
			return claims;
		}


		/// <summary>
		/// Enter authenticated session.
		/// Note: this is MOCK version, enter will allways succeed. New Authenticated user will be created if it does not exists.
		/// </summary>
		/// <param name="value">Request argument</param>
		/// <response code="200">A new Session was created and its id is returned.</response>
		/// <response code="400">Bad request, like Thing Id is not OK.</response>
		[HttpPost, ActionName("EnterAuthenticatedSession")]
		[Produces(typeof(AuthenticationResponse))]
		public IActionResult EnterAuthenticatedSession([FromBody]AuthenticationRequest value)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);
			string errMsg = null;
			AuthenticationBL authenticationBL = AuthenticationBL.CreateAuthenticationBL(_dbc);
			var sessionBL = authenticationBL.EnterAuthenticatedSession(out errMsg, value.ThingId, value.AuthenticationType);
			if (sessionBL == null) return BadRequest(errMsg);

			var ret = new AuthenticationResponse
			{
				Session = sessionBL.Session.Id.ToString(),
			};
			return Ok(ret);
		}

		/// <summary>
		/// Enter Anonymous session.
		/// </summary>
		/// <response code="200">A new Session was created and its id is returned.</response>
		[HttpPost, ActionName("EnterAnonymousSession")]
		[Produces(typeof(AuthenticationResponse))]
		public IActionResult EnterAnonymousSession()
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);
			string errMsg = null;
			AuthenticationBL authenticationBL = AuthenticationBL.CreateAuthenticationBL(_dbc);
			var sessionBL = authenticationBL.EnterAnonymousSession(out errMsg);
			if (sessionBL == null) return BadRequest(errMsg);

			var ret = new AuthenticationResponse
			{
				Session = sessionBL.Session.Id.ToString(),
			};
			return Ok(ret);

		}

	}
}
