using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using T2D.Model.Helpers;

namespace T2D.Model
{
	public class ThingIdAttribute:ValidationAttribute
	{
		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			string thingId = (string)value;
			if (string.IsNullOrWhiteSpace(thingId))
			{
				return new ValidationResult("ThingId is null or empty.");
			}
			try
			{
				ThingIdHelper.GetFQDN(thingId);
				ThingIdHelper.GetUniqueString(thingId);
			}
			catch (Exception ex)
			{
				return new ValidationResult($"ThingID '{thingId}' is not valid: {ex.Message}");
			}
			return ValidationResult.Success;
		}
	}
}
