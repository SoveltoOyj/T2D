using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace T2D.Model.Helpers
{
	public static class ThingIdHelper
	{

		// http://stackoverflow.com/questions/11809631/fully-qualified-domain-name-validation
		public static bool CheckFQHN(string fqhn)
		{
			if (string.IsNullOrWhiteSpace(fqhn))
				return false;

			var match = Regex.Match(fqhn, @"(?=^.{4,253}$)(^((?!-)[a-zA-Z0-9-]{0,62}[a-zA-Z0-9]\.)+[a-zA-Z]{2,63}$)", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(5));
			return match.Success;
		}
		public static bool CheckUniqueString(string uniqueString)
		{
			if (string.IsNullOrWhiteSpace(uniqueString))
				return false;

			return uniqueString.Length < 1024;
		}

		public static string Create(string creatorFQHN, string uniqueString)
		{
			if (!ThingIdHelper.CheckFQHN(creatorFQHN))
				throw new ArgumentException("Argument is not FQHN.", "creatorFQHN");

			if (!ThingIdHelper.CheckUniqueString(uniqueString))
				throw new ArgumentException("UniqueString is invalid.", "uniqueString");

			return $"{creatorFQHN}/{uniqueString}";
		}

		public static string GetFQHN(string thingId)
		{
			if (string.IsNullOrWhiteSpace(thingId))
				throw new ArgumentException("Argument is null or empty.", "thingId");

			var index = thingId.IndexOf('/');
			if (index < 1)
				throw new ArgumentException("Argument does not contain '/'.", "thingId");

			string fqhn = thingId.Substring(0, index);
			if (!ThingIdHelper.CheckFQHN(fqhn))
				throw new ArgumentException("Argument does not contain FQHN.", "thingId");

			return fqhn;
		}

		public static string GetUniqueString(string thingId)
		{
			if (string.IsNullOrWhiteSpace(thingId))
				throw new ArgumentException("Argument is null or empty.", "thingId");

			var index = thingId.IndexOf('/');
			if (index < 1)
				throw new ArgumentException("Argument does not contain '/'.", "thingId");

			if (thingId.Length < (index + 1))
				throw new ArgumentException("Argument does not contain uniqueString.", "thingId");

			string uniqueString = thingId.Substring(index+1);
			if (!ThingIdHelper.CheckUniqueString(uniqueString))
				throw new ArgumentException("Argument does not contain uniqueString.", "thingId");

			return uniqueString;
		}

	}
}

