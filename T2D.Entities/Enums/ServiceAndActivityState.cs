using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using T2D.Helpers;

namespace T2D.Entities
{

	public enum ServiceAndActitivityState
	{
		NotStarted = 1,
		Started,
		Done,
		Failed,
		NotDoneInTime,
	}
}
