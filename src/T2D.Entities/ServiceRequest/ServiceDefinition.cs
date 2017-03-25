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
		public BaseThing Thing { get; set; }

		/* pitää miettiä miten tehdään
		public List<ActionDefinition> Mandatories { get; set; }
		public List<ActionDefinition> Optionals { get; set; }
		public List<ActionDefinition> Selecteds { get; set; }
		public List<ActionDefinition> Pendings { get; set; }
		*/
	}
}