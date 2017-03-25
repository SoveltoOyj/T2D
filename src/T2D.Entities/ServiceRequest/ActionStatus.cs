using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace T2D.Entities
{
	public class ActionStatus:IEntity
	{
		public Guid Id { get; set; }

		public ServiceRequestInstance ServiceRequestInstanceId { get; set; }
		public ServiceRequestInstance ServiceRequestInstance { get; set; }

		public Guid ActionTypeId { get; set; }
		public ActionTypeBase ActionType { get; set; }

		public int ActionKind { get; set; }

		public Guid ThingId { get; set; }
		public BaseThing Thing { get; set; }

		public Guid Alarm_ThingId { get; set; }
		public BaseThing Alarm_Thing { get; set; }

		public DateTime	DeadLine { get; set; }

		public ActionState ActionStatusEnum { get; set; }

	}

	public enum ActionState
	{
		NotStarted,
		Started,
		DoneOk,
		Failed,
	}

}