﻿using System;
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

		public ThingTest(ITestOutputHelper output) : base(output){ }

		[Fact]
		public async void CreateLocalThing_ShouldBeOK()
		{
			var jsonContent = new JsonContent(new CreateLocalThingRequest {
				Session = "00000000-0000-0000-0000-000000000001",
				ThingId = $"{_cfqdn}/M100",
				Role="Omnipotent",
				NewThingId =  $"{_cfqdn}/Test@{DateTime.Now.ToString()}",
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
					$"{_cfqdn}/M100",
					$"{_cfqdn}/AnonymousUser",
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
				ThingId = $"{_cfqdn}/M100",
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
				ThingId = $"{_cfqdn}/T1",
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

		[Fact]
		public async void GetAttributes_T1_ShouldReturn()
		{
			var getAttributeRequest = new GetAttributesRequest
			{
				Session = "00000000-0000-0000-0000-000000000001",
				ThingId = $"{_cfqdn}/T1",
				Role = "Owner",
				Attributes = new List<string>
				{
					"Title",
					"Modified",
				}
			};
			var jsonContent = new JsonContent(getAttributeRequest);
			var response = await _client.PostAsync($"{_url}/GetAttributes", jsonContent);
			response.EnsureSuccessStatusCode();

			var result = await response.Content.ReadAsJsonAsync<GetAttributesResponse>();

			Assert.NotNull(result);
			Assert.NotNull(result.AttributeValues);
			Assert.True(result.AttributeValues.Count() == getAttributeRequest.Attributes.Count());
			Assert.True(result.AttributeValues.Any(av=>av.Attribute == "Title" && av.IsOk==true));

		}

		[Fact]
		public async void SetAttributes_NewThing_ShouldReturn()
		{
			string thingId = await CreateATestThing();
			var setAttributeRequest = new SetAttributesRequest
			{
				Session = "00000000-0000-0000-0000-000000000001",
				ThingId = thingId,
				Role = "Owner",
				AttributeValues = new List<SetAttributeValue>
				{
					new SetAttributeValue
					{
						Attribute="Title",
						Value = "Moikka vaan",
					},
					new SetAttributeValue
					{
						Attribute="IsGpsPublic",
						Value = true,
					},
					new SetAttributeValue
					{
						Attribute="Location_Timestamp",
						Value = DateTime.UtcNow.ToString("u"),
					},
				}
			};
			var jsonContent = new JsonContent(setAttributeRequest);
			var response = await _client.PostAsync($"{_url}/SetAttributes", jsonContent);
			response.EnsureSuccessStatusCode();

			var result = await response.Content.ReadAsJsonAsync<SetAttributesResponse>();

			Assert.NotNull(result);
			Assert.NotNull(result.AttributeValues);
			Assert.True(result.AttributeValues.Count() == setAttributeRequest.AttributeValues.Count());
			Assert.True(result.AttributeValues.Any(av => av.Attribute == "Title" && av.IsOk == true));

		}
		[Fact]
		public async void SetExtension_AndReadValue()
		{
			string thingId = await CreateATestThing();
			var setAttributeRequest = new SetAttributesRequest
			{
				Session = "00000000-0000-0000-0000-000000000001",
				ThingId = thingId,
				Role = "Owner",
				AttributeValues = new List<SetAttributeValue>
				{
					new SetAttributeValue
					{
						Attribute=$"{_cfqdn}/TestExtension{Guid.NewGuid()}",
						Value = "Testing data",
					},
					
				}
			};
			var jsonContent = new JsonContent(setAttributeRequest);
			var response = await _client.PostAsync($"{_url}/SetAttributes", jsonContent);
			response.EnsureSuccessStatusCode();

			var result = await response.Content.ReadAsJsonAsync<SetAttributesResponse>();

			Assert.NotNull(result);
			Assert.NotNull(result.AttributeValues);
			Assert.True(result.AttributeValues.Count() == setAttributeRequest.AttributeValues.Count());
			Assert.True(result.AttributeValues.Any(av => av.Attribute == setAttributeRequest.AttributeValues[0].Attribute && av.IsOk == true));

			//read the value
			var getAttributeRequest = new GetAttributesRequest
			{
				Session = "00000000-0000-0000-0000-000000000001",
				ThingId = thingId,
				Role = "Owner",
				Attributes = new List<string>
				{
					setAttributeRequest.AttributeValues[0].Attribute,
				}
			};
			var jsonContent2 = new JsonContent(getAttributeRequest);
			response = await _client.PostAsync($"{_url}/GetAttributes", jsonContent2);
			response.EnsureSuccessStatusCode();

			var result2 = await response.Content.ReadAsJsonAsync<GetAttributesResponse>();

			Assert.NotNull(result2);
			Assert.NotNull(result2.AttributeValues);
			Assert.True(result2.AttributeValues.Count() == getAttributeRequest.Attributes.Count());
			Assert.True(result2.AttributeValues.Any(av => av.Attribute == setAttributeRequest.AttributeValues[0].Attribute && av.IsOk == true));
			Assert.True(string.Compare((string)result2.AttributeValues[0].Value,(string)setAttributeRequest.AttributeValues[0].Value)==0 );

		}

	}
}
