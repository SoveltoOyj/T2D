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
			GetActionStatusesResponse ret = new GetActionStatusesResponse
			{
				Statuses = new List<ActionStatusResponse>()
			};
			var rnd = new Random();

			
				int count = rnd.Next(11);
				for (int i = 0; i < count; i++)
				{
					ret.Statuses.Add(new ActionStatusResponse
					{
						ActionId = Guid.NewGuid(),
						Title = "Action Title...",
						AddedAt = DateTime.Now.AddSeconds(rnd.NextDouble() * -1000.0),
						State = this.StateMapper.EnumToEntity((T2D.Entities.StateEnum)(rnd.Next(2) + 1)).Name,
					});
				}

				count = rnd.Next(11);
				for (int i = 0; i < count; i++)
				{
					ret.Statuses.Add(new ActionStatusResponse
					{
						ActionId = Guid.NewGuid(),
						Title = "Action Title...",
						AddedAt = DateTime.Now.AddSeconds(rnd.NextDouble() * -1000.0),
						State = this.StateMapper.EnumToEntity((T2D.Entities.StateEnum)(rnd.Next(3) + 1)).Name,
					});
				}

			
			return Ok(ret);
		}

		[HttpPost, ActionName("GetActionStatus")]
		[Produces(typeof(GetActionStatusResponse))]
		public IActionResult GetActionStatus([FromBody]GetActionStatusRequest value)
		{
			var rnd = new Random();
			var state = StateMapper.EnumToEntity((T2D.Entities.StateEnum)(rnd.Next(4) + 1)).Name;

			var ret = new GetActionStatusResponse
			{
				Action=new T2D.Model.Action
				{
					Id=value.ActionId,
					Title="Action Title...",
					ActionListType="Mandatory",
					ActionType = "GenericAction",
					Alarm_ThingId="inv1.sovelto.fi/M100",
					DeadLine=DateTime.Now.AddHours(rnd.Next(20)),
					State = state,
					ThingId = "inv1.sovelto.fi/M100",
					Service = new Service
					{
						ThingId = "inv1.sovelto.fi/SL1",
						AddedAt = DateTime.Now.AddHours(-1*rnd.Next(20)),
						Id = Guid.NewGuid(),
						RequestorThingId = "Anonymous",
						SessionId=Guid.NewGuid(),
						State = state,
						Title="Service title..",
					}
				}
			};

			return Ok(ret);
		}

		[HttpPost, ActionName("UpdateActionStatus")]
		public IActionResult UpdateActionStatus([FromBody]UpdateActionStatusRequest value)
		{
			return Ok();

		}


	}
}
