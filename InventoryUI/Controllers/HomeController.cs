using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Client;
using InventoryUI.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using System.Security.Claims;

namespace InventoryUI.Controllers
{
	public class HomeController : Controller
	{
		AzureAdB2COptions AzureAdB2COptions;
		public IActionResult Index()
		{
			return View();
		}

		[Authorize]
		public IActionResult About()
		{
			ViewData["Message"] = "Your information.";
			var thingIdClaimn = User.Claims.FirstOrDefault(c => c.Type == "extension_ThingId");
			ViewData["ThingId"] = thingIdClaimn?.Value;
			return View();
		}

		public IActionResult Contact()
		{
			ViewData["Message"] = "Your contact page.";

			return View();
		}

		[Authorize]
		public async Task<IActionResult> Api()
		{
			string responseString = "";
			try
			{
				// Retrieve the token with the specified scopes
				var scope = AzureAdB2COptions.ApiScopes.Split(' ');
				string signedInUserID = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
				TokenCache userTokenCache = new MSALSessionCache(signedInUserID, this.HttpContext).GetMsalCacheInstance();
				ConfidentialClientApplication cca = new ConfidentialClientApplication(AzureAdB2COptions.ClientId, AzureAdB2COptions.Authority, AzureAdB2COptions.RedirectUri, new ClientCredential(AzureAdB2COptions.ClientSecret), userTokenCache, null);

				AuthenticationResult result = await cca.AcquireTokenSilentAsync(scope, cca.Users.FirstOrDefault(), AzureAdB2COptions.Authority, false);

				HttpClient client = new HttpClient();
				HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, AzureAdB2COptions.ApiUrl);

				// Add token to the Authorization header and make the request
				request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);
				HttpResponseMessage response = await client.SendAsync(request);

				// Handle the response
				switch (response.StatusCode)
				{
					case HttpStatusCode.OK:
						responseString = await response.Content.ReadAsStringAsync();
						break;
					case HttpStatusCode.Unauthorized:
						responseString = $"Please sign in again. {response.ReasonPhrase}";
						break;
					default:
						responseString = $"Error calling API. StatusCode=${response.StatusCode}";
						break;
				}
			}
			catch (MsalUiRequiredException ex)
			{
				responseString = $"Session has expired. Please sign in again. {ex.Message}";
			}
			catch (Exception ex)
			{
				responseString = $"Error calling API: {ex.Message}";
			}

			ViewData["Payload"] = $"{responseString}";
			return View();
		}

		public IActionResult Error()
		{
			return View();
		}
	}
}
