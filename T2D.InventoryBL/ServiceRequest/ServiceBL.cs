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
			BaseThing alarmThing = null;
			if (!string.IsNullOrWhiteSpace(request.AlarmThingId))
			{
				alarmThing = _dbc.FindThing<BaseThing>(request.AlarmThingId);
				if (alarmThing == null)
				{
					errMsg = $"The alarm thing '{request.AlarmThingId}' do not exists.";
					return false;
				}
			}

			//create ServiceDefinition
			var sd = new ServiceDefinition
			{
				ThingId = toThing.Id,
				Title = request.Title,
				TimeSpan = request.Timespan,
				Alarm_ThingId = alarmThing?.Id,
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
					Object_ThingId = objectThing.Id,
					Operator_ThingId = operatorThing.Id,

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
			//have to read again, navigation properties did not work as excpected
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
				DeadLine = serviceDefinition.TimeSpan != null ? DateTime.UtcNow.Add(serviceDefinition.TimeSpan.Value) : (DateTime?)null,
				AlarmThingId = serviceDefinition.Alarm_ThingId,
			};
			_dbc.ServiceStatuses.Add(ss);

			//create ActionStatuses
			foreach (var item in serviceDefinition.Actions)
			{
				var actionStatus = new ActionStatus
				{
					ActionDefinitionId = item.Id,
					ServiceStatus = ss,
					State = ServiceAndActitivityState.NotStarted,
					AddedAt = DateTime.UtcNow,
					CompletedAt = null,
					ActionType = item.ActionListType,
				};
				ss.ActionStatuses.Add(actionStatus);
			}

			_dbc.SaveChanges();

			//start watch job if timespan
			if (ss.DeadLine != null)
			{
				double minutes = ss.DeadLine.Value.Subtract(DateTime.UtcNow).TotalMinutes ;
				string connStr = _dbc.Database.GetDbConnection().ConnectionString;
				Hangfire.BackgroundJob.Schedule(() => ServiceBL.ScheduleUpdateServiceStatus(connStr, ss.Id) , TimeSpan.FromMinutes(minutes));
			}
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


			var actionStatuses = _dbc.ActionStatuses
				.Include(acs => acs.ServiceStatus)
				.Include(acs => acs.ActionDefinition)
				.Where(acs => acs.ActionDefinition.Operator_ThingId == thing.Id)  //action is assigned to this thing
				.ToList()
				;

			//add alarm and failed statuses
			actionStatuses.AddRange(_dbc.ActionStatuses
				.Include(acs => acs.ServiceStatus)
				.Include(acs => acs.ActionDefinition)
				.Where(acs => acs.ActionType == ActionListType.Alarm || acs.ActionType == ActionListType.Failed)
				.Where(acs => acs.ServiceStatus.AlarmThingId == thing.Id)  //action is assigned to this thing
				.ToList()
				);


			foreach (var item in actionStatuses.OrderBy(ass=>ass.State))
			{
				ret.Add(new ActionStatusResponse
				{
					ActionId = item.Id,
					Title = item.ActionDefinition != null? item.ActionDefinition.Title: item.Description,
					AddedAt = item.AddedAt,
					State = item.State.ToString(),
					ActionClass = item.ActionDefinition != null ? item.ActionDefinition.GetType().Name: "",
					ActionType = item.ActionType.ToString(),
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
				.SingleOrDefault(acs => acs.Id == actionId);

			if (actionStatus == null)
			{
				errMsg = $"Action '{actionId}' do not exists.";
				return null;
			}

			//get Status and update status if needed
			var serviceStatus = UpdateServiceRequestState(actionStatus.ServiceStatusId);

			Model.Action ret = new Model.Action
			{
					Id = actionId,
					Title = actionStatus.ActionDefinition!=null? actionStatus.ActionDefinition.Title: "",
					ActionClass = actionStatus.ActionDefinition != null? actionStatus.ActionDefinition.GetType().Name: "Alarm",
					ActionType = actionStatus.ActionType.ToString(),
					Description = actionStatus.Description,
					State = actionStatus.State.ToString(),
					ThingId = actionStatus.ActionDefinition!=null? _dbc.GetThingStrId(actionStatus.ActionDefinition.Operator_Thing): _dbc.GetThingStrId(serviceStatus.AlarmThing),
					Service = new Service
					{
						ThingId = _dbc.GetThingStrId(serviceStatus.ServiceDefinition.Thing),
						AddedAt = serviceStatus.StartedAt,
						Id = serviceStatus.Id,
						RequestorThingId = _dbc.GetThingStrId(serviceStatus.Thing),
						SessionId = serviceStatus.SessionId,
						State = serviceStatus.State.ToString(),
						Title = serviceStatus.ServiceDefinition.Title,
						DeadLine = serviceStatus.DeadLine,
						Alarm_ThingId = _dbc.GetThingStrId(serviceStatus.AlarmThing)
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
				actionStatus.State = enumBl.EnumFromApiString<ServiceAndActitivityState>(state).Value ;
			_dbc.SaveChanges();

			UpdateServiceRequestState(actionStatus.ServiceStatusId);
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
						DeadLine = item.DeadLine,
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
						DeadLine = serviceStatus.DeadLine,
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


		/// <summary>
		/// This method is called also from Scheduled jobs. Session is then null.
		/// </summary>
		/// <param name="serviceStatusId"></param>
		/// <returns></returns>
		public ServiceStatus UpdateServiceRequestState(Guid serviceStatusId)
		{

			var thisServiceStatus = _dbc.ServiceStatuses
				.Include(ss => ss.Thing)
				.Include(ss => ss.ActionStatuses)
					.ThenInclude(acs => acs.ActionDefinition)
				.Include(ss => ss.ServiceDefinition)
					.ThenInclude(sd => sd.Thing)
				.Include(ss=>ss.AlarmThing)
				.Single(ss => ss.Id == serviceStatusId)
				;



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

			// if started, check if done in time
			if (thisServiceStatus.State == ServiceAndActitivityState.Started)
			{
				// check if all mandatory are done
				bool allMandatoryAreDone = true;
				var q = thisServiceStatus.ActionStatuses
					.Where(acs => acs.State != ServiceAndActitivityState.Done)
					.Where(acs => acs.ActionDefinition.ActionListType == ActionListType.Mandatory)
					;
				allMandatoryAreDone = q.Count() < 1;

				//exactly one of selected is done
				bool oneOfSelectedIsDone = false;
				q = thisServiceStatus.ActionStatuses
					.Where(acs => acs.ActionDefinition.ActionListType == ActionListType.Selected)
					;
				var count = q.Count(acs => acs.State == ServiceAndActitivityState.Done);
				if ((q.Count() > 0 &&  count == 1) || q.Count() < 1)
				{
					oneOfSelectedIsDone = true;
				}
				else if (count > 1)
				{
					SetFailed(thisServiceStatus, "More than one of selected actions are done.");
					return thisServiceStatus;
				}
				else {
					oneOfSelectedIsDone = false;
				}

				if (thisServiceStatus.DeadLine != null && thisServiceStatus.DeadLine.Value < DateTime.UtcNow)
				{
					SetNotDoneInTime(thisServiceStatus);
				}
				else if (allMandatoryAreDone && oneOfSelectedIsDone)
				{
					thisServiceStatus.State = ServiceAndActitivityState.Done;
				}
			}
			if (thisServiceStatus.State == ServiceAndActitivityState.NotStarted && thisServiceStatus.DeadLine != null && thisServiceStatus.DeadLine.Value < DateTime.UtcNow)
			{
				SetNotDoneInTime(thisServiceStatus);
			}

			_dbc.SaveChanges();
			return thisServiceStatus;
		}

		private bool IsStateNotFinnished(ServiceAndActitivityState state)
		{
			return state == ServiceAndActitivityState.NotStarted || state == ServiceAndActitivityState.Started;
		}

		private void SetNotDoneInTime(ServiceStatus serviceStatus)
		{
			serviceStatus.State = ServiceAndActitivityState.NotDoneInTime;
			if (serviceStatus.AlarmThingId != null)
			{
				var actionStatus = new ActionStatus
				{
					ActionDefinitionId = null,
					ServiceStatus = serviceStatus,
					State = ServiceAndActitivityState.NotStarted,
					AddedAt = DateTime.UtcNow,
					CompletedAt = null,
					ServiceStatusId = serviceStatus.Id,
					ActionType = ActionListType.Alarm,
					Description = "ServiceRequest is not done in time."
				};
				 _dbc.ActionStatuses.Add(actionStatus);
				_dbc.SaveChanges();
			}
		}
		private void SetFailed(ServiceStatus serviceStatus, string msg)
		{
			serviceStatus.State = ServiceAndActitivityState.NotDoneInTime;
			if (serviceStatus.AlarmThingId != null)
			{
				var actionStatus = new ActionStatus
				{
					ActionDefinitionId = null,
					ServiceStatus = serviceStatus,
					State = ServiceAndActitivityState.NotStarted,
					AddedAt = DateTime.UtcNow,
					CompletedAt = null,
					ServiceStatusId = serviceStatus.Id,
					ActionType = ActionListType.Failed,
					Description = msg,
				};
				_dbc.ActionStatuses.Add(actionStatus);
				_dbc.SaveChanges();
			}
		}

		public static bool ScheduleUpdateServiceStatus(string connectionStr, Guid serviceStatusId)
		{
			EfContext ctx = new EfContext(connectionStr);
			ServiceBL serviceBl = new ServiceBL(ctx,null);

			serviceBl.UpdateServiceRequestState(serviceStatusId);
			return true;
		}

	}

}