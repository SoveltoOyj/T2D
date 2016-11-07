﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using T2D.Helpers;

namespace T2D.Entities
{
	public class Role:IEnumEntity
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public override string ToString()
		{
			return this.ToJson();
		}

	}

	public enum RoleEnum
	{
		Omnipotent = 1,
		Anonymous,
        Owner,
        Administrator,
        User,
        Maintenance,
        Logistics,
        Manufacturer,
        Gatekeeper1,
        Gatekeeper2,
        Alias,
        IoTBot,
        ArchetypeMember,
        CurrentVersion,
    };
}
