using InventoryApi.Controllers.BaseControllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using T2D.Entities;
using T2D.Helpers;
using T2D.InventoryBL;
using T2D.InventoryBL.Thing;
using T2D.Model;
using T2D.Model.Helpers;
using T2D.Model.InventoryApi;

namespace InventoryApi.Controllers.InventoryControllers
{
	/// <summary>
	/// Core 1 operations, currently only MOCS
	/// </summary>
	[Route("api/inventory/[controller]/[action]")]
	public class CoreController : ApiBaseController
	{

		//these are got from BaseRequest
		protected SessionBL _sessionBl;
		protected ThingBL _thingBl;
		protected int _roleId;

		/// <summary>
		/// Query my roles.
		/// </summary>
		/// <param name="value">Request argument</param>
		/// <returns>Available roles.</returns>
		/// <response code="200">Returns available roles.</response>
		/// <response code="400">Bad request, like Thing do not exists or not enough priviledges.</response>
		[HttpPost, ActionName("QueryMyRoles")]
		[Produces(typeof(QueryMyRolesResponse))]
		public IActionResult QueryMyRoles([FromBody]QueryMyRolesRequest value)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);

			_sessionBl = SessionBL.CreateSessionBLForExistingSession(_dbc, value.Session);
			if (_sessionBl == null) return BadRequest("Session is not correct.");

			_thingBl = ThingBL.CreateThingBL(_dbc, _sessionBl);
			if (_thingBl == null) return BadRequest("Something went wrong.");

			string errMsg = null;
			var ret = _thingBl.QueryMyRoles(out errMsg, value.ThingId);

			if (ret != null)
				return Ok(ret);

			return BadRequest(errMsg);
		}

		/// <summary>
		/// Get Relations.
		/// </summary>
		/// <param name="value">Request argument</param>
		/// <returns>All relations this object has.</returns>
		/// <response code="200">Returns Relations.</response>
		/// <response code="400">Bad request, like Thing do not exists or not enough priviledges.</response>
		[HttpPost, ActionName("GetRelations")]
		[Produces(typeof(GetRelationsResponse))]
		public IActionResult GetRelations([FromBody]GetRelationsRequest value)
		{
			ProcessBaseRequest(value);
			var baseResponse = ProcessBaseRequest(value);
			if (baseResponse != null) return baseResponse;

			string errMsg = null;
			var ret = _thingBl.GetRelations(out errMsg, _roleId, value.ThingId);

			if (ret != null) return Ok(ret);
			return BadRequest(errMsg);

		}

		/// <summary>
		/// Get Attributes.
		/// </summary>
		/// <param name="value">Request argument</param>
		/// <returns>Attribute values.</returns>
		/// <response code="200">Returns attribute values.</response>
		/// <response code="400">Bad request, like Thing do not exists or not enough priviledges.</response>
		[HttpPost, ActionName("GetAttributes")]
		[Produces(typeof(GetAttributesResponse))]
		public IActionResult GetAttributes([FromBody]GetAttributesRequest value)
		{
			ProcessBaseRequest(value);
			var baseResponse = ProcessBaseRequest(value);
			if (baseResponse != null) return baseResponse;

			var ret = new GetAttributesResponse
			{
				AttributeValues = new List<AttributeValue>(value.Attributes.Count()),
			};
			foreach (var item in value.Attributes)
			{
				ret.AttributeValues.Add(_thingBl.GetAttribute(item, _roleId, value.ThingId));
			}

			return Ok(ret);
		}

		/// <summary>
		/// Set Attributes.
		/// </summary>
		/// <param name="value">Request argument</param>
		/// <response code="200">Returns current attribute values, where you can find out which settings were successfull.</response>
		/// <response code="400">Bad request, like Thing do not exists or not enough priviledges.</response>
		[HttpPost, ActionName("SetAttributes")]
		[Produces(typeof(SetAttributesResponse))]
		public IActionResult SetAttributes([FromBody]SetAttributesRequest value)
		{
			ProcessBaseRequest(value);
			var baseResponse = ProcessBaseRequest(value);
			if (baseResponse != null) return baseResponse;

			var ret = new SetAttributesResponse
			{
				AttributeValues = new List<AttributeValue>(value.AttributeValues.Count()),
			};
			foreach (var item in value.AttributeValues)
			{
				ret.AttributeValues.Add(_thingBl.SetAttribute(item.Attribute, _roleId, value.ThingId, item.Value));
			}
			_dbc.SaveChanges();
			return Ok(ret);
		}


		/// <summary>
		/// Create a new local Thing.
		/// </summary>
		/// <param name="value">Request argument</param>
		/// <response code="200">A new Thing was created.</response>
		/// <response code="400">Bad request, like Thing Id is not Unique or not enough priviledges.</response>
		[HttpPost, ActionName("CreateLocalThing")]
		public IActionResult CreateLocalThing([FromBody]CreateLocalThingRequest value)
		{
			var baseResponse = ProcessBaseRequest(value);
			if (baseResponse != null) return baseResponse;

			string errMsg = null;
			bool ret = _thingBl.CreateNewThing(
				out errMsg,
				value.NewThingId,
				value.Title,
				value.ThingType,
				value.ThingId
				);

			if (ret) return Ok();
			return BadRequest(errMsg);
		}

		/// <summary>
		/// Set thing role access rights
		/// </summary>
		/// <param name="value">Request argument</param>
		/// <response code="200">Right set was done, there can be errors (look at return body).</response>
		/// <response code="400">Bad request, like Thing Id is OK or not enough priviledges.</response>
		[HttpPost, ActionName("SetRoleAccessRight")]
		[Produces(typeof(string))]
		public IActionResult SetRoleAccessRight([FromBody]SetRoleAccessRightsRequest value)
		{
			var baseResponse = ProcessBaseRequest(value);
			if (baseResponse != null) return baseResponse;

			string errMsg = null;
			string allErrorMessages = "";
			int? roleForRightId = _enumBL.EnumIdFromApiString<RoleEnum>(value.RoleForRights);
			if (roleForRightId == null) return BadRequest($"Role for right '{value.RoleForRights}' is not correct.");
			foreach (var item in value.AttributeRoleRights)
			{
				int? attributeId = _enumBL.EnumIdFromApiString<AttributeEnum>(item.Attribute);
				if (attributeId == null)
				{
					allErrorMessages += $"Attribute {item.Attribute} is not correct, continueing with other attributes. ";
					continue;
				}
				_thingBl.SetRoleAccessRights(out errMsg, _roleId, roleForRightId.Value, attributeId.Value, value.ThingId, item.RoleAccessRights);
				allErrorMessages += errMsg;
				errMsg = null;
			}

			return Ok(allErrorMessages);
		}

		/// <summary>
		/// Get thing role access rights
		/// </summary>
		/// <param name="value">Request argument</param>
		/// <response code="200">Get arguments were OK.</response>
		/// <response code="400">Bad request, like Thing Id is OK or not enough priviledges.</response>
		[HttpPost, ActionName("GetRoleAccessRight")]
		[Produces(typeof(GetRoleAccessRightsResponse))]
		public IActionResult GetRoleAccessRight([FromBody]GetRoleAccessRightsRequest value)
		{
			var baseResponse = ProcessBaseRequest(value);
			if (baseResponse != null) return baseResponse;

			string errMsg = null;
			int? roleForRightId = _enumBL.EnumIdFromApiString<RoleEnum>(value.RoleForRights);
			if (roleForRightId == null) return BadRequest($"Role for right '{value.RoleForRights}' is not correct.");

			var ret = new GetRoleAccessRightsResponse();
			ret.AttributeRoleRights = _thingBl.GetRoleAccessRights(out errMsg, _roleId, roleForRightId.Value, value.ThingId);

			if (ret.AttributeRoleRights == null) return BadRequest(errMsg);
			return Ok(ret);
		}


		/// <summary>
		/// Set the thing role memberlist.
		/// </summary>
		/// <param name="value">Request argument</param>
		/// <response code="200">Memberlist is set, there can be errors (look at return body).</response>
		/// <response code="400">Bad request, like Thing Id is OK or not enough priviledges.</response>
		[HttpPost, ActionName("SetRoleMemberList")]
		[Produces(typeof(string))]
		public IActionResult SetRoleMemberList([FromBody]SetRoleMemberListRequest value)
		{
			var baseResponse = ProcessBaseRequest(value);
			if (baseResponse != null) return baseResponse;

			string errMsg = null;
			int? roleToSetId = _enumBL.EnumIdFromApiString<RoleEnum>(value.RoleForMemberList);
			if (roleToSetId == null) return BadRequest($"Role for right '{value.RoleForMemberList}' is not correct.");

			if (_thingBl.SetRoleMemberList(out errMsg, _roleId, roleToSetId.Value, value.ThingId, value.MemberThingIds))
			{
				return Ok(errMsg);
			}
			return BadRequest(errMsg);
		}

		/// <summary>
		/// Get thing role Members
		/// </summary>
		/// <param name="value">Request argument</param>
		/// <response code="200">Get arguments were OK.</response>
		/// <response code="400">Bad request, like Thing Id is OK or not enough priviledges.</response>
		[HttpPost, ActionName("GetRoleMemberList")]
		[Produces(typeof(GetRoleMemberListResponse))]
		public IActionResult GetRoleMemberList([FromBody]GetRoleMemberListRequest value)
		{
			var baseResponse = ProcessBaseRequest(value);
			if (baseResponse != null) return baseResponse;

			string errMsg = null;
			int? roleForMemberListId = _enumBL.EnumIdFromApiString<RoleEnum>(value.RoleForMemberList);
			if (roleForMemberListId == null) return BadRequest($"Role for right '{value.RoleForMemberList}' is not correct.");

			var ret = new GetRoleMemberListResponse();
			ret.ThingIds = _thingBl.GetRoleMemberList(out errMsg, _roleId, roleForMemberListId.Value, value.ThingId);

			if (ret.ThingIds == null) return BadRequest(errMsg);
			return Ok(ret);
		}

		/// <summary>
		/// Get thing ID:s that are near some location.
		/// </summary>
		/// <param name="value">Request argument</param>
		/// <response code="200">Get arguments were OK.</response>
		/// <response code="400">Bad request.</response>
		[HttpPost, ActionName("GetNearbyPublicLocationThings")]
		[Produces(typeof(GetNearbyPublicLocationThingsResponse))]
		public IActionResult GetNearbyPublicLocationThings([FromBody]GetNearbyPublicLocationThingsRequest value)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);
			GetNearbyPublicLocationThingsResponse ret = new GetNearbyPublicLocationThingsResponse();
			ret.Things = new List<IdTitleDistance>();

			using (var command = _dbc.Database.GetDbConnection().CreateCommand())
			{
				string sql = $"declare @p geography;" +
					$" set @p = geography::Parse('POINT({value.GpsLocation.Longitude.ToString(System.Globalization.CultureInfo.InvariantCulture)} {value.GpsLocation.Latitude.ToString(System.Globalization.CultureInfo.InvariantCulture)})');" +
						$" select fqdn, US, Title, @p.STDistance(Location_Gps) as 'Distance' from things " +
						$" WHERE  @p.STDistance(Location_Gps) < {value.Distance.ToString(System.Globalization.CultureInfo.InvariantCulture)} " +
						$" AND IsGpsPublic=1" +
						$" ORDER BY Distance, Id;" 
						;

				command.CommandText = sql;
				_dbc.Database.OpenConnection();
				using (var result = command.ExecuteReader())
				{
					while (result.Read())
					{
						ret.Things.Add(new IdTitleDistance
						{
							Distance = decimal.Parse(result["Distance"].ToString(), System.Globalization.CultureInfo.InvariantCulture),
							IdTitle = new GetRelationsResponse.RelationsThings.IdTitle
							{
								ThingId = ThingIdHelper.Create(result["fqdn"].ToString(), result["US"].ToString()),
								Title = result["title"].ToString()
							}
						});
					}
				}
			}
			return Ok(ret);
		}



		[NonAction]
		protected BadRequestObjectResult ProcessBaseRequest(BaseRequest value)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);

			_sessionBl = SessionBL.CreateSessionBLForExistingSession(_dbc, value.Session);
			if (_sessionBl == null) return BadRequest("Session is not correct.");

			_thingBl = ThingBL.CreateThingBL(_dbc, _sessionBl);
			if (_thingBl == null) return BadRequest("Something went wrong.");

			int? roleId = _enumBL.EnumIdFromApiString<RoleEnum>(value.Role);
			if (roleId == null) return BadRequest($"Role '{value.Role}' is not correct.");
			_roleId = roleId.Value;
			return null;
		}
	}
}
