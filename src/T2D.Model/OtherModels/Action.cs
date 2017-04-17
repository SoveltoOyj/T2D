using System;
using System.Collections.Generic;
using System.Text;

namespace T2D.Model
{
    public class Action : IModel
	{
		/// <summary>
		/// Action ID (Guid)
		/// </summary>
		public Guid Id { get; set; }

		public Service Service { get; set; }

		public string	Title { get; set; }
		public string  ActionType { get; set; }

		/// <summary>
		/// The Thing who should do this action
		/// </summary>
		public string ThingId { get; set; }


		/// <summary>
		/// THe thing that will get an alarm if this action is overdue
		/// </summary>
		public string Alarm_ThingId { get; set; }

		public DateTime? DeadLine { get; set; }

		public string ActionClass { get; set; }

		public string State { get; set; }

	}
}
