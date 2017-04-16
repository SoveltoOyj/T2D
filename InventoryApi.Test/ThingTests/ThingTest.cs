using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using T2D.Model;
using T2D.Model.InventoryApi;
using Xunit;
using Xunit.Abstractions;

namespace InventoryApi.Test
{
	public class ThingTest : InventoryApiBase
	{
		private string _url = "api/inventory/core";
		private string cfqdn = "inv1.sovelto.fi";

		public ThingTest(ITestOutputHelper output) : base(output){ }

		[Fact]
		public async void CreateLocalThing_ShouldBeOK()
		{
			var jsonContent = new JsonContent(new CreateLocalThingRequest {
				Session = "00000000-0000-0000-0000-000000000001",
				ThingId = $"{cfqdn}/M100",
				Role="Omnipotent",
				NewThingId =  $"{cfqdn}/Test@{DateTime.Now.ToString()}",
				Title = "joku title",
				ThingType = T2D.Model.Enums.ThingType.RegularThing,
			});
			var response = await _client.PostAsync($"{_url}/CreateLocalThing", jsonContent );

			response.EnsureSuccessStatusCode();
		}


		[Fact]
		public async void SetAndGetRoleRights_NewThing_ShouldBeOK()
		{
			string thingId = await CreateATestThing();
			var setRoleAccessRightsRequest = new SetRoleAccessRightsRequest
			{
				Session = "00000000-0000-0000-0000-000000000001",
				ThingId = thingId,
				Role = "Omnipotent",
				RoleForRights = "Owner",
				AttributeRoleRights = new List<AttributeRoleRight>
				{
					new AttributeRoleRight
					{
						Attribute = "Description",
						RoleAccessRights = new List<string> { "Create", "Read", "Update" },
					},
					new AttributeRoleRight
					{
						Attribute = "Created",
						RoleAccessRights = new List<string> { "Create", "Read" },
					},
					new AttributeRoleRight
					{
						Attribute = "Location",
						RoleAccessRights = new List<string> { "Create", "Update" },
					},
				},
			};
			var jsonContent = new JsonContent(setRoleAccessRightsRequest);
			var response = await _client.PostAsync($"{_url}/SetRoleAccessRight", jsonContent);
			response.EnsureSuccessStatusCode();

			//get
			jsonContent = new JsonContent(new GetRoleAccessRightsRequest
			{
				Session = setRoleAccessRightsRequest.Session,
				ThingId = setRoleAccessRightsRequest.ThingId,
				Role = setRoleAccessRightsRequest.Role,
				RoleForRights = setRoleAccessRightsRequest.RoleForRights,
			});
			response = await _client.PostAsync($"{_url}/GetRoleAccessRight", jsonContent);
			response.EnsureSuccessStatusCode();
			var result = await response.Content.ReadAsJsonAsync<GetRoleAccessRightsResponse>();

			Assert.NotNull(result);
			Assert.NotNull(result.AttributeRoleRights);
			Assert.NotEmpty(result.AttributeRoleRights);
			Assert.True(result.AttributeRoleRights.Count() == setRoleAccessRightsRequest.AttributeRoleRights.Count());
			foreach (var item in setRoleAccessRightsRequest.AttributeRoleRights)
			{
				var arr = result.AttributeRoleRights.SingleOrDefault(r => r.Attribute == item.Attribute);
				Assert.True( arr != null);
				Assert.True(arr.RoleAccessRights.Count() == item.RoleAccessRights.Count());
				foreach (var rar in arr.RoleAccessRights)
				{
					Assert.True(arr.RoleAccessRights.Contains(rar));
				}
			}
		}

		[Fact]
		public async void SetAndGetRoleMemberList_NewThing_ShouldBeOK()
		{
			string thingId = await CreateATestThing();
			var setRoleMemberListRequest = new SetRoleMemberListRequest
			{
				Session = "00000000-0000-0000-0000-000000000001",
				ThingId = thingId,
				Role = "Omnipotent",
				RoleForMemberList = "Owner",
				MemberThingIds = new List<string>
				{
					$"{cfqdn}/M100",
					$"{cfqdn}/AnonymousUser",
				}
			};
			var jsonContent = new JsonContent(setRoleMemberListRequest);
			var response = await _client.PostAsync($"{_url}/SetRoleMemberList", jsonContent);
			response.EnsureSuccessStatusCode();

			//get
			jsonContent = new JsonContent(new GetRoleMemberListRequest
			{
				Session = setRoleMemberListRequest.Session,
				ThingId = setRoleMemberListRequest.ThingId,
				Role = setRoleMemberListRequest.Role,
				RoleForMemberList = setRoleMemberListRequest.RoleForMemberList,
			});
			response = await _client.PostAsync($"{_url}/GetRoleMemberList", jsonContent);
			response.EnsureSuccessStatusCode();
			var result = await response.Content.ReadAsJsonAsync<GetRoleMemberListResponse>();

			Assert.NotNull(result);
			Assert.NotNull(result.ThingIds);
			Assert.NotEmpty(result.ThingIds);
			Assert.True(result.ThingIds.Count() == setRoleMemberListRequest.MemberThingIds.Count());
			foreach (var item in setRoleMemberListRequest.MemberThingIds)
			{
				var arr = result.ThingIds.SingleOrDefault(r => r == item);
				Assert.True(arr != null);
			}
		}


		[Fact]
		public async void QueryMyRoles_M100_ShouldReturnOmnipotent()
		{
			var queryMyRolesRequest = new QueryMyRolesRequest
			{
				Session = "00000000-0000-0000-0000-000000000001",
				ThingId = $"{cfqdn}/M100",
			};
			var jsonContent = new JsonContent(queryMyRolesRequest);
			var response = await _client.PostAsync($"{_url}/QueryMyRoles", jsonContent);
			response.EnsureSuccessStatusCode();

			var result = await response.Content.ReadAsJsonAsync<QueryMyRolesResponse>();

			Assert.NotNull(result);
			Assert.NotNull(result.Roles);
			Assert.True(result.Roles.Contains("Omnipotent"));
		}

		[Fact]
		public async void GetRelations_T1_ShouldReturn()
		{
			var getRelationsRequest = new GetRelationsRequest
			{
				Session = "00000000-0000-0000-0000-000000000001",
				ThingId = $"{cfqdn}/T1",
				Role = "Owner",
			};
			var jsonContent = new JsonContent(getRelationsRequest);
			var response = await _client.PostAsync($"{_url}/GetRelations", jsonContent);
			response.EnsureSuccessStatusCode();

			var result = await response.Content.ReadAsJsonAsync<GetRelationsResponse>();

			Assert.NotNull(result);
			Assert.NotNull(result.RelationThings);
			Assert.True(result.RelationThings.Count() >= 2);
		}






		//		[Fact]
		//		public async void GetT2_ShouldReturnData()
		//		{
		//			//using (var client = _server.CreateClient().AcceptJson())
		//			//{
		//			//	var response = await client.GetAsync($"{_url}/id?c={cfqdn}&u=T2");
		//			//	var result = await response.Content.ReadAsJsonAsync<BaseThing>();

		//			//	Assert.NotNull(result);
		//			//	Assert.Equal("inv1.sovelto.fi/T2", result.Id);
		//			//	Assert.NotNull(result.Title);
		//			//}
		//		}


		private async Task<string> CreateATestThing()
		{
			string thingId = $"{cfqdn}/Test@{DateTime.Now.ToString()} - {Guid.NewGuid()}";
			var jsonContent = new JsonContent(new CreateLocalThingRequest
			{
				Session = "00000000-0000-0000-0000-000000000001",
				ThingId = $"{cfqdn}/M100",
				Role= "Omnipotent",
				NewThingId = thingId,
				Title = "Test thing",
				ThingType = T2D.Model.Enums.ThingType.RegularThing,
			});
			var response = await _client.PostAsync($"{_url}/CreateLocalThing", jsonContent);
			response.EnsureSuccessStatusCode();
			return thingId;
		}
		
		//		private async Task<System.Net.HttpStatusCode> DeleteATestThing(string id, HttpClient client)
		//		{
		//			var deleteResponse = await client.DeleteAsync($"{_url}{GetThingIdQueryString(id)}");
		//			var result = deleteResponse.StatusCode;
		//			Assert.Equal(System.Net.HttpStatusCode.OK, result);

		//			return result;
		//		}

		//		private string GetThingIdQueryString(string thingId)
		//		{
		//			return $"?c={cfqdn}&u={thingId.Split(new char[] { '/' })[1]}";
		//		}
	}
}
