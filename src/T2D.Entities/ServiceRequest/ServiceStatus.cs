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

		/* pitää miettiä kuinka toteutetaan
		public List<ActionStatus> Mandatories { get; set; }
		public List<ActionStatus> Optionals { get; set; }
		public List<ActionStatus> Selecteds { get; set; }
		public List<ActionStatus> Pendings { get; set; }
		*/

		public StateEnum State{ get; set; }

		public Guid SessionId { get; set; }

		public DateTime StartedAt { get; set; }

		public List<ActionStatus> ActionStatuses { get; set; }

		public ServiceStatus()
		{
			ActionStatuses = new List<ActionStatus>(); 
		}

	}
	
}