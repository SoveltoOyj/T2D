using System;
using System.Collections.Generic;
using System.Text;

namespace T2D.Model
{
    public class Action : IModel
	{
		public Guid Id { get; set; }

		public Service Service { get; set; }

		public string	Title { get; set; }
		public string  ActionType { get; set; }

		public string ThingId { get; set; }

		public string Alarm_ThingId { get; set; }

		public DateTime DeadLine { get; set; }

		public string ActionListType { get; set; }

		public string State { get; set; }

	}
}
