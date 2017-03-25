using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace T2D.Entities
{
	
	public class ActionDefinition:IEntity
	{
		public Guid Id { get; set; }
		public Guid ServiceDefinitionId { get; set; }
		public ServiceDefinition ServiceDefinition { get; set; }

		public Guid ActionTypeId { get; set; }
		public ActionTypeBase ActionType { get; set; }

		public Guid ThingId { get; set; }
		public BaseThing Thing { get; set; }

		public Guid Alarm_ThingId { get; set; }
		public BaseThing Alarm_Thing { get; set; }


		public TimeSpan TimeSpan { get; set; }

		public ActionListType ActionListType { get; set; }
	}

	public enum ActionListType
	{
		Mandatory,
		Optional,
		Selected,
		Pending,
	}



}