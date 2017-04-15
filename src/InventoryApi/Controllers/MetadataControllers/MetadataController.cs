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
	public class MetadataController : ApiBaseControllerOld
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
			return Ok(new EnumBL().ApiEnumNames());
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
			var enumBL = new EnumBL();
			if (!enumBL.ApiEnumNames().Contains(enumName)) return BadRequest($"{enumName} is not a valid EnumName.");

			switch (enumName.ToLower())
			{
				case "attributetype":
					return Ok(enumBL.ApiEnumValuesFromEnum<T2D.Entities.AttributeType>());
				case "functionalstatus":
					return Ok(enumBL.ApiEnumValuesFromEnumEntity(dbc.Status.AsQueryable()));
				case "locationtype":
					return Ok(enumBL.ApiEnumValuesFromEnumEntity(dbc.LocationTypes.AsQueryable()));
				case "relation":
					return Ok(enumBL.ApiEnumValuesFromEnumEntity(dbc.Relations.AsQueryable()));
				case "right":
					return Ok(enumBL.ApiEnumValuesFromEnum<T2D.Entities.RightFlag>());
				case "role":
					return Ok(enumBL.ApiEnumValuesFromEnumEntity(dbc.Roles.AsQueryable()));
				case "serviceandactivitystate":
					return Ok(enumBL.ApiEnumValuesFromEnum<T2D.Entities.ServiceAndActitivityState>());
				case "thingstatus":
					return Ok(enumBL.ApiEnumValuesFromEnum<T2D.Entities.ThingStatus>());
				case "attribute":
					//Note: attribute has GUID ID, thats why this code.
					var q = dbc.Attributes
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
					return Ok(enumBL.ApiEnumValuesFromEnumEntity(values.AsQueryable <MyIEnumEntity>()));
				case "authenticationtype":
					return Ok(enumBL.ApiEnumValuesFromEnum<T2D.Model.Enums.AuthenticationType>());
				case "thingtype":
					return Ok(enumBL.ApiEnumValuesFromEnum<T2D.Model.Enums.ThingType>());
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
