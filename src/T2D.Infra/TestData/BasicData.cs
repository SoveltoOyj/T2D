using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using T2D.Entities;

namespace T2D.Infra.TestData
{
	class BasicData
	{
		private EfContext _dbc { get; set; }


		public BasicData(EfContext dbc)
		{
			this._dbc = dbc;
		}

		public void DoIt()
		{
			Console.WriteLine("\nInserting Base data");
			InsertEnums();
			InsertBaseData();
		}

		private void InsertEnums()
		{
			//Enums
			CommonTestData.AddEnumData(_dbc, _dbc.Relations, typeof(RelationEnum));
			CommonTestData.AddEnumData(_dbc, _dbc.Roles, typeof(RoleEnum));
			CommonTestData.AddEnumData(_dbc, _dbc.LocationTypes, typeof(LocationTypeEnum));
			CommonTestData.AddEnumData(_dbc, _dbc.Status, typeof(FunctionalStatusEnum));

			//attributes
			AddAttributeData(_dbc, typeof(AttributeEnum));

		}
		private void InsertBaseData()
		{
			//Inventories
			var i1 = new Inventory
			{
				Id = CommonTestData.Next(),
				Fqdn = CommonTestData.Fqdn,
				Title = "Sovelto"
			};
			_dbc.Inventories.Add(i1);
			_dbc.SaveChanges();
			CommonTestData.Entities["I1"] = i1;

			//Archetypethings
			_dbc.ArchetypeThings.Add(new ArchetypeThing { Id = CommonTestData.Next(), Fqdn = CommonTestData.Fqdn, US = "ArcNb1", Title = "Archetype example", Modified = new DateTime(2016, 3, 23), Published = new DateTime(2016, 4, 13), Created = new DateTime(2014, 3, 23) });

			//AuthenticationThings
			//Anonymous user
			_dbc.AuthenticationThings.Add(
			new AuthenticationThing
			{
				Id = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1),
				Fqdn = CommonTestData.Fqdn,
				US = "AnonymousUser",
				Title = "Anonymous User",
			});
			var M100 = new AuthenticationThing { Id = CommonTestData.Next(),
				Fqdn = CommonTestData.Fqdn, US = "M100", Title = "Matti, Facebook", };
			_dbc.AuthenticationThings.Add(M100);
			_dbc.SaveChanges();
			CommonTestData.Entities["M100"] = M100;

			//things
			var T1 = new RegularThing
			{
				Id = CommonTestData.Next(),
				Fqdn = CommonTestData.Fqdn,
				US = "T1",
				Title = "MySuitcase",
				Created = new DateTime(2015, 3, 1),
				IsLocalOnly = true,
				StatusId = 1,
				LocationTypeId = 1,
				Logging = true,
				Preferred_LocationTypeId = 1,
				Modified = new DateTime(2016, 3, 23),
				Published = new DateTime(2016, 4, 13),
				CreatorThingId = M100.Id,
			};
			_dbc.RegularThings.Add(T1);
			CommonTestData.Entities["T1"] = T1;



			var T2 = new RegularThing
			{
				Id = CommonTestData.Next(),
				Fqdn = CommonTestData.Fqdn,
				US = "T2",
				Title = "A Container",
				Created = new DateTime(2015, 3, 1),
				IsLocalOnly = true,
				StatusId = 1,
				LocationTypeId = 2,
				Location_Gps = "(123.8, 56.9)",
				Logging = true,
				Preferred_LocationTypeId = 1,
				Modified = new DateTime(2014, 3, 3),
				Published = new DateTime(2012, 4, 13)
			};
			_dbc.RegularThings.Add(T2);
			_dbc.SaveChanges();
			CommonTestData.Entities["T2"] = T2;

			_dbc.RegularThings.Add(new RegularThing
			{
				Fqdn = CommonTestData.Fqdn,
				US = "ThingNb3",
				Title = "A Thing",
				Created = new DateTime(2016, 3, 1),
				IsLocalOnly = true,
				StatusId = 1,
				LocationTypeId = 1,
				Location_Gps = "(12.0, 43.9)",
				Logging = true,
				Preferred_LocationTypeId = 1,
				Modified = new DateTime(2016, 3, 23),
				Published = new DateTime(2016, 4, 13),
				PartedThingId = T2.Id,
			});
			_dbc.SaveChanges();

			//ThingRoleMember
			// add omnipotent role to T0 for T0
			ThingRole tr = new ThingRole { Id = CommonTestData.Next(), RoleId = (int)RoleEnum.Omnipotent, ThingId = M100.Id };
			_dbc.ThingRoles.Add(tr);
			ThingRoleMember trm = new ThingRoleMember { Id = CommonTestData.Next(), ThingId = M100.Id, ThingRoleId = tr.Id };
			_dbc.ThingRoleMembers.Add(trm);

			//add owner role to T1 for T0
			tr = new ThingRole { Id = CommonTestData.Next(), RoleId = (int)RoleEnum.Owner, ThingId = T1.Id };
			_dbc.ThingRoles.Add(tr);
			trm = new ThingRoleMember { Id = CommonTestData.Next(), ThingId = M100.Id, ThingRoleId = tr.Id };
			_dbc.ThingRoleMembers.Add(trm);

			//add Maintenance role to T2 for T1
			tr = new ThingRole { Id = CommonTestData.Next(), RoleId = (int)RoleEnum.Maintenance, ThingId = T2.Id };
			_dbc.ThingRoles.Add(tr);
			trm = new ThingRoleMember { Id = CommonTestData.Next(), ThingId = T1.Id, ThingRoleId = tr.Id };
			_dbc.ThingRoleMembers.Add(trm);

			_dbc.SaveChanges();


			//ThingRelation
			_dbc.ThingRelations.Add(new ThingRelation
			{
				Id = CommonTestData.Next(),
				FromThingId = M100.Id,
				ToThingId = T1.Id,
				RelationId = (int)RelationEnum.Belongings
			});
			_dbc.ThingRelations.Add(new ThingRelation
			{
				Id = CommonTestData.Next(),
				FromThingId = M100.Id,
				ToThingId = T1.Id,
				RelationId = (int)RelationEnum.RoleIn
			});
			_dbc.ThingRelations.Add(new ThingRelation
			{
				Id = CommonTestData.Next(),
				FromThingId = T1.Id,
				ToThingId = T2.Id,
				RelationId = (int)RelationEnum.ContainedBy
			});


			_dbc.SaveChanges();
			// test session data
			// add session 00000001 for T0, no sessionaccess yet
			var S1 = new Session
			{
				Id = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1),
				StartTime = DateTime.UtcNow,
				EntryPoint_ThingId = M100.Id,
		//		InventoryId = CommonTestData.Entities["I1"].Id,
			};
			_dbc.Sessions.Add(S1);

			//anonymous
			_dbc.Sessions.Add( new Session
			{
				Id = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2),
				StartTime = DateTime.UtcNow,
				EntryPoint_ThingId = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1),
			});

			_dbc.SaveChanges();
			CommonTestData.Entities["S1"] = S1;
		}


		private void AddAttributeData(EfContext dbc, Type enumType)
		{
			var entityType = dbc.Model.FindEntityType(typeof(Entities.Attribute));
			var table = entityType.SqlServer();
			var tableName = table.Schema + "." + table.TableName;

			foreach (var item in Enum.GetNames(enumType))
			{
				//ToDo: Add min/max/Pattern and so on...
				dbc.Attributes.Add(new Entities.Attribute { Id = (int)Enum.Parse(enumType, item, false), Name = item });
			}
			CommonTestData.SaveIdentityOnData<Entities.Attribute>(dbc);
		}

	}
}
