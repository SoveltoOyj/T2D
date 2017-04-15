using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using T2D.Entities;
using T2D.Helpers;
using T2D.Model;

namespace T2D.InventoryBL.Mappers
{
	/// <summary>
	/// Mapper converts enum-data from Entity to Metadata API Enum type
	/// </summary>
	/// <typeparam name="TEnumEntity"></typeparam>
	/// <typeparam name="TEnumModel"></typeparam>
	public class MetadataEnumMapper<TEnumEntity, TEnumModel> 
		where TEnumEntity : class, IEnumEntity, new()
		where TEnumModel : ModelEnum, new()
	{
		public TEnumModel EntityToModel(TEnumEntity from)
		{
			return new TEnumModel { Id = FromEntityId(from.Id), Name = from.Name } ;
		}

		public int FromEntityId(int id)
		{
			return id;
		}

		public int FromModelId(int id)
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
