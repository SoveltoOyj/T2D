using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using T2D.Entities;

namespace T2D.InventoryBL
{
	public static class AttributeSecurity
	{
		public static bool QueryMyRolesRight(BaseThing thing, Session session)
		{
			return true;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="thing"></param>
		/// <param name="session"></param>
		/// <param name="role">not necessary in dbContect!</param>
		/// <returns></returns>
		public static bool QueryRelationsRight(BaseThing thing, Session session, Role role)
		{
			return true;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="thing"></param>
		/// <param name="session"></param>
		/// <param name="role">not necessary in dbContext!</param>
		/// <param name="attribute">not necessary in dbContext</param>
		/// <returns></returns>
		public static bool QueryAttributeRight(BaseThing thing, Session session, Role role, T2D.Entities.Attribute attribute)
		{
			return true;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="thing"></param>
		/// <param name="session"></param>
		/// <param name="role">not necessary in dbContext!</param>
		/// <returns></returns>
		public static bool QueryServiceRequestRight(BaseThing thing, Session session, Role role)
		{
			return true;
		}

	}
}
