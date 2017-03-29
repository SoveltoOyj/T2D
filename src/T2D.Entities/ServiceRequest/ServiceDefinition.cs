using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace T2D.Entities
{
	public class ServiceDefinition : IEntity
	{
		public Guid Id { get; set; }


		[StringLength(256), Required]
		public string Title { get; set; }

		public Guid ThingId { get; set; }
		public GenericThing Thing { get; set; }

		public List<ActionDefinition> Actions { get; set; }

		public ServiceDefinition()
		{
			Actions = new List<ActionDefinition>();
		}
	}
}