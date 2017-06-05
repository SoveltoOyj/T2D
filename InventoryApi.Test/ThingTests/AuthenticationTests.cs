using System;
using System.Collections.Generic;
using System.Text;
using T2D.Model.InventoryApi;
using Xunit;
using Xunit.Abstractions;

namespace InventoryApi.Test.ThingTests
{
	public class AuthenticationTests : InventoryApiBase
	{
		private string _url = "api/inventory/Authentication";
		private string cfqdn = "inv1.sovelto.fi";

		public AuthenticationTests(ITestOutputHelper output) : base(output) { }

		[Fact(DisplayName ="Requires JWT", Skip ="Requires Authorization")]
		public async void EnterAuthenticatedSession_whenKnownUserM100()
		{
			var jsonContent = new JsonContent(new AuthenticationRequest
			{
				AuthenticationType = T2D.Model.Enums.AuthenticationType.Facebook,
				ThingId = $"{cfqdn}/M100"
			});
			var response = await _client.PostAsync($"{_url}/EnterAuthenticatedSession", jsonContent);
			var result = await response.Content.ReadAsJsonAsync<AuthenticationResponse>();

			response.EnsureSuccessStatusCode();
			Assert.NotNull(result);
			Assert.False(string.IsNullOrWhiteSpace(result.Session));
		}

		[Fact(DisplayName = "Requires JWT", Skip = "Requires Authorization")]
		public async void EnterAuthenticatedSession_OK_whenNewUser_MOCK()
		{
			var jsonContent = new JsonContent(new AuthenticationRequest
			{
				AuthenticationType = T2D.Model.Enums.AuthenticationType.Facebook,
				ThingId = $"{cfqdn}/newUser{DateTime.Now.ToString()}"
			});
			var response = await _client.PostAsync($"{_url}/EnterAuthenticatedSession", jsonContent);
			var result = await response.Content.ReadAsJsonAsync<AuthenticationResponse>();

			response.EnsureSuccessStatusCode();
			Assert.NotNull(result);
			Assert.False(string.IsNullOrWhiteSpace(result.Session));
		}

		[Fact(DisplayName = "Requires JWT", Skip = "Requires Authorization")]
		public async void EnterAuthenticatedSession_400_whenNewUserIsNotAuthenticatedUser()
		{
			var jsonContent = new JsonContent(new AuthenticationRequest
			{
				AuthenticationType = T2D.Model.Enums.AuthenticationType.Facebook,
				ThingId = $"{cfqdn}/T1"
			});
			var response = await _client.PostAsync($"{_url}/EnterAuthenticatedSession", jsonContent);
			var result = await response.Content.ReadAsStringAsync();

			Assert.True(response.StatusCode==System.Net.HttpStatusCode.BadRequest);
		}

		[Fact]
		public async void EnterAnonymousSession_ShouldBeSuccesfullAlways()
		{
			var response = await _client.PostAsync($"{_url}/EnterAnonymousSession", null);
			var result = await response.Content.ReadAsJsonAsync<AuthenticationResponse>();

			response.EnsureSuccessStatusCode();
			Assert.NotNull(result);
			Assert.False(string.IsNullOrWhiteSpace(result.Session));
		}

	}
}
