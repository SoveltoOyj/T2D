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
				"ActionType",
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

		public List<ModelEnum> ApiEnumValuesFromEnumEntity<TEnumEntity>(IQueryable<TEnumEntity> query)
			where TEnumEntity : IEnumEntity
		{
			var ret = new List<ModelEnum>();
			foreach (var item in query)
			{
				ret.Add(new ModelEnum { Id = item.Id, Name = item.Name });
			}
			return ret;
		}

		/// <summary>
		/// Gets int value of enum string
		/// </summary>
		/// <typeparam name="TEnum">Enum type</typeparam>
		/// <param name="enumStr">Enum value as string.</param>
		/// <returns>int value or null if enum does not contain that value.</returns>
		public int? EnumIdFromApiString<TEnum>(string enumStr)
			where TEnum : struct, IConvertible
		{
			if (!typeof(TEnum).GetTypeInfo().IsEnum)
			{
				throw new ArgumentException("TEnum must be of type System.Enum");
			}

			TEnum value;
			if (!Enum.TryParse<TEnum>(enumStr, true, out value))
			{
				return null;
			}
			return ((IConvertible)value).ToInt32(System.Globalization.CultureInfo.InvariantCulture);
		}

		/// <summary>
		/// Gets enum? of enum string
		/// </summary>
		/// <typeparam name="TEnum">Enum type</typeparam>
		/// <param name="enumStr">Enum value as string.</param>
		/// <returns>enum?, null if enum does not contain that value.</returns>
		public TEnum? EnumFromApiString<TEnum>(string enumStr)
			where TEnum : struct, IConvertible
		{
			if (!typeof(TEnum).GetTypeInfo().IsEnum)
			{
				throw new ArgumentException("TEnum must be of type System.Enum");
			}

			TEnum value;
			if (!Enum.TryParse<TEnum>(enumStr, out value))
			{
				return null;
			}
			return value;
		}

		public string EnumNameFromInt<TEnum>(int intValue)
	where TEnum : struct
		{
			if (!typeof(TEnum).GetTypeInfo().IsEnum)
			{
				throw new ArgumentException("TEnum must be of type System.Enum");
			}

			return Enum.GetName(typeof(TEnum), intValue);

		}

	}
}
