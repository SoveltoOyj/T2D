using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using T2D.InventoryBL.Mappers;
using InventoryApi.Controllers.BaseControllers;

namespace InventoryApi.Controllers.MetadataControllers
{
	[Route("api/metadata/[controller]/[Action]")]
	public class VersionController : ApiBaseController
	{
		/// <summary>
		/// Get Inventory API version number.
		/// </summary>
		/// <returns>Version number as a string.</returns>
		/// <response code="200">Returns version number.</response>
		[HttpGet(), ActionName("ApiVersion")]
		[Produces(typeof(string))]
		public IActionResult Get()
		{
			return Ok("1.0.0.0");
		}

		/// <summary>
		/// Get API compatibility to specific version.
		/// </summary>
		/// <param name="version">Version to which compatibility is compared to.</param>
		/// <returns>List on functions this Inventory compatible to.</returns>
		/// <response code="200">Returns compatible functions.</response>
		/// <response code="400">Version is not correct.</response>
		[HttpGet(), ActionName("ApiCompatibility")]
		[Produces(typeof(List<string>))]
		public IActionResult GetApiCompatibility(string version)
		{
			if (string.IsNullOrWhiteSpace(version)) return BadRequest("version is empty or null.");

			List<string> ret = new List<string>
			{
				"Not yet implemented",
			};
			return Ok(ret);
		}


	}
}
