using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace T2D.Entities
{
	
	public abstract class ActionDefinition:IEntity
	{
		public Guid Id { get; set; }

		[StringLength(256), Required]
		public string Title { get; set; }

		public Guid ServiceDefinitionId { get; set; }
		public ServiceDefinition ServiceDefinition { get; set; }

		/// <summary>
		/// To which thing this action should be executed.
		/// </summary>
		public Guid Object_ThingId { get; set; }
		public BaseThing Object_Thing { get; set; }

		public Guid Operator_ThingId { get; set; }
		public BaseThing Operator_Thing { get; set; }

		public Guid? Alarm_ThingId { get; set; }
		public BaseThing Alarm_Thing { get; set; }

		public TimeSpan? TimeSpan { get; set; }

		public ActionListType ActionListType { get; set; }
	}


	public class GenericAction : ActionDefinition { }
	public class PaymentRequestAction : ActionDefinition { }
	public class ReceiptRequestAction : ActionDefinition { }
	public class IoTBotRequestAction : ActionDefinition { }
	public class ServiceRequestAction : ActionDefinition { }


	public enum ActionListType
	{
		Mandatory,
		Optional,
		Selected,
		Pending,
	}



}