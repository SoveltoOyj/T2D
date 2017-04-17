using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using T2D.Model;
using T2D.Model.InventoryApi;
using T2D.Model.ServiceApi;
using Xunit;
using Xunit.Abstractions;

namespace InventoryApi.Test
{
	public class ServiceTests : InventoryApiBase
	{
		private string _url = "api/inventory/service";

		public ServiceTests(ITestOutputHelper output) : base(output) { }

		[Fact]
		public async void TestWholeServicePipeline_OK()
		{
			string thingId = await CreateATestThing();
			string userId = await CreateTestAuthenticatedThing();
			//EnterAuhtenticatedSession
			var jsonContent = new JsonContent(new AuthenticationRequest
			{
				AuthenticationType = T2D.Model.Enums.AuthenticationType.Facebook,
				ThingId = userId,
			});
			var response = await _client.PostAsync($"api/inventory/Authentication/EnterAuthenticatedSession", jsonContent);
			var sessionResult = await response.Content.ReadAsJsonAsync<AuthenticationResponse>();

			response.EnsureSuccessStatusCode();
			Assert.NotNull(sessionResult);
			var session = sessionResult.Session;

			//create Service definition where operator is this new user
			var createServiceTypeRequest = new CreateServiceTypeRequest
			{
				Session = session,
				ThingId = thingId,
				Role = "Owner",
				Title = $"New ServiceDef@{DateTime.Now.ToString()}",
				MandatoryActions = new List<ActionDefinition>
				{
					new ActionDefinition
					{
						ActionType = T2D.Model.Enums.ActionType.GenericAction,
						AlarmThingId=null,
						ObjectThingId = $"{_cfqdn}/T1",
						OperatorThingId = userId,
						Timespan = null,
						Title="Homma 1"
					},
					new ActionDefinition
					{
						ActionType = T2D.Model.Enums.ActionType.GenericAction,
						AlarmThingId=$"{_cfqdn}/M100",
						ObjectThingId = $"{_cfqdn}/T2",
						OperatorThingId = userId,
						Timespan = new TimeSpan(0,10,0),
						Title="Homma 2"
					},
				},
			};

			jsonContent = new JsonContent(createServiceTypeRequest);
			response = await _client.PostAsync($"{_url}/CreateService", jsonContent);
			response.EnsureSuccessStatusCode();

			//get
			jsonContent = new JsonContent(new GetServicesRequest
			{
				Session = createServiceTypeRequest.Session,
				ThingId = createServiceTypeRequest.ThingId,
				Role = createServiceTypeRequest.Role,
			});
			response = await _client.PostAsync($"{_url}/GetServices", jsonContent);
			response.EnsureSuccessStatusCode();
			var result = await response.Content.ReadAsJsonAsync<GetServicesResponse>();

			Assert.NotNull(result);
			Assert.NotNull(result.Services);
			Assert.NotEmpty(result.Services);
			Assert.True(result.Services.Count() == 1);
			Assert.True(result.Services[0] == createServiceTypeRequest.Title);

			//activate 2 times
			for (int i = 0; i < 2; i++)
			{
				jsonContent = new JsonContent(new ServiceRequestRequest
				{
					Session = createServiceTypeRequest.Session,
					ThingId = createServiceTypeRequest.ThingId,
					Role = createServiceTypeRequest.Role,
					Service = createServiceTypeRequest.Title,
				});
				response = await _client.PostAsync($"{_url}/ServiceRequest", jsonContent);
				response.EnsureSuccessStatusCode();
			}
			//read Service Statuses, last 10
			jsonContent = new JsonContent(new GetServiceStatusRequest
			{
				Session = createServiceTypeRequest.Session,
				ThingId = createServiceTypeRequest.ThingId,
				Role = createServiceTypeRequest.Role,
				ServiceId=null,
			});
			response = await _client.PostAsync($"{_url}/GetServiceStatus", jsonContent);
			response.EnsureSuccessStatusCode();
			var resultSS = await response.Content.ReadAsJsonAsync<GetServiceStatusResponse>();

			Assert.NotNull(resultSS);
			Assert.NotNull(resultSS.Statuses);
			Assert.NotEmpty(resultSS.Statuses);
			Assert.True(resultSS.Statuses.Count() == 2);

			//read Service Statuses by ID
			jsonContent = new JsonContent(new GetServiceStatusRequest
			{
				Session = createServiceTypeRequest.Session,
				ThingId = createServiceTypeRequest.ThingId,
				Role = createServiceTypeRequest.Role,
				ServiceId = resultSS.Statuses[0].ServiceId,
			});
			response = await _client.PostAsync($"{_url}/GetServiceStatus", jsonContent);
			response.EnsureSuccessStatusCode();
			resultSS = await response.Content.ReadAsJsonAsync<GetServiceStatusResponse>();

			Assert.NotNull(resultSS);
			Assert.NotNull(resultSS.Statuses);
			Assert.NotEmpty(resultSS.Statuses);
			Assert.True(resultSS.Statuses.Count() == 1);

			//GetActionStatuses
			jsonContent = new JsonContent(new GetActionStatusesRequest
			{
				Session = createServiceTypeRequest.Session,
				ThingId = userId,
				Role = createServiceTypeRequest.Role,
			});
			response = await _client.PostAsync($"{_url}/GetActionStatuses", jsonContent);
			response.EnsureSuccessStatusCode();
			var resultAS = await response.Content.ReadAsJsonAsync<GetActionStatusesResponse>();

			Assert.NotNull(resultAS);
			Assert.NotNull(resultAS.Statuses);
			Assert.NotEmpty(resultAS.Statuses);
			Assert.True(resultAS.Statuses.Count() >= 4);

			var actionId = resultAS.Statuses.First().ActionId;

			//get ActionStatus
			var getActionStatusRequest = new GetActionStatusRequest
			{
				Session = createServiceTypeRequest.Session,
				ThingId = userId,
				Role = createServiceTypeRequest.Role,
				ActionId = actionId,
			};
			jsonContent = new JsonContent(getActionStatusRequest);
			response = await _client.PostAsync($"{_url}/GetActionStatus", jsonContent);
			response.EnsureSuccessStatusCode();
			var resultAS1 = await response.Content.ReadAsJsonAsync<GetActionStatusResponse>();
			Assert.NotNull(resultAS1);
			Assert.NotNull(resultAS1.Action);
			Assert.True(resultAS1.Action.State == "NotStarted");

			//Update Status and read it
			jsonContent = new JsonContent(new UpdateActionStatusRequest
			{
				Session = createServiceTypeRequest.Session,
				ThingId = userId,
				Role = createServiceTypeRequest.Role,
				ActionId = actionId,
				State = "Done",
			});
			response = await _client.PostAsync($"{_url}/UpdateActionStatus", jsonContent);
			response.EnsureSuccessStatusCode();
			//read status
			jsonContent = new JsonContent(getActionStatusRequest);
			response = await _client.PostAsync($"{_url}/GetActionStatus", jsonContent);
			response.EnsureSuccessStatusCode();
			resultAS1 = await response.Content.ReadAsJsonAsync<GetActionStatusResponse>();
			Assert.NotNull(resultAS1);
			Assert.NotNull(resultAS1.Action);
			Assert.True(resultAS1.Action.State == "Done");

		}

	}

}
