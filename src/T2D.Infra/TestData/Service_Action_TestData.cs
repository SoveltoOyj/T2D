﻿using System;
using System.Collections.Generic;
using System.Text;
using T2D.Entities;

namespace T2D.Infra.TestData
{
	public class Service_Action_TestData
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
				CreatorThingId = ((IThing)CommonTestData.Entities["M100"]).Id,
			};
			_dbc.RegularThings.Add(_streetlight);
			_dbc.SaveChanges();
		}

		private void InsertServiceDefinitions()
		{
			var sd = new ServiceDefinition
			{
				Id = CommonTestData.Next(),
				ThingId = _streetlight.Id,
				Title = "Huollettava, lamppu sammunut",
				TimeSpan = new TimeSpan(0, 1, 0),
				Alarm_ThingId = CommonTestData.Entities["M100"].Id,
			};
			_dbc.ServiceDefinitions.Add(sd);

			_dbc.ActionDefinitions.Add(
				new GenericAction
				{
					Id = CommonTestData.Next(),
					Title = "Change bulb",
					ServiceDefinitionId = sd.Id,
					ActionListType = ActionListType.Mandatory,
					Object_ThingId = _streetlight.Id,
					Operator_ThingId = CommonTestData.Entities["M100"].Id,
					
				});

			sd = new ServiceDefinition
			{
				Id = CommonTestData.Next(),
				ThingId = _streetlight.Id,
				Title = "Hämäräkytkin/ajastusongelma",
				TimeSpan = new TimeSpan(0, 1, 0),
				Alarm_ThingId = CommonTestData.Entities["M100"].Id,
			};
			_dbc.ServiceDefinitions.Add(sd);

			_dbc.ActionDefinitions.Add(
				new GenericAction
				{
					Id = CommonTestData.Next(),
					Title = "Check darkness switch",
					ServiceDefinitionId = sd.Id,
					ActionListType = ActionListType.Mandatory,
					Object_ThingId = _streetlight.Id,
					Operator_ThingId = CommonTestData.Entities["M100"].Id,
				});
			_dbc.SaveChanges();


			sd = new ServiceDefinition
			{
				Id = CommonTestData.Next(),
				ThingId = _streetlight.Id,
				Title = "Remove StreetLight",
				TimeSpan = new TimeSpan(0, 1, 0),
				Alarm_ThingId = CommonTestData.Entities["M100"].Id,
			};
			_dbc.ServiceDefinitions.Add(sd);

			_dbc.ActionDefinitions.Add(
				new GenericAction
				{
					Id = CommonTestData.Next(),
					Title = "Switch SL Off",
					ServiceDefinitionId = sd.Id,
					ActionListType = ActionListType.Mandatory,
					Object_ThingId = _streetlight.Id,
					Operator_ThingId = CommonTestData.Entities["M100"].Id,
				});

			_dbc.ActionDefinitions.Add(
				new GenericAction
				{
					Id = CommonTestData.Next(),
					Title = "Remove SL totally",
					ServiceDefinitionId = sd.Id,
					ActionListType = ActionListType.Mandatory,
					Object_ThingId = _streetlight.Id,
					Operator_ThingId = CommonTestData.Entities["M100"].Id,
				});

			_dbc.SaveChanges();


		}
	}
}
