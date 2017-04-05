using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using T2D.Helpers;

namespace T2D.Entities
{
	public class FunctionalStatus:IEnumEntity
	{
		public int Id { get; set; }
        [MaxLength(256)]
		public string Name { get; set; }
		public override string ToString()
		{
			return this.ToJson();
		}


	}

	public enum FunctionalStatusEnum
	{
	 NormalOperational = 1,
     Template,
     DestroyedOrDeleted,
     Planned,
     WaitingForService,
     Malfunctionioning,
     Missing,
     Danger,
     AvailableForRentOrSale,
	};
}
