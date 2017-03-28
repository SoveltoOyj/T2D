using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace T2D.Entities
{
	/// <summary>
	/// ActionDefinition will be copied here to enable that Service Type can change during Service Request.
	/// </summary>
	public class ActionStatus:IEntity
	{
		public Guid Id { get; set; }

		public ServiceStatus ServiceStatusId { get; set; }
		public ServiceStatus ServiceStatus { get; set; }

		public Guid ActionTypeId { get; set; }
		public ActionTypeBase ActionType { get; set; }

		public Guid ThingId { get; set; }
		public BaseThing Thing { get; set; }

		public Guid Alarm_ThingId { get; set; }
		public BaseThing Alarm_Thing { get; set; }

		public DateTime	DeadLine { get; set; }

		public ActionListType ActionListType { get; set; }

		public State State { get; set; }

	}


}