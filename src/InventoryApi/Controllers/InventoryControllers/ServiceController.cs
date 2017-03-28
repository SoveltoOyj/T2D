﻿using InventoryApi.Controllers.BaseControllers;
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
			GetServicesResponse ret = new GetServicesResponse
			{
				Services = new List<string>
				{
					"Huollettava, lamppu sammunut",
					"Hämäräkytkin/ajastusongelma"
				}
			};
			return Ok(ret);
		}

		[HttpPost, ActionName("ServiceRequest")]
		//[Produces(typeof(ServiceRequestResponse))]
		public IActionResult ServiceRequest([FromBody]ServiceRequestRequest value)
		{
			return Ok();
		}

		[HttpPost, ActionName("GetServiceStatus")]
		[Produces(typeof(GetServiceStatusResponse))]
		public IActionResult GetServiceStatus([FromBody]GetServiceStatusRequest value)
		{
			GetServiceStatusResponse ret = new GetServiceStatusResponse
			{
				Statuses = new List<ServiceStatusResponse>()
			};
			var rnd = new Random();

			if (value.ServiceId == null)
			{
				int count = rnd.Next(11);
				for (int i = 0; i < count; i++)
				{
					ret.Statuses.Add(new ServiceStatusResponse
					{
						ServiceId = Guid.NewGuid(),
						Title = "Service Title...",
						RequestedAt = DateTime.Now.AddSeconds(rnd.NextDouble() * -1000.0),
						State = this.StateMapper.EnumToEntity((T2D.Entities.StateEnum)(rnd.Next(5) + 1)).Name,
					});
				}
			}
			else
			{
				ret.Statuses.Add(new ServiceStatusResponse
				{
					ServiceId = value.ServiceId.Value,
					Title = "Service Title...",
					RequestedAt = DateTime.Now.AddSeconds(rnd.NextDouble() * -1000.0),
					State = this.StateMapper.EnumToEntity((T2D.Entities.StateEnum)(rnd.Next(5) + 1)).Name,
				});
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
