using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using T2D.Helpers;

namespace T2D.Entities
{

	/// <summary>
	/// Local Sessions.
	/// ToDo: External Sessions.
	/// </summary>
	public class Session : IEntity
	{
		public Guid Id { get; set; }

		/// <summary>
		/// Secret token, JWT?
		/// </summary>
		public string Token { get; set; }

		public Guid EntryPoint_ThingId { get; set; }

		public DateTime StartTime { get; set; }
		public DateTime? EndTime { get; set; }


		#region Navigation Properties
		public AuthenticationThing EntryPoint { get; set; }
		public List<SessionAccess> SessionAccesses { get; set; }
		public List<ThingAttributeRoleSessionAccess> ThingAttributeRoleSessionAccesses { get; set; }
		#endregion

		public Session()
		{
			SessionAccesses = new List<SessionAccess>();
			ThingAttributeRoleSessionAccesses = new List<ThingAttributeRoleSessionAccess>();
		}
	}
}
