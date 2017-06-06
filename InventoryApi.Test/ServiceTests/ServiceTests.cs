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
			var newServiceDef = await CreateUserAndServiceDefinition(new TimeSpan(0, 10, 0));

			//activate 2 times
			for (int i = 0; i < 2; i++)
			{
				await this.ActivateService(newServiceDef);
			}

			//read Service Statuses, last 10
			GetServiceStatusResponse resultSS;
			{
				var jsonContent = new JsonContent(new GetServiceStatusRequest
				{
					Session = newServiceDef.CreateServiceTypeRequest.Session,
					ThingId = newServiceDef.CreateServiceTypeRequest.ThingId,
					Role = newServiceDef.CreateServiceTypeRequest.Role,
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
					Session = newServiceDef.CreateServiceTypeRequest.Session,
					ThingId = newServiceDef.CreateServiceTypeRequest.ThingId,
					Role = newServiceDef.CreateServiceTypeRequest.Role,
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
			Guid actionId = (await GetActionStatuses(newServiceDef)).Statuses.First().ActionId;

			//get ActionStatus

			var getActionStatusRequest = new GetActionStatusRequest
			{
				Session = newServiceDef.CreateServiceTypeRequest.Session,
				ThingId = newServiceDef.UserThingId,
				Role = newServiceDef.CreateServiceTypeRequest.Role,
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
			await SetActionStatus(newServiceDef, actionId, "Done");
		}

		public class NewUserServiceDef
		{
			public string TestThingId { get; set; }
			public string UserThingId { get; set; }
			public string ServiceTitle { get; set; }

			public CreateServiceTypeRequest CreateServiceTypeRequest { get; set; }
		}

		public async Task<NewUserServiceDef> CreateUserAndServiceDefinition(TimeSpan? timespan, 
			int mandatoryCount = 2, 
			int selectCount = 0,
			int optionalCount = 0,
			int pendingCount = 0
			)
		{
			NewUserServiceDef ret = new NewUserServiceDef();
			ret.TestThingId = await CreateATestThing();
			ret.UserThingId = await CreateTestAuthenticatedThing();
			ret.ServiceTitle = $"New ServiceDef@{DateTime.Now.ToString()}";
			//create Service definition where operator is this new user
			ret.CreateServiceTypeRequest = new CreateServiceTypeRequest
			{
				Session = _authenticatedSession,
				ThingId = ret.TestThingId,
				Role = "Owner",
				Title = ret.ServiceTitle,
				Timespan = timespan,
				AlarmThingId = $"{_cfqdn}/M100",
				MandatoryActions = new List<ActionDefinition>(),
				SelectedActions = new List<ActionDefinition>(),
				OptionalActions = new List<ActionDefinition>(),
				PendingActions = new List<ActionDefinition>(),
			};
			for (int i = 0; i < mandatoryCount; i++)
			{
				ret.CreateServiceTypeRequest.MandatoryActions.Add(
					new ActionDefinition
					{
						ActionType = T2D.Model.Enums.ActionType.GenericAction,
						ObjectThingId = $"{_cfqdn}/T1",
						OperatorThingId = ret.UserThingId,
						Title = $"MandatoryHomma {i}"
					}
				);
			}

			for (int i = 0; i < selectCount; i++)
			{
				ret.CreateServiceTypeRequest.SelectedActions.Add(
					new ActionDefinition
					{
						ActionType = T2D.Model.Enums.ActionType.GenericAction,
						ObjectThingId = $"{_cfqdn}/T1",
						OperatorThingId = ret.UserThingId,
						Title = $"SelectedHomma {i}"
					}
				);
			}
			for (int i = 0; i < optionalCount; i++)
			{
				ret.CreateServiceTypeRequest.OptionalActions.Add(
					new ActionDefinition
					{
						ActionType = T2D.Model.Enums.ActionType.GenericAction,
						ObjectThingId = $"{_cfqdn}/T1",
						OperatorThingId = ret.UserThingId,
						Title = $"OptionalHomma {i}"
					}
				);
			}
			for (int i = 0; i < pendingCount; i++)
			{
				ret.CreateServiceTypeRequest.PendingActions.Add(
					new ActionDefinition
					{
						ActionType = T2D.Model.Enums.ActionType.GenericAction,
						ObjectThingId = $"{_cfqdn}/T1",
						OperatorThingId = ret.UserThingId,
						Title = $"PendingHomma {i}"
					}
				);
			}

			{
				var jsonContent = new JsonContent(ret.CreateServiceTypeRequest);
				var response = await _client.PostAsync($"{_url}/CreateService", jsonContent);
				response.EnsureSuccessStatusCode();
			}

			//get
			{
				var jsonContent = new JsonContent(new GetServicesRequest
				{
					Session = ret.CreateServiceTypeRequest.Session,
					ThingId = ret.CreateServiceTypeRequest.ThingId,
					Role = ret.CreateServiceTypeRequest.Role,
				});
				var response = await _client.PostAsync($"{_url}/GetServices", jsonContent);
				response.EnsureSuccessStatusCode();
				var result = await response.Content.ReadAsJsonAsync<GetServicesResponse>();

				Assert.NotNull(result);
				Assert.NotNull(result.Services);
				Assert.NotEmpty(result.Services);
				Assert.True(result.Services.Count() == 1);
				Assert.True(result.Services[0] == ret.CreateServiceTypeRequest.Title);
			}

			return ret;
		}

		public async Task<ServiceStatusResponse> ActivateService(NewUserServiceDef newServiceDef)
		{
			{
				var jsonContent = new JsonContent(new ServiceRequestRequest
				{
					Session = newServiceDef.CreateServiceTypeRequest.Session,
					ThingId = newServiceDef.CreateServiceTypeRequest.ThingId,
					Role = newServiceDef.CreateServiceTypeRequest.Role,
					Service = newServiceDef.CreateServiceTypeRequest.Title,
				});
				var response = await _client.PostAsync($"{_url}/ServiceRequest", jsonContent);
				response.EnsureSuccessStatusCode();
			}

			return (await GetServiceStatus(newServiceDef))[0];
		}

		public async Task<List<ServiceStatusResponse>> GetServiceStatus(NewUserServiceDef newServiceDef)
		{

			GetServiceStatusResponse resultSS;
			{
				var jsonContent = new JsonContent(new GetServiceStatusRequest
				{
					Session = newServiceDef.CreateServiceTypeRequest.Session,
					ThingId = newServiceDef.CreateServiceTypeRequest.ThingId,
					Role = newServiceDef.CreateServiceTypeRequest.Role,
					ServiceId = null,
				});
				var response = await _client.PostAsync($"{_url}/GetServiceStatus", jsonContent);
				response.EnsureSuccessStatusCode();
				resultSS = await response.Content.ReadAsJsonAsync<GetServiceStatusResponse>();

				Assert.NotNull(resultSS);
				Assert.NotNull(resultSS.Statuses);
				Assert.NotEmpty(resultSS.Statuses);
				Assert.True(resultSS.Statuses.Count() >= 1);
			}
			return resultSS.Statuses;
		}

		public async Task<GetActionStatusesResponse> GetActionStatuses(NewUserServiceDef newServiceDef)
		{
			var jsonContent = new JsonContent(new GetActionStatusesRequest
			{
				Session = newServiceDef.CreateServiceTypeRequest.Session,
				ThingId = newServiceDef.UserThingId,
				Role = newServiceDef.CreateServiceTypeRequest.Role,
			});
			var response = await _client.PostAsync($"{_url}/GetActionStatuses", jsonContent);
			response.EnsureSuccessStatusCode();
			var resultAS = await response.Content.ReadAsJsonAsync<GetActionStatusesResponse>();

			Assert.NotNull(resultAS);
			Assert.NotNull(resultAS.Statuses);
			Assert.NotEmpty(resultAS.Statuses);
			Assert.True(resultAS.Statuses.Count() >= 2);

			return resultAS;
		}
		public async Task<List<ActionStatusResponse>> GetActionStatusesForM100()
		{
			var jsonContent = new JsonContent(new GetActionStatusesRequest
			{
				Session = _authenticatedSession,
				ThingId = $"{_cfqdn}/M100",
				Role = "Omnipotent",
			});
			var response = await _client.PostAsync($"{_url}/GetActionStatuses", jsonContent);
			response.EnsureSuccessStatusCode();
			var resultAS = await response.Content.ReadAsJsonAsync<GetActionStatusesResponse>();

			Assert.NotNull(resultAS);
			Assert.NotNull(resultAS.Statuses);

			return resultAS.Statuses;
		}

		public async Task<GetActionStatusResponse> SetActionStatus(NewUserServiceDef newServiceDef, Guid actionId, string state)
		{
			var jsonContent = new JsonContent(new UpdateActionStatusRequest
			{
				Session = newServiceDef.CreateServiceTypeRequest.Session,
				ThingId = newServiceDef.UserThingId,
				Role = newServiceDef.CreateServiceTypeRequest.Role,
				ActionId = actionId,
				State = state,
			});

			var getActionStatusRequest = new GetActionStatusRequest
			{
				Session = newServiceDef.CreateServiceTypeRequest.Session,
				ThingId = newServiceDef.UserThingId,
				Role = newServiceDef.CreateServiceTypeRequest.Role,
				ActionId = actionId,
			};

			var response = await _client.PostAsync($"{_url}/UpdateActionStatus", jsonContent);
			response.EnsureSuccessStatusCode();
			//read status
			jsonContent = new JsonContent(getActionStatusRequest);
			response = await _client.PostAsync($"{_url}/GetActionStatus", jsonContent);
			response.EnsureSuccessStatusCode();
			var resultAS1 = await response.Content.ReadAsJsonAsync<GetActionStatusResponse>();
			Assert.NotNull(resultAS1);
			Assert.NotNull(resultAS1.Action);
			Assert.True(resultAS1.Action.State == state);

			return resultAS1;
		}

	}
}
