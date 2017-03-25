using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace T2D.Entities
{
   public class ServiceRequestInstance : IEntity
	{
		public Guid Id { get; set; }

		public Guid ServiceRequestTypeId { get; set; }
		public ServiceType ServiceRequestType { get; set; }

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

		public int Status { get; set; }

		public Guid SessionId { get; set; }

		public DateTime StartedAt { get; set; }

	}
	
}