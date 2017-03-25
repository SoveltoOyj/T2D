using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace T2D.Entities
{
	public class ServiceType : IEntity
	{
		public Guid Id { get; set; }


		[StringLength(256), Required]
		public string Title { get; set; }

		public Guid ThingId { get; set; }
		public BaseThing Thing { get; set; }

		/* pitää miettiä miten tehdään
		public List<ServiceRequestAction> Mandatories { get; set; }
		public List<ServiceRequestAction> Optionals { get; set; }
		public List<ServiceRequestAction> Selecteds { get; set; }
		public List<ServiceRequestAction> Pendings { get; set; }
		*/
	}
}