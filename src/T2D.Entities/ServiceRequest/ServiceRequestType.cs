using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace T2D.Entities
{
	public class ServiceRequestType : IEntity
	{
		public Guid Id { get; set; }


		[StringLength(256), Required]
		public string Title { get; set; }

		public Guid ThingId { get; set; }
		public BaseThing Thing { get; set; }

		/* pitää miettiä miten tehdään
		public List<ServiceRequestAction> Mandatories { get; set; }
		public List<ServiceRequestAction> Optionals { get; set; }
		public List<ServiceRequestAction> Selecteds { get; set; }
		public List<ServiceRequestAction> Pendings { get; set; }
		*/
	}

	public class ServiceRequestAction:IEntity
	{
		public Guid Id { get; set; }
		public Guid ServiceRequestTypeId { get; set; }
		public ServiceRequestType ServiceRequest { get; set; }

		public Guid ActionTypeId { get; set; }
		public ActionType ActionType { get; set; }

		public Guid ThingId { get; set; }
		public BaseThing Thing { get; set; }

		public Guid Alarm_ThingId { get; set; }
		public BaseThing Alarm_Thing { get; set; }


		public TimeSpan TimeSpan { get; set; }

		public int ActionKind { get; set; }
	}

	public enum ActionKindEnum
	{
		Mandatory,
		Optional,
		Selected,
		Pending,
	}



}