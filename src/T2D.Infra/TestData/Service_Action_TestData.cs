using System;
using System.Collections.Generic;
using System.Text;
using T2D.Entities;

namespace T2D.Infra.TestData
{
	class Service_Action_TestData
	{
		private EfContext _dbc { get; set; }

		private RegularThing _streetlight;
		public Service_Action_TestData(EfContext dbc)
		{
			this._dbc = dbc;
		}
		public void DoIt()
		{
			InsertThings();
			InsertServiceDefinitions();
		}
		private void InsertThings()
		{
			_streetlight = new RegularThing
			{
				Id = CommonTestData.Next(),
				Fqdn = CommonTestData.Fqdn,
				US = "SL1",
				Title = "Streetlight #1",
				Created = new DateTime(2015, 3, 1),
				IsLocalOnly = true,
				StatusId = 1,
				LocationTypeId = 1,
				Logging = true,
				Preferred_LocationTypeId = 1,
				Modified = new DateTime(2016, 3, 23),
				Published = new DateTime(2016, 4, 13),
				Creator_Fqdn = ((IThing) CommonTestData.Entities["M100"]).Fqdn,
				Creator_US = ((IThing)CommonTestData.Entities["M100"]).US
			};
			_dbc.RegularThings.Add(_streetlight);
			_dbc.SaveChanges();
		}

		private void InsertServiceDefinitions()
		{
			var sd = new ServiceDefinition
			{
				Id = CommonTestData.Next(),
				ThingId=_streetlight.Id,
				Title="Huollettava, lamppu sammunut"
			};
			_dbc.ServiceDefinitions.Add(sd);

			_dbc.ActionDefinitions.Add(
				new GenericAction
				{
					Id = CommonTestData.Next(),
					Title = "Change bulb",
					ServiceDefinitionId = sd.Id,
					ActionListType = ActionListType.Mandatory,
					Alarm_ThingId = CommonTestData.Entities["M100"].Id,
					Object_ThingId = _streetlight.Id,
					Operator_ThingId = CommonTestData.Entities["M100"].Id,
					TimeSpan =new TimeSpan(0,1,0),
				});

			sd = new ServiceDefinition
			{
				Id = CommonTestData.Next(),
				ThingId = _streetlight.Id,
				Title = "Hämäräkytkin/ajastusongelma"
			};
			_dbc.ServiceDefinitions.Add(sd);

			_dbc.ActionDefinitions.Add(
				new GenericAction
				{
					Id = CommonTestData.Next(),
					Title = "Check darkness switch",
					ServiceDefinitionId = sd.Id,
					ActionListType = ActionListType.Mandatory,
					Alarm_ThingId = CommonTestData.Entities["M100"].Id,
					Object_ThingId = _streetlight.Id,
					Operator_ThingId = CommonTestData.Entities["M100"].Id,
					TimeSpan = new TimeSpan(0, 1, 0),
				});
			_dbc.SaveChanges();

		}
	}
}
