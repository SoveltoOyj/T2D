using System;
using System.Collections.Generic;
using System.Text;

namespace T2D.Model
{
	public class Service : IModel
	{
		public Guid Id { get; set; }

		public string Title { get; set; }

		/// <summary>
		/// The thing which Service this is
		/// </summary>
		[ThingId]
		public string ThingId { get; set; }

		/// <summary>
		/// The thing that will get an alarm if this action is overdue
		/// </summary>
		[ThingId]
		public string Alarm_ThingId { get; set; }

		public DateTime? DeadLine { get; set; }

		/// <summary>
		/// Requestor Thing
		/// </summary>
		public string RequestorThingId { get; set; }

		public string State { get; set; }

		public Guid SessionId { get; set; }

		public DateTime AddedAt { get; set; }

	}
}
