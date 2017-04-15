using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using T2D.Entities;
using T2D.Model;

namespace T2D.InventoryBL.Metadata
{
	public class EnumBL
	{

		public List<string> ApiEnumNames()
		{

			return new List<string>
			{
				"AttributeType",
				"FunctionalStatus",
				"LocationType",
				"Relation",
				"Right",
				"Role",
				"ServiceAndActivityState",
				"ThingStatus",
				"Attribute",
				"AuthenticationType",
				"ThingType",
			};
		}

		public List<ModelEnum> ApiEnumValuesFromEnum<TEnum>()
			where TEnum : struct
		{
			if (!typeof(TEnum).GetTypeInfo().IsEnum)
			{
				throw new ArgumentException("TEnum must be of type System.Enum");
			}
			var ret = new List<ModelEnum>();
			foreach (var item in Enum.GetValues(typeof(TEnum)))
			{
				ret.Add(new ModelEnum { Id = ((IConvertible)item).ToInt32(System.Globalization.CultureInfo.InvariantCulture), Name = item.ToString() });
			}
			return ret;
		}

		public object ApiEnumValuesFromEnumEntity<TEnumEntity>(IQueryable<TEnumEntity> query)
			where TEnumEntity:IEnumEntity
		{
			var ret = new List<ModelEnum>();
			foreach (var item in query)
			{
				ret.Add(new ModelEnum { Id = item.Id, Name = item.Name });
			}
			return ret;
		}
	}
}
