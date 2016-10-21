﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using T2D.Helpers;

namespace T2D.Model
{
	public class Relation : IEnumModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public override string ToString()
		{
			return this.ToJson();
		}
	}
}
