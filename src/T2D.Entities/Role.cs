using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace T2D.Entities
{
	public class Role
	{
		public int Id { get; set; }
		public string Name { get; set; }

	}

	public enum RoleEnum
	{
		Omnipotent = 1,
		Anonymous,
		Owner,
		Administrator,
		GroupMember,
		Holder,
		ContractingParty,
	};
}
