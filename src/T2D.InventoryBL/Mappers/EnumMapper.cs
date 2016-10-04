using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using T2D.Entities;
using T2D.Helpers;
using T2D.Model;

namespace T2D.InventoryBL.Mappers
{
	public class EnumMapper<TEnumEntity, TEnumModel> : IMapper<TEnumEntity, TEnumModel, long, long>
		where TEnumEntity : class, IEnumEntity, new()
		where TEnumModel : class, IEnumModel, new()
	{
		public TEnumModel EntityToModel(TEnumEntity from)
		{
			return new TEnumModel { Id = FromEntityId(from.Id), Name = from.Name } ;
		}

		public long FromEntityId(long id)
		{
			return id;
		}

		public long FromModelId(long id)
		{
			return id;
		}

		public TEnumEntity ModelToEntity(TEnumModel from)
		{
			return new TEnumEntity { Id = FromModelId(from.Id), Name = from.Name };
		}

		public TEnumEntity UpdateEntityFromModel(TEnumModel from, TEnumEntity to)
		{
			to.Name = from.Name;
			return to;
		}
	}
}
