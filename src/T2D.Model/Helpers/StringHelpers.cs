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

	
	

}
