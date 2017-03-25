using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace T2D.Entities
{
   public abstract class ActionTypeBase : IEntity
	{
		public Guid Id { get; set; }

		[StringLength(256), Required]
		public string Title { get; set; }

	}

	public class GenericAction : ActionTypeBase { }
	public class PaymentRequestAction : ActionTypeBase { }
	public class ReceiptRequestAction : ActionTypeBase { }
	public class IoTBotRequestAction : ActionTypeBase { }
	public class ServiceRequestAction : ActionTypeBase { }

}