using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T2D.Entities;
using T2D.Model;

namespace T2D.InventoryBL.Mappers
{
	public class ThingMapper : IMapper<Entities.RegularThing, Model.Thing, long, string>
	{

		public long FromModelId(string id)
		{
			return long.Parse(id);
		}

		public string FromEntityId(long id)
		{
			return id.ToString();
		}

		public Model.Thing EntityToModel(Entities.RegularThing from)
		{
			return new Model.Thing
			{
				Id = this.FromEntityId(from.Id),
				Name = from.Name,
				Version = from.Version,
			};
		}
		public Entities.RegularThing ModelToEntity(Model.Thing from)
		{
			return new Entities.RegularThing
			{
				Id = this.FromModelId(from.Id),
				Name = from.Name,
				Version = from.Version,
			};
		}

		/// <summary>
		/// Updates entity from model. All properties except id.
		/// </summary>
		/// <param name="to">Entity to update. All properties except Id.</param>
		/// <param name="from">Model where data is from.</param>
		public Entities.RegularThing UpdateEntityFromModel(Model.Thing from, Entities.RegularThing to)
		{
			to.Name = from.Name;
			to.Version = from.Version;
			return to;
		}

	}
}

