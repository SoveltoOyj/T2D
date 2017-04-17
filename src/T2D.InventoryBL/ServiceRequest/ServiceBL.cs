using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using T2D.Entities;
using T2D.Infra;
using T2D.Model.ServiceApi;

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
			foreach (var item in thing.ServiceDefinitions.First().Actions)
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
	}
	}