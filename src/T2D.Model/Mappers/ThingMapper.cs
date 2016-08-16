using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T2D.Model.Mappers
{
	public static class ThingMapper
	{
		public static long FromModelId(string id)
		{
			return long.Parse(id);
		}

		public static string FromEntityId(long id)
		{
			return id.ToString();
		}

		public static Model.Thing EntityToModel(this Entities.Thing from)
		{
			return new Model.Thing
			{
				Id =  ThingMapper.FromEntityId(from.Id),
				Name = from.Name,
			};
		}
		public static Entities.Thing ModelToEntity(this Model.Thing from)
		{
			return new Entities.Thing
			{
				Id = ThingMapper.FromModelId(from.Id),
				Name = from.Name,
			};
		}

		/// <summary>
		/// Updates entity from model. All properties except id.
		/// </summary>
		/// <param name="to">Entity to update. All properties except Id.</param>
		/// <param name="from">Model where data is from.</param>
		public static void UpdateEntityFromModel(this Entities.Thing to, Model.Thing from)
		{
				to.Name = from.Name;
		}


	}
}
