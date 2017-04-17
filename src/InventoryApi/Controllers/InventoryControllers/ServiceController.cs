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
		#region vanha

		//[HttpPost, ActionName("GetServiceStatus")]
		//[Produces(typeof(GetServiceStatusResponse))]
		//public IActionResult GetServiceStatus([FromBody]GetServiceStatusRequest value)
		//{
		//	var session = this.GetSession(value.Session, true);

		//	T2D.Entities.GenericThing thing =
		//		this.Find<T2D.Entities.GenericThing>(value.ThingId)
		//		.Include(t => t.ThingRoles)
		//		.FirstOrDefault()
		//		;

		//	if (thing == null)
		//		return BadRequest($"Thing '{value.ThingId}' do not exists.");


		//	GetServiceStatusResponse ret = new GetServiceStatusResponse
		//	{
		//		Statuses = new List<ServiceStatusResponse>()
		//	};

		//	var baseQuery = dbc.ServiceStatuses
		//		.Include(ss => ss.ServiceDefinition)
		//		.Where(ss => ss.ThingId == session.EntryPoint_ThingId)  //requestor is me
		//		.Where(ss => ss.ServiceDefinition.ThingId == thing.Id)
		//		;

		//	if (value.ServiceId == null)
		//	{
		//		var query = baseQuery
		//			.OrderByDescending(ss=>ss.StartedAt)
		//			.Take(10)
		//			;

		//		foreach (var item in query)
		//		{
		//			ret.Statuses.Add(new ServiceStatusResponse
		//			{
		//				ServiceId = item.Id,
		//				Title = item.ServiceDefinition.Title,
		//				RequestedAt = item.StartedAt,
		//				State = item.State.ToString(),
		//			});
		//		}
		//	}
		//	else
		//	{
		//		var serviceStatus = baseQuery
		//			.Where(ss => ss.Id == value.ServiceId.Value)
		//			.SingleOrDefault()
		//			;

		//		if (serviceStatus != null)
		//		{
		//			ret.Statuses.Add(new ServiceStatusResponse
		//			{
		//				ServiceId = serviceStatus.Id,
		//				Title = serviceStatus.ServiceDefinition.Title,
		//				RequestedAt = serviceStatus.StartedAt,
		//				State = serviceStatus.State.ToString(),
		//			});
		//		}
		//	}
		//	return Ok(ret);
		//}

		//[HttpPost, ActionName("GetActionStatuses")]
		//[Produces(typeof(GetActionStatusesResponse))]
		//public IActionResult GetActionStatuses([FromBody]GetActionStatusesRequest value)
		//{
		//	var session = this.GetSession(value.Session, true);

		//	T2D.Entities.BaseThing thing =
		//		this.Find<T2D.Entities.BaseThing>(value.ThingId)
		//		.Include(t => t.ThingRoles)
		//		.FirstOrDefault()
		//		;

		//	if (thing == null)
		//		return BadRequest($"Thing '{value.ThingId}' do not exists.");

		//	GetActionStatusesResponse ret = new GetActionStatusesResponse
		//	{
		//		Statuses = new List<ActionStatusResponse>()
		//	};


		//	var query = dbc.ActionStatuses
		//		.Include(acs => acs.ServiceStatus)
		//		.Include(acs => acs.ActionDefinition)
		//		.Where(acs => acs.ActionDefinition.Operator_ThingId == thing.Id)  //action is assignt to this thing
		//		.OrderBy(acs => acs.State)
		//		.ThenByDescending(acs => acs.DeadLine)
		//		;

		//	foreach (var item in query)
		//		{
		//			ret.Statuses.Add(new ActionStatusResponse
		//			{
		//				ActionId = item.Id,
		//				Title = item.ActionDefinition.Title,
		//				AddedAt = item.AddedAt,
		//				State = item.State.ToString(),
		//				ActionClass = item.ActionDefinition.GetType().Name,
		//				ActionType= item.ActionDefinition.ActionListType.ToString(),
		//			});
		//		}


		//	return Ok(ret);
		//}

		//[HttpPost, ActionName("GetActionStatus")]
		//[Produces(typeof(GetActionStatusResponse))]
		//public IActionResult GetActionStatus([FromBody]GetActionStatusRequest value)
		//{
		//	var session = this.GetSession(value.Session, true);

		//	T2D.Entities.BaseThing thing =
		//		this.Find<T2D.Entities.BaseThing>(value.ThingId)
		//		.Include(t => t.ThingRoles)
		//		.FirstOrDefault()
		//		;

		//	if (thing == null)
		//		return BadRequest($"Thing '{value.ThingId}' do not exists.");

		//	var actionStatus = dbc.ActionStatuses
		//		.Include(acs=>acs.ActionDefinition)
		//			.ThenInclude(ad=>ad.Alarm_Thing)
		//		.SingleOrDefault(acs => acs.Id == value.ActionId);


		//	if (actionStatus == null) return BadRequest($"Action '{value.ActionId}' do not exists."); 

		//	//get Status and update status if needed
		//	var serviceStatus = UpdateServiceRequestState(actionStatus, null);

		//	var ret = new GetActionStatusResponse
		//	{
		//		Action=new T2D.Model.Action
		//		{
		//			Id=value.ActionId,
		//			Title=actionStatus.ActionDefinition.Title,
		//			ActionClass = actionStatus.ActionDefinition.GetType().Name,
		//			ActionType = actionStatus.ActionDefinition.ActionListType.ToString(),
		//			Alarm_ThingId= CreateThingIdFromThing(actionStatus.ActionDefinition.Alarm_Thing),
		//			DeadLine = actionStatus.DeadLine ,
		//			State = actionStatus.State.ToString(),
		//			ThingId = CreateThingIdFromThing(actionStatus.ActionDefinition.Operator_Thing),
		//			Service = new Service
		//			{
		//				ThingId = CreateThingIdFromThing(serviceStatus.ServiceDefinition.Thing),
		//				AddedAt = serviceStatus.StartedAt,
		//				Id = serviceStatus.Id,
		//				RequestorThingId = CreateThingIdFromThing(serviceStatus.Thing),
		//				SessionId=serviceStatus.SessionId,
		//				State = serviceStatus.State.ToString(),
		//				Title=serviceStatus.ServiceDefinition.Title,
		//			}
		//		}
		//	};

		//	return Ok(ret);
		//}

		//[HttpPost, ActionName("UpdateActionStatus")]
		//public IActionResult UpdateActionStatus([FromBody]UpdateActionStatusRequest value)
		//{
		//	var session = this.GetSession(value.Session, true);

		//	T2D.Entities.BaseThing thing =
		//		this.Find<T2D.Entities.BaseThing>(value.ThingId)
		//		.Include(t => t.ThingRoles)
		//		.FirstOrDefault()
		//		;

		//	if (thing == null)
		//		return BadRequest($"Thing '{value.ThingId}' do not exists.");


		//	var actionStatus = dbc.ActionStatuses
		//		.Include(acs => acs.ActionDefinition)
		//			.ThenInclude(ad => ad.Alarm_Thing)
		//		.SingleOrDefault(acs => acs.Id == value.ActionId);


		//	if (actionStatus == null) return BadRequest($"Action '{value.ActionId}' do not exists.");

		//	ServiceAndActitivityState newState;
		//	if (Enum.TryParse(value.State, out newState))
		//	{
		//		UpdateServiceRequestState(actionStatus, newState);
		//	}
		//	else
		//	{
		//		return BadRequest($"Unknown state {value.State}.");
		//	}
		//	return Ok();

		//}

		//[NonAction]
		//private ServiceStatus UpdateServiceRequestState(ActionStatus actionStatus, ServiceAndActitivityState? newActionState)
		//{

		//	var thisServiceStatus = dbc.ServiceStatuses
		//		.Include(ss=>ss.Thing)
		//		.Include(ss=>ss.ActionStatuses)
		//			.ThenInclude(acs=>acs.ActionDefinition)
		//		.Include(ss=>ss.ServiceDefinition)
		//		  .ThenInclude(sd=>sd.Thing)
		//		.Single(ss=>ss.Id == actionStatus.ServiceStatusId)
		//	  ;

		//	if (newActionState != null)
		//	{
		//		actionStatus.State = newActionState.Value;
		//	}
		//	if (IsStateNotFinneshed(thisServiceStatus.State))
		//	{
		//		//check if any mandatory action is over deadline
		//		var q = thisServiceStatus.ActionStatuses
		//			.Where(acs => acs.DeadLine < DateTime.UtcNow)
		//			.Where(acs => IsStateNotFinneshed(acs.State))
		//			;

		//		foreach (var item in q)
		//		{
		//			item.State = ServiceAndActitivityState.NotDoneInTime;
		//		}
		//		if (q.Count() > 0)
		//		{
		//			thisServiceStatus.State = ServiceAndActitivityState.NotDoneInTime;
		//		}
		//	}

		//	List<ServiceAndActitivityState> states;
		//	// check if it service has been started
		//	if (thisServiceStatus.State == ServiceAndActitivityState.NotStarted)
		//	{
		//		states = new List<ServiceAndActitivityState>
		//		{
		//			ServiceAndActitivityState.Done, ServiceAndActitivityState.Started,
		//		};
		//		var q = thisServiceStatus.ActionStatuses
		//			.Where(acs => states.Contains(acs.State))
		//			;
		//		if (q.Count() > 0)
		//		{
		//			thisServiceStatus.State = ServiceAndActitivityState.Started;
		//		}
		//	}

		//	// check if done, all mandatory done in time
		//	if (thisServiceStatus.State == ServiceAndActitivityState.Started)
		//	{
		//		var q = thisServiceStatus.ActionStatuses
		//			.Where(acs => acs.State != ServiceAndActitivityState.Done )
		//			.Where(acs => acs.ActionDefinition.ActionListType == ActionListType.Mandatory)
		//			;
		//		if (q.Count() < 1)
		//		{
		//			thisServiceStatus.State = ServiceAndActitivityState.Done;
		//		}
		//	}


		//	dbc.SaveChanges();
		//	return thisServiceStatus;
		//}

		//private bool IsStateNotFinneshed(ServiceAndActitivityState state)
		//{
		//	return state == ServiceAndActitivityState.NotStarted || state == ServiceAndActitivityState.Started;
		//}

		//private string CreateThingIdFromThing(T2D.Entities.BaseThing thing)
		//{
		//	return ThingIdHelper.Create(thing.Fqdn, thing.US, false);
		//}
		#endregion



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
