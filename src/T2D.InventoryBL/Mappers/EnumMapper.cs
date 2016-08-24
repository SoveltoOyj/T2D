using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using T2D.Entities;
using T2D.Model;

namespace T2D.InventoryBL.Mappers
{
	public class EnumMapper<TEnumEntity, TEnumModel> : IMapper<TEnumEntity, TEnumModel, long, string>
		where TEnumEntity : class, IEnumEntity, new()
		where TEnumModel : class, IEnumModel, new()
	{
		public TEnumModel EntityToModel(TEnumEntity from)
		{
			return new TEnumModel { Id = FromEntityId(from.Id), Name = from.Name } ;
		}

		public string FromEntityId(long id)
		{
			return id.ToString();
		}

		public long FromModelId(string id)
		{
			long ret=0;
			if (!long.TryParse(id, out ret))
				throw new Exception($"Cant't convert {id} to enum long id");
			return ret;
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
