﻿using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using T2D.Helpers;

namespace T2D.Entities
{
	public class Weatherproofness : IEnumEntity
	{
		public int Id { get; set; }
		[MaxLength(256)]
		public string Name { get; set; }
		public override string ToString()
		{
			return this.ToJson();
		}


	}

	public enum WeatherproofnessEnum
	{
		No = 1,
		moderate,
		good,
		excellent
	};
}
