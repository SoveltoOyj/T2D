using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using T2D.Model;

namespace T2D.InventoryBL.Mappers
{
	/// <summary>
	/// Mapper from/to C# enum type to EntityType
	/// </summary>
	/// <typeparam name="TEnum"></typeparam>
	/// <typeparam name="TEntity"></typeparam>
	[Obsolete("Todo: this will be taken away.")]
	public class EnumMapper<TEnum, TEntity> 
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

		/// <summary>
		/// note. this entity is not in dbContext!
		/// </summary>
		/// <param name="from"></param>
		/// <returns></returns>
		public TEntity EnumToEntity(TEnum from)
		{
			return new TEntity { Id= from.ToInt32(System.Globalization.CultureInfo.InvariantCulture) , Name=from.ToString()  };
		}

		/// <summary>
		/// note. this entity is not in dbContext!
		/// </summary>
		/// <param name="from">enum value name</param>
		/// <returns></returns>
		public TEntity EnumToEntity(string from)
		{
			TEnum enumItem =  (TEnum)Enum.Parse(typeof(TEnum), from, true);
			return new TEntity { Id = enumItem.ToInt32(System.Globalization.CultureInfo.InvariantCulture), Name = enumItem.ToString() };
		}

	}
}
