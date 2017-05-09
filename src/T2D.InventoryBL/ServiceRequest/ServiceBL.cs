using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using T2D.Entities;
using T2D.Infra;
using T2D.Model.ServiceApi;
using T2D.Model;
using T2D.Model.Helpers;
using T2D.InventoryBL.Metadata;

namespace T2D.InventoryBL.ServiceRequest
{
	public class ServiceBL
	{
		private readonly SessionBL _session;
		private readonly EfContext _dbc;

		public static ServiceBL CreateServiceBL(EfContext dbc, SessionBL session)
		{
			if (session == null) return null;
			ServiceBL ret = new ServiceBL(dbc, session);

			return ret;
		}
		private ServiceBL(EfContext dbc, SessionBL session)
		{
			_dbc = dbc;
			_session = session;
		}


		public bool CreateNewService(out string errMsg, CreateServiceTypeRequest request)
		{
			errMsg = "";
			// check things
			var toThing = _dbc.FindThing<BaseThing>(request.ThingId);
			if (toThing == null)
			{
				errMsg = $"The thing '{request.ThingId}'to which ServiceType was created do not exists.";
				return false;
			}

			//create ServiceDefinition
			var sd = new ServiceDefinition
			{
				ThingId = toThing.Id,
				Title = request.Title,
			};
			_dbc.ServiceDefinitions.Add(sd);

			//ToDo: only GenericAction is currently supported.
			if (request.MandatoryActions != null)
			{
				foreach (var item in request.MandatoryActions)
				{
					if (!CreateNewActionDefinition(out errMsg, sd, item, ActionListType.Mandatory))
					{
						return false;
					}
				}
			}
			if (request.OptionalActions != null)
			{
				foreach (var item in request.OptionalActions)
				{
					if (!CreateNewActionDefinition(out errMsg, sd, item, ActionListType.Optional))
					{
						return false;
					}
				}
			}
			if (request.SelectedActions != null)
			{
				foreach (var item in request.SelectedActions)
				{
					if (!CreateNewActionDefinition(out errMsg, sd, item, ActionListType.Selected))
					{
						return false;
					}
				}
			}

			if (request.PendingActions != null)
			{
				foreach (var item in request.PendingActions)
				{
					if (!CreateNewActionDefinition(out errMsg, sd, item, ActionListType.Pending))
					{
						return false;
					}
				}
			}
			_dbc.SaveChanges();
			return true;
		}

		private bool CreateNewActionDefinition(out string errMsg, ServiceDefinition sd, Model.ServiceApi.ActionDefinition item, ActionListType actionListType)
		{
			errMsg = null;
			BaseThing alarmThing = null;
			if (!string.IsNullOrWhiteSpace(item.AlarmThingId))
			{
				alarmThing = _dbc.FindThing<BaseThing>(item.AlarmThingId);
				if (alarmThing == null)
				{
					errMsg = $"The alarm thing '{item.AlarmThingId}' do not exists.";
					return false;
				}
			}

				BaseThing objectThing = _dbc.FindThing<BaseThing>(item.ObjectThingId);
				if (objectThing == null)
				{
					errMsg = $"The object thing '{item.ObjectThingId}' do not exists.";
					return false;
				}

				BaseThing operatorThing = _dbc.FindThing<BaseThing>(item.OperatorThingId);
				if (operatorThing == null)
				{
					errMsg = $"The operator thing '{item.OperatorThingId}' do not exists.";
					return false;
				}

				_dbc.ActionDefinitions.Add(
				new GenericAction
				{
					Title = item.Title,
					ServiceDefinitionId = sd.Id,
					ActionListType = actionListType,
					Alarm_ThingId = alarmThing?.Id,
					Object_ThingId = objectThing.Id,
					Operator_ThingId = operatorThing.Id,
					TimeSpan = item.Timespan,

				});
			return true;
			}

		public bool ActivateService(out string errMsg, string thingId, int roleId, string service)
		{
			errMsg = null;

			GenericThing thing =
				_dbc.ThingQuery<GenericThing>(thingId)
				.Include(t => t.ThingRoles)
				.Include(t => t.ServiceDefinitions)
					.ThenInclude(sd => sd.Actions)
				.Where(t => t.ServiceDefinitions.Any(sd => sd.Title == service))
				.FirstOrDefault()
				;

			if (thing == null)
			{
				errMsg = $"Thing '{thingId}' do not exists or do not have service {service}.";
				return false;
			}
			//have to read again, navigation properties did not work as exptected
			ServiceDefinition serviceDefinition = thing.ServiceDefinitions
					.Where(sd => sd.Title == service)
					.SingleOrDefault()
					;


			// create ServiceStatus
			ServiceStatus ss = new ServiceStatus
			{
				ServiceDefinitionId = serviceDefinition.Id,
				SessionId = _session.Session.Id,
				StartedAt = DateTime.UtcNow,
				State = ServiceAndActitivityState.NotStarted,
				ThingId = _session.Session.EntryPoint_ThingId,
				CompletedAt = null,
			};
			_dbc.ServiceStatuses.Add(ss);

			//create ActionStatuses
			foreach (var item in serviceDefinition.Actions)
			{
				var actionStatus = new ActionStatus
				{
					ActionDefinitionId = item.Id,
					DeadLine = item.TimeSpan!=null? ss.StartedAt.Add(item.TimeSpan.Value):(DateTime?) null,
					ServiceStatus = ss,
					State = ServiceAndActitivityState.NotStarted,
					AddedAt = DateTime.UtcNow,
					CompletedAt = null,
				};
				ss.ActionStatuses.Add(actionStatus);
			}

			_dbc.SaveChanges();
			return true;

		}

		public List<ActionStatusResponse> GetActionStatuses(out string errMsg, string thingId, int roleId)
		{
			errMsg = null;
			Entities.BaseThing thing =
				_dbc.ThingQuery<Entities.BaseThing>(thingId)
				.Include(t => t.ThingRoles)
				.FirstOrDefault()
				;

			if (thing == null)
			{
				errMsg = $"Thing '{thingId}' do not exists.";
				return null;
			}

			List<ActionStatusResponse> ret = new List<ActionStatusResponse>();

			var query = _dbc.ActionStatuses
				.Include(acs => acs.ServiceStatus)
				.Include(acs => acs.ActionDefinition)
				.Where(acs => acs.ActionDefinition.Operator_ThingId == thing.Id)  //action is assignt to this thing
				.OrderBy(acs => acs.State)
				.ThenByDescending(acs => acs.DeadLine)
				;

			foreach (var item in query)
			{
				ret.Add(new ActionStatusResponse
				{
					ActionId = item.Id,
					Title = item.ActionDefinition.Title,
					AddedAt = item.AddedAt,
					State = item.State.ToString(),
					ActionClass = item.ActionDefinition.GetType().Name,
					ActionType = item.ActionDefinition.ActionListType.ToString(),
				});
			}
			return ret;
		}

		public Model.Action GetActionStatus(out string errMsg, string thingId, int roleId, Guid actionId)
		{

			errMsg = null;
			Entities.BaseThing thing =
				_dbc.ThingQuery<Entities.BaseThing>(thingId)
				.Include(t => t.ThingRoles)
				.FirstOrDefault()
				;

			if (thing == null)
			{
				errMsg = $"Thing '{thingId}' do not exists.";
				return null;
			}

	
			
			var actionStatus = _dbc.ActionStatuses
				.Include(acs => acs.ActionDefinition)
					.ThenInclude(ad => ad.Alarm_Thing)
				.SingleOrDefault(acs => acs.Id == actionId);

			if (actionStatus == null)
			{
				errMsg = $"Action '{actionId}' do not exists.";
				return null;
			}

			//get Status and update status if needed
			var serviceStatus = UpdateServiceRequestState(actionStatus, null);

			Model.Action ret = new Model.Action
			{
					Id = actionId,
					Title = actionStatus.ActionDefinition.Title,
					ActionClass = actionStatus.ActionDefinition.GetType().Name,
					ActionType = actionStatus.ActionDefinition.ActionListType.ToString(),
					Alarm_ThingId =  _dbc.GetThingStrId(actionStatus.ActionDefinition.Alarm_Thing),
					DeadLine = actionStatus.DeadLine,
					State = actionStatus.State.ToString(),
					ThingId = _dbc.GetThingStrId(actionStatus.ActionDefinition.Operator_Thing),
					Service = new Service
					{
						ThingId = _dbc.GetThingStrId(serviceStatus.ServiceDefinition.Thing),
						AddedAt = serviceStatus.StartedAt,
						Id = serviceStatus.Id,
						RequestorThingId = _dbc.GetThingStrId(serviceStatus.Thing),
						SessionId = serviceStatus.SessionId,
						State = serviceStatus.State.ToString(),
						Title = serviceStatus.ServiceDefinition.Title,
					}
			};

			return ret;

		}

		public bool UpdateActionStatus(out string errMsg, string thingId, int roleId, Guid actionId, string state)
		{
			errMsg = null;
			Entities.BaseThing thing =
				_dbc.ThingQuery<Entities.BaseThing>(thingId)
				.Include(t => t.ThingRoles)
				.FirstOrDefault()
				;

			if (thing == null)
			{
				errMsg = $"Thing '{thingId}' do not exists.";
				return false;
			}

			var actionStatus = _dbc.ActionStatuses
				.Include(acs => acs.ActionDefinition)
					.ThenInclude(ad => ad.Alarm_Thing)
				.SingleOrDefault(acs => acs.Id == actionId);


			if (actionStatus == null)
			{
				errMsg = $"Action '{actionId}' do not exists.";
				return false;
			}

			EnumBL enumBl = new EnumBL();
			int? newState = enumBl.EnumIdFromApiString<ServiceAndActitivityState>(state);
			if (newState==null)
			{
				errMsg = $"Unknown state {state}.";
				return false;
			}
				UpdateServiceRequestState(actionStatus, enumBl.EnumFromApiString<ServiceAndActitivityState>(state).Value);
			return true;

		}

		public List<ServiceStatusResponse> GetServiceStatuses(out string errMsg, string thingId, int roleId, Guid? serviceId)
		{
			errMsg = null;
			GenericThing thing =
								_dbc.ThingQuery<GenericThing>(thingId)
				.Include(t => t.ThingRoles)
				.FirstOrDefault()
				;

			if (thing == null)
			{
				errMsg = $"Thing '{thingId}' do not exists.";
				return null;
			}

			List<ServiceStatusResponse> ret = new List<ServiceStatusResponse>();

			var baseQuery = _dbc.ServiceStatuses
				.Include(ss => ss.ServiceDefinition)
				.Where(ss => ss.ThingId == _session.Session .EntryPoint_ThingId)  //requestor is me
				.Where(ss => ss.ServiceDefinition.ThingId == thing.Id)
				;

			if (serviceId == null)
			{
				var query = baseQuery
					.OrderByDescending(ss => ss.StartedAt)
					.Take(10)
					;

				foreach (var item in query)
				{
					ret.Add(new ServiceStatusResponse
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
					.Where(ss => ss.Id == serviceId.Value)
					.SingleOrDefault()
					;

				if (serviceStatus != null)
				{
					ret.Add(new ServiceStatusResponse
					{
						ServiceId = serviceStatus.Id,
						Title = serviceStatus.ServiceDefinition.Title,
						RequestedAt = serviceStatus.StartedAt,
						State = serviceStatus.State.ToString(),
					});
				}
			}
			return ret;

		}

		public List<string> GetServices(out string errMsg, string thingId, int roleId)
		{
			errMsg = null;
			var thing = _dbc.FindThing<BaseThing>(thingId);
			if (thing == null)
			{
				errMsg = $"The thing '{thingId}'to which ServiceType was created do not exists.";
				return null;
			}

			//TODO: check that session has right to 

			var q = _dbc.ServiceDefinitions
							.Where(sd => sd.ThingId == thing.Id)
							.Select(sd => sd.Title)
							;

			return q.ToList();

		}


		private ServiceStatus UpdateServiceRequestState(ActionStatus actionStatus, ServiceAndActitivityState? newActionState)
		{

			var thisServiceStatus = _dbc.ServiceStatuses
				.Include(ss => ss.Thing)
				.Include(ss => ss.ActionStatuses)
					.ThenInclude(acs => acs.ActionDefinition)
				.Include(ss => ss.ServiceDefinition)
					.ThenInclude(sd => sd.Thing)
				.Single(ss => ss.Id == actionStatus.ServiceStatusId)
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
					item.State = ServiceAndActitivityState.NotDoneInTime;
				}
				if (q.Count() > 0)
				{
					thisServiceStatus.State = ServiceAndActitivityState.NotDoneInTime;
				}
			}

			List<ServiceAndActitivityState> states;
			// check if it service has been started
			if (thisServiceStatus.State == ServiceAndActitivityState.NotStarted)
			{
				states = new List<ServiceAndActitivityState>
				{
					ServiceAndActitivityState.Done, ServiceAndActitivityState.Started,
				};
				var q = thisServiceStatus.ActionStatuses
					.Where(acs => states.Contains(acs.State))
					;
				if (q.Count() > 0)
				{
					thisServiceStatus.State = ServiceAndActitivityState.Started;
				}
			}

			// check if done, all mandatory done in time
			if (thisServiceStatus.State == ServiceAndActitivityState.Started)
			{
				var q = thisServiceStatus.ActionStatuses
					.Where(acs => acs.State != ServiceAndActitivityState.Done)
					.Where(acs => acs.ActionDefinition.ActionListType == ActionListType.Mandatory)
					;
				if (q.Count() < 1)
				{
					thisServiceStatus.State = ServiceAndActitivityState.Done;
				}
			}


			_dbc.SaveChanges();
			return thisServiceStatus;
		}

		private bool IsStateNotFinneshed(ServiceAndActitivityState state)
		{
			return state == ServiceAndActitivityState.NotStarted || state == ServiceAndActitivityState.Started;
		}


	}
}