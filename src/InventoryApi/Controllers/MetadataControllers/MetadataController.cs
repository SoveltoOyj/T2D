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
using T2D.InventoryBL.Metadata;
using T2D.Model;
using System.ComponentModel.DataAnnotations;

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
		public IActionResult ApiCompatibility([Required]string version)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);
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
			return Ok(_enumBL.ApiEnumNames());
		}


		/// <summary>
		/// Get enum values of an enum.
		/// </summary>
		/// <returns>List on enum values.</returns>
		/// <param name="enumName">Name of an enum.</param>
		/// <response code="200">Returns enum values.</response>
		/// <response code="400">If for example enumName is null or not an enum name.</response>
		[HttpGet(), ActionName("EnumValues")]
		[Produces(typeof(List<ModelEnum>))]
		public IActionResult EnumValues([FromQuery] [Required] string enumName)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);
			if (string.IsNullOrWhiteSpace(enumName)) return BadRequest("Enum name is empty or null.");
			if (!_enumBL.ApiEnumNames().Contains(enumName)) return BadRequest($"{enumName} is not a valid EnumName.");

			switch (enumName.ToLower())
			{
				case "attributetype":
					return Ok(_enumBL.ApiEnumValuesFromEnum<T2D.Entities.AttributeType>());
				case "functionalstatus":
					return Ok(_enumBL.ApiEnumValuesFromEnumEntity(_dbc.Status.AsQueryable()));
				case "locationtype":
					return Ok(_enumBL.ApiEnumValuesFromEnumEntity(_dbc.LocationTypes.AsQueryable()));
				case "relation":
					return Ok(_enumBL.ApiEnumValuesFromEnumEntity(_dbc.Relations.AsQueryable()));
				case "right":
					return Ok(_enumBL.ApiEnumValuesFromEnum<T2D.Entities.RightFlag>());
				case "role":
					return Ok(_enumBL.ApiEnumValuesFromEnumEntity(_dbc.Roles.AsQueryable()));
				case "serviceandactivitystate":
					return Ok(_enumBL.ApiEnumValuesFromEnum<T2D.Entities.ServiceAndActitivityState>());
				case "thingstatus":
					return Ok(_enumBL.ApiEnumValuesFromEnum<T2D.Entities.ThingStatus>());
				case "attribute":
					//Note: attribute has GUID ID, thats why this code.
					var q = _dbc.Attributes
								.Where(a => a.AttributeType == T2D.Entities.AttributeType.T2DAttribute)
								.ToList()
								.Select(a => new { GuidArray = a.Id.ToByteArray(), Name = a.Title })
								;

					List<MyIEnumEntity> values = new List<MyIEnumEntity>(q.Count());
					foreach (var item in q)
					{
						Array.Reverse(item.GuidArray);
						values.Add(new MyIEnumEntity {
							Id = BitConverter.ToInt32(item.GuidArray, 0),
							Name = item.Name,
						});
					}
					return Ok(_enumBL.ApiEnumValuesFromEnumEntity(values.AsQueryable <MyIEnumEntity>()));
				case "authenticationtype":
					return Ok(_enumBL.ApiEnumValuesFromEnum<T2D.Model.Enums.AuthenticationType>());
				case "thingtype":
					return Ok(_enumBL.ApiEnumValuesFromEnum<T2D.Model.Enums.ThingType>());
			}
			return BadRequest($"Can't find values for enum '{enumName}'");

		}
		class MyIEnumEntity : T2D.Entities.IEnumEntity
		{
			public int Id { get; set; }
			public string Name { get; set; }
		}



	}
}
