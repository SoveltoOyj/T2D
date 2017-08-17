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
using T2D.Infra;
using T2D.InventoryBL;
using T2D.InventoryBL.ServiceRequest;
using T2D.InventoryBL.Thing;
using T2D.Model;
using T2D.Model.Helpers;
using T2D.Model.InventoryApi;
using T2D.Model.ServiceApi;

namespace InventoryApi.Controllers.InventoryControllers
{
	/// <summary>
	/// Core 1 operations, currently only MOCS
	/// </summary>
	[Route("api/inventory/[controller]/[action]")]
	public class ServiceController : ApiBaseController
	{
		protected SessionBL _sessionBl;
		protected ThingBL _thingBl;
		protected int _roleId;
		protected ServiceBL _serviceBl;

		public ServiceController(EfContext dbc) : base(dbc)	{	}

	/// <summary>
	/// Create a new Service.
	/// </summary>
	/// <param name="value">Request argument</param>
	/// <response code="200">if Service was created..</response>
	/// <response code="400">Bad request, like Thing do not exists or not enough priviledges.</response>
	[HttpPost, ActionName("CreateService")]
		[Produces(typeof(bool))]
		public IActionResult CreateService([FromBody]CreateServiceTypeRequest value)
		{
			ProcessBaseRequest(value);
			var baseResponse = ProcessBaseRequest(value);
			if (baseResponse != null) return baseResponse;

			string errMsg = null;
			if (_serviceBl.CreateNewService(out errMsg, value)) return Ok(true);

			return BadRequest(errMsg);
		}



		/// <summary>
		/// Get Services if one Thing.
		/// </summary>
		/// <param name="value">Request argument</param>
		/// <response code="200">Returns all Services of a thing</response>
		/// <response code="400">Bad request, like Thing do not exists or not enough priviledges.</response>
		[HttpPost, ActionName("GetServices")]
		[Produces(typeof(GetServicesResponse))]
		public IActionResult GetServices([FromBody]GetServicesRequest value)
		{
			ProcessBaseRequest(value);
			var baseResponse = ProcessBaseRequest(value);
			if (baseResponse != null) return baseResponse;

			string errMsg = null;
			GetServicesResponse ret = new GetServicesResponse();

			ret.Services = _serviceBl.GetServices(out errMsg, value.ThingId, _roleId); 
			if (errMsg==null)	return Ok(ret);
			return BadRequest(errMsg);

		}

		/// <summary>
		/// Activates a service.
		/// </summary>
		/// <param name="value">Request argument</param>
		/// <response code="200">Service was activated.</response>
		/// <response code="400">Bad request, like Thing do not exists or not enough priviledges.</response>
		[HttpPost, ActionName("ServiceRequest")]
		public IActionResult ServiceRequest([FromBody]ServiceRequestRequest value)
		{
			ProcessBaseRequest(value);
			var baseResponse = ProcessBaseRequest(value);
			if (baseResponse != null) return baseResponse;

			string errMsg = null;

			if (_serviceBl.ActivateService(out errMsg, value.ThingId, _roleId, value.Service))
				return Ok();

			return BadRequest(errMsg);

		}

		/// <summary>
		/// Get Service Status.
		/// </summary>
		/// <param name="value">Request argument. If Service Guid is missing, last 10 servicerequest will be returned.</param>
		/// <response code="200">Service status(es) are returned.</response>
		/// <response code="400">Bad request, like Thing do not exists or not enough priviledges.</response>
		[HttpPost, ActionName("GetServiceStatus")]
		[Produces(typeof(GetServiceStatusResponse))]
		public IActionResult GetServiceStatus([FromBody]GetServiceStatusRequest value)
		{
			ProcessBaseRequest(value);
			var baseResponse = ProcessBaseRequest(value);
			if (baseResponse != null) return baseResponse;

			string errMsg = null;
			GetServiceStatusResponse ret = new GetServiceStatusResponse();

			ret.Statuses = _serviceBl.GetServiceStatuses(out errMsg, value.ThingId, _roleId, value.ServiceId);
			if (errMsg == null) return Ok(ret);
			return BadRequest(errMsg);

		}

		/// <summary>
		/// Get Action Statuses for one OperatorThing.
		/// </summary>
		/// <param name="value">Request argument.</param>
		/// <response code="200">Action statuses where Operator Thing is this request Thing.</response>
		/// <response code="400">Bad request, like Thing do not exists or not enough priviledges.</response>
		[HttpPost, ActionName("GetActionStatuses")]
		[Produces(typeof(GetActionStatusesResponse))]
		public IActionResult GetActionStatuses([FromBody]GetActionStatusesRequest value)
		{
			ProcessBaseRequest(value);
			var baseResponse = ProcessBaseRequest(value);
			if (baseResponse != null) return baseResponse;

			string errMsg = null;
			GetActionStatusesResponse ret = new GetActionStatusesResponse();

			ret.Statuses = _serviceBl.GetActionStatuses(out errMsg, value.ThingId, _roleId);
			if (errMsg == null) return Ok(ret);
			return BadRequest(errMsg);


		}


		/// <summary>
		/// Get one Action Status.
		/// </summary>
		/// <param name="value">Request argument.</param>
		/// <response code="200">Action status.</response>
		/// <response code="400">Bad request, like Thing do not exists or not enough priviledges.</response>
		[HttpPost, ActionName("GetActionStatus")]
		[Produces(typeof(GetActionStatusResponse))]
		public IActionResult GetActionStatus([FromBody]GetActionStatusRequest value)
		{
			ProcessBaseRequest(value);
			var baseResponse = ProcessBaseRequest(value);
			if (baseResponse != null) return baseResponse;

			string errMsg = null;
			GetActionStatusResponse ret = new GetActionStatusResponse();

			ret.Action = _serviceBl.GetActionStatus(out errMsg, value.ThingId, _roleId, value.ActionId);
			if (errMsg == null) return Ok(ret);
			return BadRequest(errMsg);
		}

		/// <summary>
		/// Update Action Status.
		/// </summary>
		/// <param name="value">Request argument.</param>
		/// <response code="200">Action status updated.</response>
		/// <response code="400">Bad request, like Thing do not exists or not enough priviledges.</response>
		[HttpPost, ActionName("UpdateActionStatus")]
		public IActionResult UpdateActionStatus([FromBody]UpdateActionStatusRequest value)
		{
			ProcessBaseRequest(value);
			var baseResponse = ProcessBaseRequest(value);
			if (baseResponse != null) return baseResponse;

			string errMsg = null;

			_serviceBl.UpdateActionStatus(out errMsg, value.ThingId, _roleId, value.ActionId, value.State);
			if (errMsg == null) return Ok();
			return BadRequest(errMsg);


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

			_serviceBl = ServiceBL.CreateServiceBL(_dbc, _sessionBl);
			return null;
		}


	}
}
