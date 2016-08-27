using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace T2D.Helpers
{
	public static class StringHelpers
	{
		public static string ToJson(this object obj)
		{
			var json = new DataContractJsonSerializer(obj.GetType());
			using (var ms = new MemoryStream())
			{
				json.WriteObject(ms, obj);
				ms.Position = 0;
				return Encoding.UTF8.GetString(ms.ToArray());
			};
		}

		public static string TestEquality(this string aStr, params string[] bStrs)
		{
			if (string.IsNullOrWhiteSpace(aStr)) return  bStrs.Any(b=>string.IsNullOrWhiteSpace(b))?"":null;
			return bStrs.Any(b=>string.Compare(aStr, b, true) == 0)?aStr:null;
		}
	}

	public static class QueryableExtensions
	{
		public static IQueryable<T> OrderByStr<T>(this IQueryable<T> source, string sortModels)
		{
			var expression = source.Expression;
			int count = 0;
			foreach (var item in sortModels.Split(new char[]{','}, StringSplitOptions.RemoveEmptyEntries))
			{
				string paramName = "x" + count.ToString();
				var parameter = Expression.Parameter(typeof(T), paramName);
				var selector = Expression.PropertyOrField(parameter, item);
//				var method = string.Equals(item.Sort, "desc", StringComparison.OrdinalIgnoreCase) ?
				var method = string.Equals("desc", "desc", StringComparison.OrdinalIgnoreCase) ?
						(count == 0 ? "OrderByDescending" : "ThenByDescending") :
						(count == 0 ? "OrderBy" : "ThenBy");
				expression = Expression.Call(typeof(Queryable), method,
						new Type[] { source.ElementType, selector.Type },
						expression, Expression.Quote(Expression.Lambda(selector, parameter)));
				count++;
			}
			return count > 0 ? source.Provider.CreateQuery<T>(expression) : source;
		}
	}

}
