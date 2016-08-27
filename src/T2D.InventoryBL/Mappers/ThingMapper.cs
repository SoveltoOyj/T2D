using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T2D.Entities;
using T2D.Helpers;
using T2D.Model;

namespace T2D.InventoryBL.Mappers
{
	/// <summary>
	/// Note: this mapper uses Model.ThingId also for Entity key!
	/// </summary>
	public class ThingMapper : IThingMapper<Entities.RegularThing, Model.Thing, ThingId>
	{

		public ThingId FromModelId(ThingId id)
		{
			return id;
		}

		public ThingId FromEntityId(ThingId id)
		{
			return id;
		}

		public Model.Thing EntityToModel(Entities.RegularThing from)
		{
			return new Model.Thing
			{
				Id = ThingId.Create(from.Id_CreatorUri, from.Id_UniqueString),
				Width = ((float)from.Widthmm) / 1000f,
				Height = ((float)from.Heightmm) / 1000f,
			};
		}
		public Entities.RegularThing ModelToEntity(Model.Thing from)
		{
			return new Entities.RegularThing
			{
				Id_CreatorUri = from.Id.CreatorUri.ToString(),
				Id_UniqueString=from.Id.UniqueString,
				Widthmm = (long)(from.Width * 1000f),
				Heightmm = (long)(from.Height* 1000f),
			};
		}

		/// <summary>
		/// Updates entity from model. All properties except id.
		/// </summary>
		/// <param name="to">Entity to update. All properties except Id.</param>
		/// <param name="from">Model where data is from.</param>
		public Entities.RegularThing UpdateEntityFromModel(Model.Thing from, Entities.RegularThing to)
		{
			to.Widthmm = (long)(from.Width * 1000f);
			to.Heightmm = (long)(from.Height * 1000f);
			return to;
		}

		public RegularThing UpdateEntityFromModel(Thing from, RegularThing to, bool updateAlsoId)
		{
			if (updateAlsoId)
			{
				to.Id_CreatorUri = from.Id.CreatorUri.ToString();
				to.Id_UniqueString = from.Id.UniqueString;
			}
			UpdateEntityFromModel(from, to);
			return to;
		}

		public string ModelToEntityPropertyName(string from)
		{
			string ret;
			ret = from.TestEquality("Height");
			if (ret != null) return "Heightmm";

			ret = from.TestEquality("Width");
			if (ret != null) return "Widthmm";

			ret = from.TestEquality("id");
			if (ret != null) return "Id_CreatorUri, Id_UniqueString";
			return null;
		}
		public string EntityToModelPropertyName(string from)
		{
			string ret;
			ret = from.TestEquality("Heightmm");
			if (ret != null) return "height";

			ret = from.TestEquality("Widthmm");
			if (ret != null) return "widthmm";

			ret = from.TestEquality("id");
			if (ret != null) return "Id_CreatorUri, Id_UniqueString";
			return null;
		}

	}
}

