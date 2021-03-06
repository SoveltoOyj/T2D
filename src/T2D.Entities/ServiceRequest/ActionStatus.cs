﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace T2D.Entities
{
	/// <summary>
	/// ActionDefinition will be copied here to enable that Service Type can change during Service Request.
	/// </summary>
	public class ActionStatus:IEntity
	{
		public Guid Id { get; set; }

		public Guid ServiceStatusId { get; set; }
		public ServiceStatus ServiceStatus { get; set; }

		//if null, this is an Alarm or Failed
		public Guid? ActionDefinitionId { get; set; }
		public ActionDefinition ActionDefinition { get; set; }

		public ActionListType ActionType { get; set; }

		public string Description { get; set; }

		public DateTime AddedAt { get; set; }
		public DateTime? CompletedAt { get; set; }

		public ServiceAndActitivityState State { get; set; }

	}


}