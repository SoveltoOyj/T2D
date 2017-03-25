using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace T2D.Entities
{
   public class ActionType : IEntity
	{
		public Guid Id { get; set; }

		[StringLength(256), Required]
		public string Title { get; set; }

	}

	public class GenericActionType : ActionType { }
	public class PaymentRequestActionType : ActionType { }
	public class ReceiptRequestActionType : ActionType { }
	public class IoTBotRequestActionType : ActionType { }
	public class ServiceRequestActionType : ActionType { }

}