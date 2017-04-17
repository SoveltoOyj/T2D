using System;
using System.Collections.Generic;
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

		}
	}