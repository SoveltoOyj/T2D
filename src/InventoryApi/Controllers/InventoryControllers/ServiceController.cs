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
		[HttpPost, ActionName("GetServices")]
		[Produces(typeof(GetServicesResponse))]
		public IActionResult GetServices([FromBody]GetServicesRequest value)
		{
			var session = this.GetSession(value.Session, true);

			T2D.Entities.BaseThing thing =
				this.Find<T2D.Entities.BaseThing>(value.ThingId)
				.Include(t => t.ThingRoles)
				.FirstOrDefault()
				;

			if (thing == null)
				return BadRequest($"Thing '{value.ThingId}' do not exists.");

			var role = this.RoleMapper.EnumToEntity(value.Role);
			//TODO: check that session has right to 
			if (!AttributeSecurity.QueryServiceRequestRight(thing, session, role))
				return BadRequest($"Not enough priviledges to query Services of {value.ThingId}.");

			var q = dbc.ServiceDefinitions
							.Where(sd => sd.ThingId == thing.Id)
							.Select(sd => sd.Title)
							;

			GetServicesResponse ret = new GetServicesResponse
			{
				Services = q.ToList(),
			};
			return Ok(ret);
		}

		[HttpPost, ActionName("ServiceRequest")]
		public IActionResult ServiceRequest([FromBody]ServiceRequestRequest value)
		{
			var session = this.GetSession(value.Session, true);

			T2D.Entities.GenericThing thing =
				this.Find< T2D.Entities.GenericThing>(value.ThingId)
				.Include(t => t.ThingRoles)
				.Include(t => t.ServiceDefinitions)
					.ThenInclude(sd => sd.Actions)
				.Where(t=>t.ServiceDefinitions.Any(sd=>sd.Title==value.Service))
				.FirstOrDefault()
				;

			if (thing == null)
				return BadRequest($"Thing '{value.ThingId}' do not exists or do not have service {value.Service}.");

			//have to read again, navigation properties did not work as exptected
			ServiceDefinition serviceDefinition =	thing.ServiceDefinitions
					.Where(sd => sd.Title == value.Service)
					.SingleOrDefault()
					;


			// create ServiceStatus
			ServiceStatus ss = new ServiceStatus
			{
				ServiceDefinitionId = serviceDefinition.Id,
				SessionId = session.Id,
				StartedAt = DateTime.UtcNow,
				State = StateEnum.NotStarted,
				ThingId = session.EntryPoint_ThingId,
				CompletedAt=null,
			};
			dbc.ServiceStatuses.Add(ss);

			//create ActionStatuses
			foreach (var item in thing.ServiceDefinitions.First().Actions)
			{
				var actionStatus = new ActionStatus
				{
					ActionDefinitionId = item.Id,
					DeadLine = ss.StartedAt.Add(item.TimeSpan),
					ServiceStatus = ss,
					State = StateEnum.NotStarted,
					AddedAt = DateTime.UtcNow,
					CompletedAt=null,
				};
				ss.ActionStatuses.Add(actionStatus);
			}

			dbc.SaveChanges();

			return Ok();
		}

		[HttpPost, ActionName("GetServiceStatus")]
		[Produces(typeof(GetServiceStatusResponse))]
		public IActionResult GetServiceStatus([FromBody]GetServiceStatusRequest value)
		{
			var session = this.GetSession(value.Session, true);

			T2D.Entities.GenericThing thing =
				this.Find<T2D.Entities.GenericThing>(value.ThingId)
				.Include(t => t.ThingRoles)
				.FirstOrDefault()
				;

			if (thing == null)
				return BadRequest($"Thing '{value.ThingId}' do not exists.");


			GetServiceStatusResponse ret = new GetServiceStatusResponse
			{
				Statuses = new List<ServiceStatusResponse>()
			};

			var baseQuery = dbc.ServiceStatuses
				.Include(ss => ss.ServiceDefinition)
				.Where(ss => ss.ThingId == session.EntryPoint_ThingId)  //requestor is me
				.Where(ss => ss.ServiceDefinition.ThingId == thing.Id)
				;

			if (value.ServiceId == null)
			{
				var query = baseQuery
					.OrderByDescending(ss=>ss.StartedAt)
					.Take(10)
					;

				foreach (var item in query)
				{
					ret.Statuses.Add(new ServiceStatusResponse
					{
						ServiceId = item.Id,
						Title = item.ServiceDefinition.Title,
						RequestedAt = item.StartedAt,
						State = item.State.ToString(),
					});
				}
			}
			else
			{
				var serviceStatus = baseQuery
					.Where(ss => ss.Id == value.ServiceId.Value)
					.SingleOrDefault()
					;

				if (serviceStatus != null)
				{
					ret.Statuses.Add(new ServiceStatusResponse
					{
						ServiceId = serviceStatus.Id,
						Title = serviceStatus.ServiceDefinition.Title,
						RequestedAt = serviceStatus.StartedAt,
						State = serviceStatus.State.ToString(),
					});
				}
			}
			return Ok(ret);
		}

		[HttpPost, ActionName("GetActionStatuses")]
		[Produces(typeof(GetActionStatusesResponse))]
		public IActionResult GetActionStatuses([FromBody]GetActionStatusesRequest value)
		{
			var session = this.GetSession(value.Session, true);

			T2D.Entities.BaseThing thing =
				this.Find<T2D.Entities.BaseThing>(value.ThingId)
				.Include(t => t.ThingRoles)
				.FirstOrDefault()
				;

			if (thing == null)
				return BadRequest($"Thing '{value.ThingId}' do not exists.");

			GetActionStatusesResponse ret = new GetActionStatusesResponse
			{
				Statuses = new List<ActionStatusResponse>()
			};


			var query = dbc.ActionStatuses
				.Include(acs => acs.ServiceStatus)
				.Include(acs => acs.ActionDefinition)
				.Where(acs => acs.ActionDefinition.Operator_ThingId == thing.Id)  //action is assignt to this thing
				.OrderBy(acs => acs.State)
				.ThenByDescending(acs => acs.DeadLine)
				;

			foreach (var item in query)
				{
					ret.Statuses.Add(new ActionStatusResponse
					{
						ActionId = item.Id,
						Title = item.ActionDefinition.Title,
						AddedAt = item.AddedAt,
						State = item.State.ToString(),
						ActionClass = item.ActionDefinition.GetType().Name,
						ActionType= item.ActionDefinition.ActionListType.ToString(),
					});
				}

			
			return Ok(ret);
		}

		[HttpPost, ActionName("GetActionStatus")]
		[Produces(typeof(GetActionStatusResponse))]
		public IActionResult GetActionStatus([FromBody]GetActionStatusRequest value)
		{
			var session = this.GetSession(value.Session, true);

			T2D.Entities.BaseThing thing =
				this.Find<T2D.Entities.BaseThing>(value.ThingId)
				.Include(t => t.ThingRoles)
				.FirstOrDefault()
				;

			if (thing == null)
				return BadRequest($"Thing '{value.ThingId}' do not exists.");

			var actionStatus = dbc.ActionStatuses
				.Include(acs=>acs.ActionDefinition)
					.ThenInclude(ad=>ad.Alarm_Thing)
				.SingleOrDefault(acs => acs.Id == value.ActionId);


			if (actionStatus == null) return BadRequest($"Action '{value.ActionId}' do not exists."); 

			//get Status and update status if needed
			var serviceStatus = UpdateServiceRequestState(actionStatus, null);

			var ret = new GetActionStatusResponse
			{
				Action=new T2D.Model.Action
				{
					Id=value.ActionId,
					Title=actionStatus.ActionDefinition.Title,
					ActionClass = actionStatus.ActionDefinition.GetType().Name,
					ActionType = actionStatus.ActionDefinition.ActionListType.ToString(),
					Alarm_ThingId= CreateThingIdFromThing(actionStatus.ActionDefinition.Alarm_Thing),
					DeadLine = actionStatus.DeadLine ,
					State = actionStatus.State.ToString(),
					ThingId = CreateThingIdFromThing(actionStatus.ActionDefinition.Operator_Thing),
					Service = new Service
					{
						ThingId = CreateThingIdFromThing(serviceStatus.Thing),
						AddedAt = serviceStatus.StartedAt,
						Id = serviceStatus.Id,
						RequestorThingId = CreateThingIdFromThing(serviceStatus.Thing),
						SessionId=serviceStatus.SessionId,
						State = serviceStatus.State.ToString(),
						Title=serviceStatus.ServiceDefinition.Title,
					}
				}
			};

			return Ok(ret);
		}

		[HttpPost, ActionName("UpdateActionStatus")]
		public IActionResult UpdateActionStatus([FromBody]UpdateActionStatusRequest value)
		{
			var session = this.GetSession(value.Session, true);

			T2D.Entities.BaseThing thing =
				this.Find<T2D.Entities.BaseThing>(value.ThingId)
				.Include(t => t.ThingRoles)
				.FirstOrDefault()
				;

			if (thing == null)
				return BadRequest($"Thing '{value.ThingId}' do not exists.");


			var actionStatus = dbc.ActionStatuses
				.Include(acs => acs.ActionDefinition)
					.ThenInclude(ad => ad.Alarm_Thing)
				.SingleOrDefault(acs => acs.Id == value.ActionId);


			if (actionStatus == null) return BadRequest($"Action '{value.ActionId}' do not exists.");

			StateEnum newState;
			if (Enum.TryParse(value.State, out newState))
			{
				UpdateServiceRequestState(actionStatus, newState);
			}
			else
			{
				return BadRequest($"Unknown state {value.State}.");
			}
			return Ok();

		}

		[NonAction]
		private ServiceStatus UpdateServiceRequestState(ActionStatus actionStatus, StateEnum? newActionState)
		{

			var thisServiceStatus = dbc.ServiceStatuses
				.Include(ss=>ss.ActionStatuses)
					.ThenInclude(acs=>acs.ActionDefinition)
				.Include(ss=>ss.ServiceDefinition)
				.Single(ss=>ss.Id == actionStatus.ServiceStatusId)
			  ;

			if (newActionState != null)
			{
				actionStatus.State = newActionState.Value;
			}
			if (IsStateNotFinneshed(thisServiceStatus.State))
			{
				//check if any mandatory action is over deadline
				var q = thisServiceStatus.ActionStatuses
					.Where(acs => acs.DeadLine < DateTime.UtcNow)
					.Where(acs => IsStateNotFinneshed(acs.State))
					;

				foreach (var item in q)
				{
					item.State = StateEnum.NotDoneInTime;
				}
				if (q.Count() > 0)
				{
					thisServiceStatus.State = StateEnum.NotDoneInTime;
				}
			}

			List<StateEnum> states;
			// check if it service has been started
			if (thisServiceStatus.State == StateEnum.NotStarted)
			{
				states = new List<StateEnum>
				{
					StateEnum.Done, StateEnum.Started,
				};
				var q = thisServiceStatus.ActionStatuses
					.Where(acs => states.Contains(acs.State))
					;
				if (q.Count() > 0)
				{
					thisServiceStatus.State = StateEnum.Started;
				}
			}

			// check if done, all mandatory done in time
			if (thisServiceStatus.State == StateEnum.Started)
			{
				var q = thisServiceStatus.ActionStatuses
					.Where(acs => acs.State != StateEnum.Done )
					.Where(acs => acs.ActionDefinition.ActionListType == ActionListType.Mandatory)
					;
				if (q.Count() < 1)
				{
					thisServiceStatus.State = StateEnum.Done;
				}
			}


			dbc.SaveChanges();
			return thisServiceStatus;
		}

		private bool IsStateNotFinneshed(StateEnum state)
		{
			return state == StateEnum.NotStarted || state == StateEnum.Started;
		}

		private string CreateThingIdFromThing(T2D.Entities.BaseThing thing)
		{
			return ThingIdHelper.Create(thing.Fqdn, thing.US, false);
		}
	}
}
