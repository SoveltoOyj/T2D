using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using T2D.Model;

namespace T2D.InventoryBL.Mappers
{
	public class EnumMapper<TEnum, TEntity> : IEnumMapper<TEnum, TEntity>
	where TEnum : struct, IConvertible
	where TEntity : class, T2D.Entities.IEnumEntity, new()
	{
		public TEnum EntityToEnum(TEntity from)
		{
			return (TEnum)Enum.Parse(typeof(TEnum), from.Name, true);
		}

		public TEnum FromEntityId(int id)
		{
			return (TEnum)Enum.ToObject(typeof(TEnum), id);
		}

		public TEntity EnumToEntity(TEnum from)
		{
			return new TEntity { Id= from.ToInt32(System.Globalization.CultureInfo.InvariantCulture) , Name=from.ToString()  };
		}
	}
}
