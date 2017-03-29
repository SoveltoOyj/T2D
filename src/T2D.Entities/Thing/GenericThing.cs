using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace T2D.Entities
{
    public class GenericThing:BaseThing
    {
		public DateTime? Created { get; set; }
		public DateTime? Published { get; set; }
		public DateTime? Modified { get; set; }


		[StringLength(256)]
		public string Creator_Fqdn { get; set; }
		[StringLength(512)]
		public string Creator_US { get; set; }

		[StringLength(256)]
		public string Archetype_Fqdn { get; set; }
		[StringLength(512)]
		public string Archetype_US { get; set; }

		[StringLength(256)]
		public string Parted_Fqdn { get; set; }
		[StringLength(512)]
		public string Parted_US { get; set; }

		public List<ServiceDefinition> ServiceDefinitions { get; set; }

		public GenericThing()
		{
			ServiceDefinitions = new List<ServiceDefinition>();
		}
	}
}
