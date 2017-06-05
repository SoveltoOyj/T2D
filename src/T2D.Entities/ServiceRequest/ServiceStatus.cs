using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace T2D.Entities
{
   public class ServiceStatus : IEntity
	{
		public Guid Id { get; set; }

		public Guid ServiceDefinitionId { get; set; }
		public ServiceDefinition ServiceDefinition { get; set; }

		/// <summary>
		/// Requestor ThingId
		/// </summary>
		public Guid ThingId { get; set; } 
		public BaseThing Thing { get; set; }


		public ServiceAndActitivityState State{ get; set; }

		public Guid SessionId { get; set; }

		public DateTime StartedAt { get; set; }
		public DateTime? CompletedAt { get; set; }

		public DateTime? DeadLine { get; set; }

		public Guid? AlarmThingId { get; set; }
		public BaseThing AlarmThing { get; set; }


		public List<ActionStatus> ActionStatuses { get; set; }

		public ServiceStatus()
		{
			ActionStatuses = new List<ActionStatus>(); 
		}

	}
	
}