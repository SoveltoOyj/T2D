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
		public async void CreateGetAndActivateServiceDefinition_OK()
		{
			string thingId = await CreateATestThing();

			var createServiceTypeRequest = new CreateServiceTypeRequest
			{
				Session = "00000000-0000-0000-0000-000000000001",
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
						OperatorThingId = $"{_cfqdn}/M100",
						Timespan = null,
						Title="Homma 1"
					},
					new ActionDefinition
					{
						ActionType = T2D.Model.Enums.ActionType.GenericAction,
						AlarmThingId=$"{_cfqdn}/M100",
						ObjectThingId = $"{_cfqdn}/T2",
						OperatorThingId = $"{_cfqdn}/M100",
						Timespan = new TimeSpan(0,10,0),
						Title="Homma 2"
					},
				},
			};

			var jsonContent = new JsonContent(createServiceTypeRequest);


			var response = await _client.PostAsync($"{_url}/CreateService", jsonContent);
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

			//activate
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

	}

}
