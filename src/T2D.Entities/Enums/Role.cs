using System;
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
	 Owner,
	 Administrator,
	 GroupMember,
	 Holder,
	 ContractingParty,
	 Lessee,
	 User,
	 Maintenance,
	 Transport,
	 Storage,
	 Finder,
	 Purchaser,
	 Inspector,
	 Manufacturer,
	 Customs,
	 Bank,
	 Belongings,
	 Passanger,
	 Anonymous,
	 Alias,
	 IoTBot,
	 ArchetypeMember, 
	 CurrentVersion,
	};
}
