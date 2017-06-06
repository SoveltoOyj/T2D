using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using T2D.Infra;
using T2D.InventoryBL.ServiceRequest;
using T2D.Model;
using T2D.Model.InventoryApi;
using T2D.Model.ServiceApi;
using Xunit;
using Xunit.Abstractions;

namespace InventoryApi.Test
{
	public class ServiceBLTests : InventoryBLBase
	{
		public ServiceBLTests(ITestOutputHelper output) : base(output) { }

		[Fact]
		public async void OnlyMandatoryActions_NoDeadline_ServiceStatus()
		{
			// Create a serviceRequest
			var serviceTest = new ServiceTests(_output);

			ServiceTests.NewUserServiceDef newServiceDef = await serviceTest.CreateUserAndServiceDefinition(null);

			//Activate it
			var serviceStatus = await serviceTest.ActivateService(newServiceDef);

			Assert.True(serviceStatus.DeadLine == null);
			Assert.True(serviceStatus.State == "NotStarted");

			//first action to done, service status should be Started
			var actionStatuses = (await serviceTest.GetActionStatuses(newServiceDef)).Statuses;
			Guid actionId = actionStatuses.First().ActionId;

			await serviceTest.SetActionStatus(newServiceDef, actionId, "Started");
			serviceStatus = (await serviceTest.GetServiceStatus(newServiceDef))[0];
			Assert.True(serviceStatus.State == "Started");

			await serviceTest.SetActionStatus(newServiceDef, actionId, "Done");
			serviceStatus = (await serviceTest.GetServiceStatus(newServiceDef))[0];
			Assert.True(serviceStatus.State == "Started");


			//Second action to done, service status should turn to Done
			actionId = actionStatuses.Last().ActionId; ;

			await serviceTest.SetActionStatus(newServiceDef, actionId, "Started");
			serviceStatus = (await serviceTest.GetServiceStatus(newServiceDef))[0];
			Assert.True(serviceStatus.State == "Started");

			await serviceTest.SetActionStatus(newServiceDef, actionId, "Done");
			serviceStatus = (await serviceTest.GetServiceStatus(newServiceDef))[0];
			Assert.True(serviceStatus.State == "Done");

		}


		[Fact]
		public async void OnlyMandatoryActions_Deadline_ServiceStatus()
		{
			var connectionStr = Configuration["ConnectionStrings:T2DConnection"];
			// Create a serviceRequest
			var serviceTest = new ServiceTests(_output);
			ServiceTests.NewUserServiceDef newServiceDef = await serviceTest.CreateUserAndServiceDefinition(null);


			//Activate it
			var serviceStatus = await serviceTest.ActivateService(newServiceDef);

			//set Deadline
			EfContext dbc = new EfContext(connectionStr);
			var dbServiceStatus = dbc.ServiceStatuses.SingleOrDefault(st=>st.Id == serviceStatus.ServiceId);
			Assert.NotNull(dbServiceStatus);
			Assert.Null(dbServiceStatus.DeadLine);
			dbServiceStatus.DeadLine = DateTime.UtcNow.AddSeconds(10);
			dbc.SaveChanges();
			dbc.Dispose();

			serviceStatus = (await serviceTest.GetServiceStatus(newServiceDef))[0];
			Assert.True(serviceStatus.DeadLine != null);
			Assert.True(serviceStatus.State == "NotStarted");

			//first action to done, service status should be Started
			var actionStatuses = (await serviceTest.GetActionStatuses(newServiceDef)).Statuses;
			Guid actionId = actionStatuses.First().ActionId;

			await serviceTest.SetActionStatus(newServiceDef, actionId, "Started");
			serviceStatus = (await serviceTest.GetServiceStatus(newServiceDef))[0];
			Assert.True(serviceStatus.State == "Started");

			await serviceTest.SetActionStatus(newServiceDef, actionId, "Done");
			serviceStatus = (await serviceTest.GetServiceStatus(newServiceDef))[0];
			Assert.True(serviceStatus.State == "Started");

			//Wait 10 seconds, activate batchJob and service status should be NotDoneInTime
			var m100ASsPre = (await serviceTest.GetActionStatusesForM100()).Where(ass=>ass.ActionType == "Alarm").ToList();
			await Task.Delay(1000 * 10);
			ServiceBL.ScheduleUpdateServiceStatus(connectionStr, serviceStatus.ServiceId);
			serviceStatus = (await serviceTest.GetServiceStatus(newServiceDef))[0];
			Assert.True(serviceStatus.State == "NotDoneInTime");

			// and there should be  one more Alarm actionstatus for M100
			var m100ASsPost = (await serviceTest.GetActionStatusesForM100()).Where(ass => ass.ActionType == "Alarm").ToList();
			Assert.True(m100ASsPost.Count == m100ASsPre.Count + 1);

		}


		[Fact]
		public async void Mandatory_AndSelectActions_NoDeadline_ServiceStatus()
		{
			// Create a serviceRequest
			var serviceTest = new ServiceTests(_output);

			ServiceTests.NewUserServiceDef newServiceDef = await serviceTest.CreateUserAndServiceDefinition(null,2,2);

			//Activate it
			var serviceStatus = await serviceTest.ActivateService(newServiceDef);

			Assert.True(serviceStatus.DeadLine == null);
			Assert.True(serviceStatus.State == "NotStarted");

			//first action to done, service status should be Started
			var actionStatuses = (await serviceTest.GetActionStatuses(newServiceDef)).Statuses;
			Guid actionId = actionStatuses.First(st => st.ActionType == "Mandatory").ActionId;

			await serviceTest.SetActionStatus(newServiceDef, actionId, "Started");
			serviceStatus = (await serviceTest.GetServiceStatus(newServiceDef))[0];
			Assert.True(serviceStatus.State == "Started");

			await serviceTest.SetActionStatus(newServiceDef, actionId, "Done");
			serviceStatus = (await serviceTest.GetServiceStatus(newServiceDef))[0];
			Assert.True(serviceStatus.State == "Started");


			//Second action to done, service status should stay at Done
			actionId = actionStatuses.Last(st => st.ActionType == "Mandatory").ActionId; ;

			await serviceTest.SetActionStatus(newServiceDef, actionId, "Started");
			serviceStatus = (await serviceTest.GetServiceStatus(newServiceDef))[0];
			Assert.True(serviceStatus.State == "Started");

			await serviceTest.SetActionStatus(newServiceDef, actionId, "Done");
			serviceStatus = (await serviceTest.GetServiceStatus(newServiceDef))[0];
			Assert.True(serviceStatus.State == "Started");

			//Set one of selected to "Done", service status should be Done
			actionId = actionStatuses.Last(st => st.ActionType == "Selected").ActionId; ;

			await serviceTest.SetActionStatus(newServiceDef, actionId, "Started");
			serviceStatus = (await serviceTest.GetServiceStatus(newServiceDef))[0];
			Assert.True(serviceStatus.State == "Started");

			await serviceTest.SetActionStatus(newServiceDef, actionId, "Done");
			serviceStatus = (await serviceTest.GetServiceStatus(newServiceDef))[0];
			Assert.True(serviceStatus.State == "Done");

		}

	}

}
