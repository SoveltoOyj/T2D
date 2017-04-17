using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using T2D.Model.Enums;
using T2D.Model.InventoryApi;

namespace T2D.Model.ServiceApi
{
	public class CreateServiceTypeRequest : BaseRequest
	{
		/// <summary>
		/// Title of Service.
		/// </summary>
		[Required]
		public string Title { get; set; }

		public List<ActionDefinition> MandatoryActions { get; set; }
		public List<ActionDefinition> OptionalActions { get; set; }
		public List<ActionDefinition> SelectedActions { get; set; }
		public List<ActionDefinition> PendingActions { get; set; }
	}

	public class ActionDefinition
	{
		/// <summary>
		/// Type of the Action. Enum value.
		/// </summary>
		public ActionType ActionType { get; set; }

		/// <summary>
		/// The title of Action.
		/// </summary>
		[Required]
		public string Title { get; set; }

		/// <summary>
		/// The thing that will be send an alarm if action is overtime.
		/// </summary>
		public string AlarmThingId { get; set; }

		/// <summary>
		/// The thing to which this action should be done.
		/// </summary>
		[Required]
		public string ObjectThingId { get; set; }

		/// <summary>
		/// The thing that should execute this Action.
		/// </summary>
		[Required]
		public string OperatorThingId { get; set; }

		/// <summary>
		/// In which time this Action should be done.
		/// </summary>
		public TimeSpan? Timespan { get; set; }
	}


}
