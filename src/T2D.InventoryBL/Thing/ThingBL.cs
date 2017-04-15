using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using T2D.Entities;
using T2D.Infra;
using T2D.Model.Enums;
using T2D.Model.Helpers;

namespace T2D.InventoryBL.Thing
{
	public class ThingBL
	{
		private readonly SessionBL _session;
		private readonly EfContext _dbc;

		public static ThingBL CreateThingBL(EfContext dbc, SessionBL session)
		{
			if (session == null) return null;
			ThingBL ret = new ThingBL(dbc, session);

			return ret;
		}
		private ThingBL(EfContext dbc, SessionBL session)
		{
			_dbc = dbc;
			_session = session;
		}

		public bool CreateNewThing(out string errorMsg, string new_thingID, string title, ThingType thingType, string omnipotent_ThingID)
		{
			errorMsg = "";

			//Check that omnipotent_Thing is available
			BaseThing omnipotent = Find<BaseThing>(omnipotent_ThingID);
			if (omnipotent == null)
			{
				errorMsg = $"Can't find omnipotent thing '{omnipotent_ThingID}'";
				return false;
			}

			//Check that new thingID is unique 
			if (Find<BaseThing>(new_thingID) != null)
			{
				errorMsg = $"new ThingID'{new_thingID}' is not unique.";
				return false;
			}

			IInventoryThing newThing;
			switch (thingType)
			{
				case ThingType.RegularThing:
					newThing = new RegularThing {
						IsGpsPublic=false,
						IsLocalOnly=true,
						Logging=false,
					};
					break;
				case ThingType.ArchetypeThing:
					newThing = new ArchetypeThing
					{
					};
					break;
				case ThingType.AuthenticationThing:
					newThing = new AuthenticationThing
					{
					};
					break;
				case ThingType.AliasThing:
				case ThingType.IoTThing:
				case ThingType.WalletThing:
				default:
					errorMsg = "Not yet implemented ThingType:{thingType}";
					return false;
			}
			newThing.Title = title;
			newThing.Fqdn = ThingIdHelper.GetFQDN(new_thingID);
			newThing.US = ThingIdHelper.GetUniqueString(new_thingID);
			_dbc.Things.Add(newThing as BaseThing);
			_dbc.SaveChanges();

			// set ominipotent role
			if (AddRoleMember(newThing.Id, omnipotent.Id, (int)RoleEnum.Omnipotent) == null)
			{
				errorMsg = "Can't create omnipotent role to the thing 'omnipotent_ThingID'. New Thing was created.";
				return false;
			}

			// add to session
			_session.AddSessionAccess((int)RoleEnum.Omnipotent, newThing.Id);

			return true;
		}


		public T Find<T>(string thingId)
			where T : class, T2D.Entities.IThing
		{
			return _dbc.Things.SingleOrDefault(t=>t.Fqdn == ThingIdHelper.GetFQDN(thingId) && t.US== ThingIdHelper.GetUniqueString(thingId)) as T;
		}

		public ThingRoleMember AddRoleMember(Guid toId, Guid fromId, int roleId)
		{
			//todo: security Check

			//create new ThingRole if not exists
			var thingRole= _dbc.ThingRoles.SingleOrDefault(tr => tr.ThingId == toId && tr.RoleId == roleId);
			if (thingRole==null)
			{
				thingRole = new ThingRole {
					Logging = false,
					RoleId = roleId,
					ThingId = toId,
				};
				_dbc.ThingRoles.Add(thingRole);
			}

			// add ThingRoleMember
			var thingRoleMember = new ThingRoleMember {
				ThingId = fromId,
				ThingRole = thingRole,
			};
			_dbc.ThingRoleMembers.Add(thingRoleMember);
			_dbc.SaveChanges();
			return thingRoleMember;
		}


	}
}
