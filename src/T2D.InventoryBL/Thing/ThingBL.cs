using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using T2D.Entities;
using T2D.Infra;
using T2D.InventoryBL.Metadata;
using T2D.Model.Enums;
using T2D.Model.Helpers;
using T2D.Model.InventoryApi;
using System.Reflection;
using Newtonsoft.Json;

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
				errorMsg = $"Can't find creator thing '{omnipotent_ThingID}'";
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

		public GetRelationsResponse GetRelations(out string errMsg, int roleId, string thingId)
		{
			errMsg = null;

			BaseThing thing =
			_dbc.ThingQuery(thingId)
				.Include(t => t.ThingAttributes)
				.Include(t => t.ToThingRelations)
				.FirstOrDefault()
				;

			if (thing == null)
			{
				errMsg = $"Thing '{thingId}' do not exists.";
				return null;
			}

			_session.AddSessionAccess(roleId, thing.Id);

			var ret = new GetRelationsResponse();
			ret.RelationThings = new List<GetRelationsResponse.RelationsThings>();
			var enumBL = new EnumBL();

			foreach (var group in thing.ToThingRelations.GroupBy(tr => tr.RelationId))
			{
				var rt = new GetRelationsResponse.RelationsThings
				{
					Relation = enumBL.EnumNameFromInt<RelationEnum>(group.Key),
					Things = new List<GetRelationsResponse.RelationsThings.IdTitle>(),
				};
				foreach (var th in group)
				{
					var thing2 = _dbc.FindThing<BaseThing>(th.ToThingId);
					var thingIdTitle = new GetRelationsResponse.RelationsThings.IdTitle
					{
						ThingId = ThingIdHelper.Create(thing2.Fqdn, thing2.US)
					};
					if (thing2 != null && thing2 is IInventoryThing)
						thingIdTitle.Title = ((IInventoryThing)thing2).Title;
					rt.Things.Add(thingIdTitle);
				}
				ret.RelationThings.Add(rt);
			}

			return ret;

		}

		public AttributeValue GetAttribute(string attributeName, int roleId, string thingId)
		{
			BaseThing thing =
				_dbc.ThingQuery(thingId)
					.Include(t => t.ThingAttributes)
					.FirstOrDefault()
					;

			var ret = new AttributeValue
			{
				Attribute = attributeName,
			};

			if (thing == null)
			{
				ret.IsOk=false;
				ret.ErrorDescription =  $"Thing '{thingId}' do not exists.";
				return ret;
			}
			//is it an Extension.
			if (ThingIdHelper.IsValidThingId(attributeName))
			{
				GetExtensionValue(thing, attributeName, ret);
				return ret;
			}

			// not an extension,


			var enumBL = new EnumBL();
			int? attributeId = enumBL.EnumIdFromApiString<AttributeEnum>(attributeName);
			if (attributeId==null)
			{
				ret.IsOk = false;
				ret.ErrorDescription = $"Attribute do not exists.";
				return ret;
			}

			ret.Value = GetAttributeValue(thing, attributeName, (AttributeEnum)attributeId.Value);
			ret.IsOk = true;
			return ret;

		}

		public AttributeValue SetAttribute(string attributeName, int roleId, string thingId, object value)
		{
			BaseThing thing =
				_dbc.ThingQuery(thingId)
					.Include(t => t.ThingAttributes)
					.FirstOrDefault()
					;

			var ret = new AttributeValue
			{
				Attribute = attributeName,
			};
			if (thing == null)
			{
				ret.IsOk = false;
				ret.ErrorDescription = $"Thing '{thingId}' do not exists.";
				return ret;
			}

			// if attribute is Extension
			if (ThingIdHelper.IsValidThingId(attributeName))
			{
				SetExtensionValue(thing, attributeName, (string)value);
				ret.IsOk = true;
				ret.Value = value;
				return ret;
			}

			// not an extension,
			var enumBL = new EnumBL();
			int? attributeId = enumBL.EnumIdFromApiString<AttributeEnum>(attributeName);
			if (attributeId == null)
			{
				ret.IsOk = false;
				ret.ErrorDescription = $"Attribute do not exists.";
				return ret;
			}

			ret.ErrorDescription = SetAttributeValue(thing, attributeName, value, (AttributeEnum)attributeId.Value);
			ret.IsOk = (ret.ErrorDescription == null);
			ret.Value = GetAttributeValue(thing, attributeName, (AttributeEnum)attributeId.Value);
			return ret;
		}

		public QueryMyRolesResponse QueryMyRoles(out string errMsg, string thingId)
		{
			errMsg = null;

			BaseThing thing =
			_dbc.ThingQuery(thingId)
				.Include(t => t.ThingRoles)
				.FirstOrDefault()
				;

			if (thing == null)
			{
				errMsg = $"Thing '{thingId}' do not exists.";
				return null;
			}

			//TODO: check that session has right to 

			// explicit loading - select those thingRoleMembers, where ThingRoleMember is one of things in session
			// have to use Lists
			List<Guid> sessionThings = new List<Guid>();
			sessionThings.Add(_session.Session.EntryPoint_ThingId);

			foreach (var item in _session.Session.SessionAccesses)
			{
				if (!sessionThings.Contains(item.ThingId))
					sessionThings.Add(item.ThingId);
			}
			List<Guid> thingRoles = new List<Guid>();
			thingRoles.AddRange(thing.ThingRoles.Select(tr => tr.Id));

			var thingRoleMembers =
				_dbc.ThingRoleMembers
					.Where(trm => sessionThings.Contains(trm.ThingId) && thingRoles.Contains(trm.ThingRoleId))
					.ToList()
					;

			var ret = new QueryMyRolesResponse
			{
				Roles = new List<string>(),
			};
			var roleIds = new List<int>();

			// add roles and add to SessionAccess
			foreach (var item in thingRoleMembers)
			{
				if (item.ThingRole != null)
				{
					int roleId = item.ThingRole.RoleId;
					if (!roleIds.Contains(roleId)) roleIds.Add(roleId);

					_session.AddSessionAccess(roleId, thing.Id);
				}
			}
			ret.Roles.AddRange(
				_dbc.Roles
					.Where(r => roleIds.Contains(r.Id))
					.Select(r => r.Name)
					.ToList()
				);

			return ret;
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

		public bool SetRoleAccessRights(out string errMsg, int executingRole, int roleForAccessRightsId, int attributeId, string thingId, IEnumerable<string> roleAccessRights)
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
			ThingRole tr = RetrieveThingRole(roleForAccessRightsId, thing.Id);

			//create a new ThingAttribute if it does not exists
			ThingAttribute ta =
				_dbc.ThingAttributes
				.Include(tatt => tatt.ThingAttributeRoleRights)
				.SingleOrDefault(tatt => tatt.AttributeId == attributeId && tatt.ThingId == thing.Id)
				;
			if (ta == null)
			{
				ta = new ThingAttribute
				{
					AttributeId = attributeId,
					ThingId = thing.Id,
					Logging = false,
				};
				_dbc.ThingAttributes.Add(ta);
			}

			//create a new ThingAttributeRoleRight if it does not exists
			ThingAttributeRoleRight tarl = ta.ThingAttributeRoleRights.SingleOrDefault(tattrl => tattrl.ThingRoleId == tr.Id);
			if (tarl == null)
			{
				tarl = new ThingAttributeRoleRight
				{
					ThingAttributeId = ta.Id,
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


		public List<AttributeRoleRight> GetRoleAccessRights(out string errMsg, int roleId, int roleForRightId, string thingId)
		{
			//todo: security Check
			errMsg = "";

			//get all ThingRoles
			BaseThing thing =
				_dbc.ThingQuery(thingId)
				.Include(t => t.ThingRoles)
				.SingleOrDefault()
				;
			if (thing == null)
			{
				errMsg = $"ThingId {thingId} do not exists.";
				return null;
			}

			var ret = new List<AttributeRoleRight>();

			//get thingAttributeRoleRights for asked role
			var thingRole =
				thing.ThingRoles
				.Where(tr => tr.RoleId == roleForRightId)
				.SingleOrDefault()
				;
			if (thingRole == null)
			{
				return ret;
			}

			var thingAttribureRoleRights =
				_dbc.ThingAttributeRoleRights
				.Include(tarr => tarr.ThingAttribute)
					.ThenInclude(ta => ta.Attribute)
				.Where(tarr => tarr.ThingRoleId == thingRole.Id)
				;

			foreach (var item in thingAttribureRoleRights)
			{
				// Todo: .ThenInclude seems to not working, that's we are using Enum directly.
				// Explicit loading do not work either. Some problem in navigation property?
				//var reference = _dbc.Entry(item.ThingAttribute).Reference(ta => ta.Attribute);
				//if (!reference.IsLoaded) reference.Load();
				string attrName;
				EnumBL enumBL = new EnumBL();
				if (item.ThingAttribute.Attribute != null) attrName = item.ThingAttribute.Attribute.Title;
				else attrName = enumBL.EnumNameFromInt<AttributeEnum>(item.ThingAttribute.AttributeId);

				AttributeRoleRight newRoleRight = new AttributeRoleRight
				{
					Attribute = attrName,
					RoleAccessRights = new List<string>(),
				};
				string rights = item.Rights.ToString();
				foreach (string right in rights.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
				{
					newRoleRight.RoleAccessRights.Add(right);
				}
				ret.Add(newRoleRight);
			}
			return ret;
		}

		private ThingRole RetrieveThingRole(int roleId, Guid thingId)
		{
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


		private object GetAttributeValue(IThing thing, string attributeName, AttributeEnum attEnum)
		{
			BindingFlags bf = BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance;
			var typeInfo = thing.GetType().GetTypeInfo();
			if (typeInfo.GetProperties(bf).Any(p=>p.Name==attributeName))
			{
				var ret =  typeInfo.GetProperty(attributeName, bf).GetValue(thing);
				if (ret == null) return ret;
				switch (attEnum)
				{
					case AttributeEnum.Location_Gps:
					case AttributeEnum.PreferredLocation_Gps:
						string[] str = ret.ToString().Split(new char[] { '(', ' ', ')' }, StringSplitOptions.RemoveEmptyEntries);
						if (str.Length != 3)
						{
							return null;
						}
						GpsLocation location = new GpsLocation
						{
							Longitude = decimal.Parse(str[1], System.Globalization.CultureInfo.InvariantCulture),
							Latitude = decimal.Parse(str[2], System.Globalization.CultureInfo.InvariantCulture)
						};
						return location;
				}
				return ret;

			}
			return null;
		}

		private string SetAttributeValue(IThing thing, string attributeName, object value, AttributeEnum attEnum)
		{
			try
			{
				BindingFlags bf = BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance;
				var typeInfo = thing.GetType().GetTypeInfo();
				if (typeInfo.GetProperties(bf).Any(p => p.Name == attributeName))
				{

					switch (attEnum)
					{
						case AttributeEnum.Location_Gps:
						case AttributeEnum.PreferredLocation_Gps:
							GpsLocation location =JsonConvert.DeserializeObject<GpsLocation>(value.ToString());
							value = $"Point({location.Longitude.ToString(System.Globalization.CultureInfo.InvariantCulture)} {location.Latitude.ToString(System.Globalization.CultureInfo.InvariantCulture)})";
							break;
						default:
							break;
					}

					var propertyInfo = typeInfo.GetProperty(attributeName, bf);
					object changedType = propertyInfo.PropertyType == typeof(DateTime) || propertyInfo.PropertyType == typeof(DateTime?)
					? DateTime.Parse(value.ToString())
					: Convert.ChangeType(value, propertyInfo.PropertyType);

					propertyInfo.SetValue(thing, changedType, null);
					return null;
				}
				return $"Attribute {attributeName} do not exists";
			}
			catch (Exception ex)
			{
				return $"Attribute {attributeName} value could not be set, error: {ex.Message}";
			}
		}

		private void SetExtensionValue(IThing thing, string extensionName, string value)
		{
			string fqdn = ThingIdHelper.GetFQDN(extensionName);
			string us = ThingIdHelper.GetUniqueString(extensionName);
			var extension = _dbc.Extensions.SingleOrDefault(et => et.Fqdn == fqdn && et.US == us);
			//is it a new Extensiontype
			if (extension == null)
			{
				extension = new Extension
				{
					Fqdn = fqdn,
					US = us,
				};
				_dbc.Extensions.Add(extension);
				_dbc.SaveChanges();
			}

			var data = _dbc.ExtensionDatas.SingleOrDefault(ed => ed.ThingId == thing.Id && ed.ExtensionId == extension.Id);
			if (data == null)
			{
				data = new ExtensionData
				{
					ThingId = thing.Id,
					ExtensionId = extension.Id,
				};
				_dbc.ExtensionDatas.Add(data);
			}
			data.Data = value;
			_dbc.SaveChanges();
			return;
		}

		private void GetExtensionValue(IThing thing, string extensionName, AttributeValue attributeValue)
		{
			string fqdn = ThingIdHelper.GetFQDN(extensionName);
			string us = ThingIdHelper.GetUniqueString(extensionName);
			var extension = _dbc.Extensions.SingleOrDefault(et => et.Fqdn == fqdn && et.US == us);
			if (extension == null)
			{
				attributeValue.IsOk = false;
				attributeValue.ErrorDescription = "No such extension type.";
				return;
			}

			var data = _dbc.ExtensionDatas.SingleOrDefault(ed => ed.ThingId == thing.Id && ed.ExtensionId == extension.Id);
			if (data == null)
			{
				attributeValue.IsOk = false;
				attributeValue.ErrorDescription = "No value.";
				return;
			}
			attributeValue.IsOk = true;
			attributeValue.Value =  data.Data ;
			return;
		}
		public bool SetRoleMemberList(out string errMsg, int roleId, int roleToSetId, string thingId, List<string> memberThingIds)
		{
			//todo: security Check
			errMsg = "";

			//Check that Thing is available
			BaseThing thing = _dbc.FindThing<BaseThing>(thingId);
			if (thing == null)
			{
				errMsg = $"Can't find thing '{thingId}'";
				return false;
			}

			//create new ThingRole if not exists
			var thingRole = RetrieveThingRole(roleToSetId, thing.Id);

			// remove existing ThingRoleMembers
			_dbc.ThingRoleMembers.RemoveRange(_dbc.ThingRoleMembers.Where(trm => trm.ThingRoleId == thingRole.Id));
			_dbc.SaveChanges();

			foreach (var item in memberThingIds)
			{
				//Check that Thing is available
				BaseThing memberThing = _dbc.FindThing<BaseThing>(item);
				if (memberThing == null)
				{
					errMsg = $"Can't find memberThing '{item}', continueing to add members. ";
					return false;
				}
				// add ThingRoleMembers
				var thingRoleMember = new ThingRoleMember
				{
					ThingId = memberThing.Id,
					ThingRoleId = thingRole.Id,
				};
				_dbc.ThingRoleMembers.Add(thingRoleMember);
			}
			_dbc.SaveChanges();
			return true;

		}

		public List<string> GetRoleMemberList(out string errMsg, int roleId, int roleForMemberListId, string thingId)
		{
			errMsg = null;
			//Check that Thing is available
			BaseThing thing = _dbc.FindThing<BaseThing>(thingId);
			if (thing == null)
			{
				errMsg = $"Can't find thing '{thingId}'";
				return null;
			}
			var ret = new List<string>();
			//create new ThingRole if not exists
			var thingRole = RetrieveThingRole(roleForMemberListId, thing.Id);

			var q =
			_dbc.ThingRoleMembers
				.Include(trm => trm.Thing)
				.Where(trm => trm.ThingRoleId == thingRole.Id)
				.Select(trm => new { Fqdn = trm.Thing.Fqdn, US = trm.Thing.US })
				;

			foreach (var item in q)
			{
				ret.Add(ThingIdHelper.Create(item.Fqdn, item.US));
			}
			return ret;

		}
	}
}
