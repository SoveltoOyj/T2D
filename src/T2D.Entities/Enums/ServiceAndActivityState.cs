using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using T2D.Helpers;

namespace T2D.Entities
{
	/// <summary>
	/// Service ja Action States
	/// ToDo: We'll propably use enum directly
	/// </summary>
	public class ServiceAndActivityState : IEnumEntity
	{
		public int Id { get; set; }
		[MaxLength(256)]
		public string Name { get; set; }
		public override string ToString()
		{
			return this.ToJson();
		}

	}


	public enum ServiceAndActitivityStateEnum
	{
		NotStarted = 1,
		Started,
		Done,
		Failed,
		NotDoneInTime,
	}
}
