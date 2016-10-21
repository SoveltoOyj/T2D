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
		public static bool CheckFQDN(string fqdn, bool allowNull= false)
		{
			if (string.IsNullOrWhiteSpace(fqdn))
				return allowNull;

			var match = Regex.Match(fqdn, @"(?=^.{4,253}$)(^((?!-)[a-zA-Z0-9-]{0,62}[a-zA-Z0-9]\.)+[a-zA-Z]{2,63}$)", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(5));
			return match.Success;
		}
		public static bool CheckUniqueString(string uniqueString, bool allowNull = false)
		{
			if (string.IsNullOrWhiteSpace(uniqueString))
				return allowNull;

			return uniqueString.Length < 1024;	//ToDo: configuration, Entity will use it also
		}

		public static string Create(string creatorFQDN, string uniqueString, bool allowNull=false)
		{
			if (!ThingIdHelper.CheckFQDN(creatorFQDN, allowNull))
				throw new ArgumentException("Argument is not FQDN.", "creatorFQDN");

			if (!ThingIdHelper.CheckUniqueString(uniqueString, allowNull))
				throw new ArgumentException("UniqueString is invalid.", "uniqueString");

			return $"{creatorFQDN}/{uniqueString}";
		}

		public static string GetFQDN(string thingId)
		{
			if (string.IsNullOrWhiteSpace(thingId))
				throw new ArgumentException("Argument is null or empty.", "thingId");

			var index = thingId.IndexOf('/');
			if (index < 1)
				throw new ArgumentException("Argument does not contain '/'.", "thingId");

			string fqdn = thingId.Substring(0, index);
			if (!ThingIdHelper.CheckFQDN(fqdn))
				throw new ArgumentException("Argument does not contain FQDN.", "thingId");

			return fqdn;
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

