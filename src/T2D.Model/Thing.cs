﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T2D.Helpers;

namespace T2D.Model
{
	public class Thing:IThingModel
	{
		public ThingId Id { get; set; }

		public float Height { get; set; }
		public float Width { get; set; }

		public override string ToString()
		{
			return this.ToJson();
		}
	}
}
