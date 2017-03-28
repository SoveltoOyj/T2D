using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using T2D.Helpers;

namespace T2D.Entities
{
	/// <summary>
	/// Service ja Action States
	/// </summary>

	public class State : IEnumEntity
	{
		public int Id { get; set; }
		[MaxLength(256)]
		public string Name { get; set; }
		public override string ToString()
		{
			return this.ToJson();
		}

	}


	public enum StateEnum
	{
		NotStarted = 1,
		Started,
		Done,
		Failed,
		NotDoneInTime,
	}
}
