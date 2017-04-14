using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using T2D.InventoryBL.Mappers;
using InventoryApi.Controllers.BaseControllers;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Reflection;

namespace InventoryApi.Controllers.MetadataControllers
{
	[Route("api/[controller]/[Action]")]
	public class MetadataController : ApiBaseController
	{

		private readonly IHostingEnvironment _env;

		public MetadataController(IHostingEnvironment env)
		{
			_env = env;
		}

		/// <summary>
		/// Get Inventory API version number.
		/// </summary>
		/// <returns>Version number as a string.</returns>
		/// <response code="200">Returns version number.</response>
		[HttpGet(), ActionName("ApiVersion")]
		[Produces(typeof(string))]
		public IActionResult ApiVersion()
		{
			return Ok("1.0.0.0");
		}

		/// <summary>
		/// Get API compatibility to specific version. Not yet implemented!
		/// </summary>
		/// <param name="version">Version to which compatibility is compared to.</param>
		/// <returns>List on functions this Inventory compatible to.</returns>
		/// <response code="200">Returns compatible functions.</response>
		/// <response code="400">Version is not correct.</response>
		[HttpGet(), ActionName("ApiCompatibility")]
		[Produces(typeof(List<string>))]
		public IActionResult ApiCompatibility(string version)
		{
			if (string.IsNullOrWhiteSpace(version)) return BadRequest("version is empty or null.");

			List<string> ret = new List<string>
			{
				"Not yet implemented",
			};
			return Ok(ret);
		}


		/// <summary>
		/// Get all enum names.
		/// </summary>
		/// <returns>List on enum names.</returns>
		/// <response code="200">Returns enum names.</response>
		[HttpGet(), ActionName("EnumNames")]
		[Produces(typeof(List<string>))]
		public IActionResult EnumNames()
		{
			List<string> ret = new List<string>
			{
				"AttributeType",
				"FunctionalStatus",
				"LocationType",
				"Relation",
				"Right",
				"Role",
				"ServiceAndActivityState",
				"ThingStatus",
			};
			return Ok(ret);
		}




	}
}
