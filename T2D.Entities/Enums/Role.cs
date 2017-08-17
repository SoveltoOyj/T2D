using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using T2D.Helpers;

namespace T2D.Entities
{
	public class Role : IEnumEntity
	{
		public int Id { get; set; }
		[MaxLength(256)]
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
		Alias,
		ArchetypeMember,
		CurrentVersion,
		Owner,
		Administrator,
		Member,
		User,
		Maintenance,
		Logistics,
		Viewer,
		Manufacturer,
		Gatekeeper1,
		Gatekeeper2,
	};
}
