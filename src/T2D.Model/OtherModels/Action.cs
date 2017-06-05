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
		[ThingId]
		public string ThingId { get; set; }



		public string ActionClass { get; set; }

		public string State { get; set; }

	}
}
