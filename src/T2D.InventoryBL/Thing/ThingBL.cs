using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using T2D.Entities;
using T2D.Infra;
using T2D.InventoryBL.Metadata;
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
			BaseThing omnipotent = _dbc.FindThing<BaseThing>(omnipotent_ThingID);
			if (omnipotent == null)
			{
				errorMsg = $"Can't find omnipotent thing '{omnipotent_ThingID}'";
				return false;
			}

			//Check that new thingID is unique 
			if (_dbc.FindThing<BaseThing>(new_thingID) != null)
			{
				errorMsg = $"new ThingID'{new_thingID}' is not unique.";
				return false;
			}

			IInventoryThing newThing;
			switch (thingType)
			{
				case ThingType.RegularThing:
					newThing = new RegularThing
					{
						IsGpsPublic = false,
						IsLocalOnly = true,
						Logging = false,
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



		public ThingRoleMember AddRoleMember(Guid toId, Guid fromId, int roleId)
		{
			//todo: security Check

			//create new ThingRole if not exists
			var thingRole = RetrieveThingRole(roleId, toId);

			// add ThingRoleMember
			var thingRoleMember = new ThingRoleMember
			{
				ThingId = fromId,
				ThingRole = thingRole,
			};
			_dbc.ThingRoleMembers.Add(thingRoleMember);
			_dbc.SaveChanges();
			return thingRoleMember;
		}

		public bool SetRoleAccessRights(out string errMsg, int roleId, int attributeId, string thingId, string[] roleAccessRights)
		{
			//todo: security Check
			errMsg = "";
			BaseThing thing = _dbc.FindThing<BaseThing>(thingId);
			if (thing == null)
			{
				errMsg = $"ThingId {thingId} do not exists.";
				return false;
			}

			RightFlag accessRight = 0;
			foreach (var item in roleAccessRights)
			{
				RightFlag value;
				if (!Enum.TryParse<RightFlag>(item, out value))
				{
					errMsg = $"RoleAccessRight {item} is not correct.";
					return false;
				}
				accessRight |= value;
			}

			//create a new ThingRole if it does not exists
			ThingRole tr = RetrieveThingRole(roleId, thing.Id);

			//create a new ThingAttribute if it does not exists
			ThingAttribute ta =
				_dbc.ThingAttributes
				.Include(tatt=>tatt.ThingAttributeRoleRights)
				.SingleOrDefault(tatt => tatt.AttributeId == attributeId && tatt.ThingId == thing.Id)
				;
			if (ta == null)
			{
				ta = new ThingAttribute
				{
					AttributeId=attributeId,
					ThingId=thing.Id,
					Logging=false,
				};
				_dbc.ThingAttributes.Add(ta);
			}

			//create a new ThingAttributeRoleRight if it does not exists
			ThingAttributeRoleRight tarl = ta.ThingAttributeRoleRights.SingleOrDefault(tattrl => tattrl.ThingRoleId == tr.Id);
			if (tarl == null)
			{
				tarl = new ThingAttributeRoleRight
				{
					ThingAttributeId=ta.Id,
					ThingRoleId = tr.Id,
					Rights = accessRight,
				};
				_dbc.ThingAttributeRoleRights.Add(tarl);
			}
			else
			{
				tarl.Rights = accessRight;
			}

			_dbc.SaveChanges();
			return true;
		}

		private ThingRole RetrieveThingRole(int roleId, Guid thingId) {
			//create new ThingRole if not exists
			var thingRole = _dbc.ThingRoles.SingleOrDefault(tr => tr.ThingId == thingId && tr.RoleId == roleId);
			if (thingRole == null)
			{
				thingRole = new ThingRole
				{
					Logging = false,
					RoleId = roleId,
					ThingId = thingId,
				};
				_dbc.ThingRoles.Add(thingRole);
				_dbc.SaveChanges();
			}
			return thingRole;

		}
	}
}
