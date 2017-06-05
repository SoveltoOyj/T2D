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
			//create Service definition where operator is this new user
			var createServiceTypeRequest = new CreateServiceTypeRequest
			{
				Session = _authenticatedSession,
				ThingId = thingId,
				Role = "Owner",
				Title = $"New ServiceDef@{DateTime.Now.ToString()}",
				Timespan = new TimeSpan(0, 10, 0),
				AlarmThingId = $"{_cfqdn}/M100",
				MandatoryActions = new List<ActionDefinition>
				{
					new ActionDefinition
					{
						ActionType = T2D.Model.Enums.ActionType.GenericAction,
						ObjectThingId = $"{_cfqdn}/T1",
						OperatorThingId = userId,
						Title="Homma 1"
					},
					new ActionDefinition
					{
						ActionType = T2D.Model.Enums.ActionType.GenericAction,
						ObjectThingId = $"{_cfqdn}/T2",
						OperatorThingId = userId,
						Title="Homma 2"
					},
				},
			};
			{
				var jsonContent = new JsonContent(createServiceTypeRequest);
				var response = await _client.PostAsync($"{_url}/CreateService", jsonContent);
				response.EnsureSuccessStatusCode();
			}

			//get
			{
				var jsonContent = new JsonContent(new GetServicesRequest
				{
					Session = createServiceTypeRequest.Session,
					ThingId = createServiceTypeRequest.ThingId,
					Role = createServiceTypeRequest.Role,
				});
				var response = await _client.PostAsync($"{_url}/GetServices", jsonContent);
				response.EnsureSuccessStatusCode();
				var result = await response.Content.ReadAsJsonAsync<GetServicesResponse>();

				Assert.NotNull(result);
				Assert.NotNull(result.Services);
				Assert.NotEmpty(result.Services);
				Assert.True(result.Services.Count() == 1);
				Assert.True(result.Services[0] == createServiceTypeRequest.Title);
			}

			//activate 2 times
			{
				for (int i = 0; i < 2; i++)
				{
					var jsonContent = new JsonContent(new ServiceRequestRequest
					{
						Session = createServiceTypeRequest.Session,
						ThingId = createServiceTypeRequest.ThingId,
						Role = createServiceTypeRequest.Role,
						Service = createServiceTypeRequest.Title,
					});
					var response = await _client.PostAsync($"{_url}/ServiceRequest", jsonContent);
					response.EnsureSuccessStatusCode();
				}
			}

			//read Service Statuses, last 10
			GetServiceStatusResponse resultSS;
			{
				var jsonContent = new JsonContent(new GetServiceStatusRequest
				{
					Session = createServiceTypeRequest.Session,
					ThingId = createServiceTypeRequest.ThingId,
					Role = createServiceTypeRequest.Role,
					ServiceId = null,
				});
				var response = await _client.PostAsync($"{_url}/GetServiceStatus", jsonContent);
				response.EnsureSuccessStatusCode();
				resultSS = await response.Content.ReadAsJsonAsync<GetServiceStatusResponse>();

				Assert.NotNull(resultSS);
				Assert.NotNull(resultSS.Statuses);
				Assert.NotEmpty(resultSS.Statuses);
				Assert.True(resultSS.Statuses.Count() == 2);
			}

			//read Service Statuses by ID
			{
				var jsonContent = new JsonContent(new GetServiceStatusRequest
				{
					Session = createServiceTypeRequest.Session,
					ThingId = createServiceTypeRequest.ThingId,
					Role = createServiceTypeRequest.Role,
					ServiceId = resultSS.Statuses[0].ServiceId,
				});
				var response = await _client.PostAsync($"{_url}/GetServiceStatus", jsonContent);
				response.EnsureSuccessStatusCode();
				resultSS = await response.Content.ReadAsJsonAsync<GetServiceStatusResponse>();

				Assert.NotNull(resultSS);
				Assert.NotNull(resultSS.Statuses);
				Assert.NotEmpty(resultSS.Statuses);
				Assert.True(resultSS.Statuses.Count() == 1);
			}

			//GetActionStatuses
			Guid actionId;
			{
				var jsonContent = new JsonContent(new GetActionStatusesRequest
				{
					Session = createServiceTypeRequest.Session,
					ThingId = userId,
					Role = createServiceTypeRequest.Role,
				});
				var response = await _client.PostAsync($"{_url}/GetActionStatuses", jsonContent);
				response.EnsureSuccessStatusCode();
				var resultAS = await response.Content.ReadAsJsonAsync<GetActionStatusesResponse>();

				Assert.NotNull(resultAS);
				Assert.NotNull(resultAS.Statuses);
				Assert.NotEmpty(resultAS.Statuses);
				Assert.True(resultAS.Statuses.Count() >= 4);
				actionId = resultAS.Statuses.First().ActionId;
			}

			//get ActionStatus

			var getActionStatusRequest = new GetActionStatusRequest
			{
				Session = createServiceTypeRequest.Session,
				ThingId = userId,
				Role = createServiceTypeRequest.Role,
				ActionId = actionId,
			};
			{
				var jsonContent = new JsonContent(getActionStatusRequest);
				var response = await _client.PostAsync($"{_url}/GetActionStatus", jsonContent);
				response.EnsureSuccessStatusCode();
				var resultAS1 = await response.Content.ReadAsJsonAsync<GetActionStatusResponse>();
				Assert.NotNull(resultAS1);
				Assert.NotNull(resultAS1.Action);
				Assert.True(resultAS1.Action.State == "NotStarted");
				Assert.True(resultAS1.Action.Service.Alarm_ThingId == $"{_cfqdn}/M100");
			}

			//Update Status and read it
			{
				var jsonContent = new JsonContent(new UpdateActionStatusRequest
				{
					Session = createServiceTypeRequest.Session,
					ThingId = userId,
					Role = createServiceTypeRequest.Role,
					ActionId = actionId,
					State = "Done",
				});
				var response = await _client.PostAsync($"{_url}/UpdateActionStatus", jsonContent);
				response.EnsureSuccessStatusCode();
				//read status
				jsonContent = new JsonContent(getActionStatusRequest);
				response = await _client.PostAsync($"{_url}/GetActionStatus", jsonContent);
				response.EnsureSuccessStatusCode();
				var resultAS1 = await response.Content.ReadAsJsonAsync<GetActionStatusResponse>();
				Assert.NotNull(resultAS1);
				Assert.NotNull(resultAS1.Action);
				Assert.True(resultAS1.Action.State == "Done");
			}
		}

	}

}
