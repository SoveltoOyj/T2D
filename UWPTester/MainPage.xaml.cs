using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWPTester
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainPage : Page
	{

		string serviceUrl = "http://localhost:27122/api/test/";
		HttpClient client;

		public MainPage()
		{
			this.InitializeComponent();
			ModelType.Items.Add("Thing");
			ModelType.SelectedIndex = 0;

			client = new HttpClient();
			client.BaseAddress = new Uri(serviceUrl);

		}

		private async void bGetAll_Click(object sender, RoutedEventArgs e)
		{
			await HandleResponse(await client.GetAsync(ModelType.SelectedValue.ToString()));
		}

		private async void bGetOne_Click(object sender, RoutedEventArgs e)
		{
			int id;
			if (!int.TryParse(txtInOut.Text, out id)) id = 1;

			await HandleResponse(await client.GetAsync($"{ModelType.SelectedValue.ToString()}/{id}"));
		}

		private void bGetOneInput_Click(object sender, RoutedEventArgs e)
		{
			txtInOut.Text = "3";
		}

		private void bPostInp_Click(object sender, RoutedEventArgs e)
		{
			txtInOut.Text=
@"{
  ""name"":""uusi thing"",
  ""version"":776
}";
		}

		private async void bPost_Click(object sender, RoutedEventArgs e)
		{
			var content = new StringContent(txtInOut.Text, new System.Text.UTF8Encoding(), "application/json");
			await HandleResponse(await client.PostAsync($"{ModelType.SelectedValue.ToString()}", content));
		}

		private async Task<bool> HandleResponse(HttpResponseMessage resp)
		{
			string result = null;
			if (resp.IsSuccessStatusCode)
			{
				result = await resp.Content.ReadAsStringAsync();
				txtInOut.Text = result;
			}
			else
			{
				txtInOut.Text = "error";
				return false;
			}
			var obj = Newtonsoft.Json.JsonConvert.DeserializeObject(result);
			gridView.ItemsSource = obj;
			return true;
		}

		private void bPatchInp_Click(object sender, RoutedEventArgs e)
		{
			txtInOut.Text =
@"3 (tämä on päivitettävän thing:n id, päivitetään vain osa propertyistä)

[
    {""op"":""replace"",
	  ""path"":""name"",
	  ""value"": ""nimi päivitetään""
		},
	{""op"":""replace"",
	  ""path"":""version"",
	  ""value"": 123 
	}
]";
		}

		private async void bPatch_Click(object sender, RoutedEventArgs e)
		{
			int id;
			if (!int.TryParse(txtInOut.Text.Substring(0, txtInOut.Text.IndexOf(" ")), out id))
				id = 1;

			string s = txtInOut.Text.Substring(txtInOut.Text.IndexOf("["));
			var msg = new HttpRequestMessage(new HttpMethod("PATCH"), $"{ModelType.SelectedValue.ToString()}/{id}");
			msg.Content = new StringContent(s, new System.Text.UTF8Encoding(), "application/json");
	
			await HandleResponse(await client.SendAsync (msg));

		}

		private void bPutInp_Click(object sender, RoutedEventArgs e)
		{
			txtInOut.Text =
			@"3 (tämä on päivitettävän thing:n id, päivitetään kaikki propertyt)

{
  ""name"":""uusi name"",
  ""version"":776
}";
		}

		private async void bPut_Click(object sender, RoutedEventArgs e)
		{
			int id;
			if (!int.TryParse(txtInOut.Text.Substring(0, txtInOut.Text.IndexOf(" ")), out id))
				id = 1;

			string s = txtInOut.Text.Substring(txtInOut.Text.IndexOf("{"));
			var content = new StringContent(s, new System.Text.UTF8Encoding(), "application/json");
			await HandleResponse(await client.PutAsync($"{ModelType.SelectedValue.ToString()}/{id}", content));

		}

		private void bDelInp_Click(object sender, RoutedEventArgs e)
		{
			txtInOut.Text = "3";
		}

		private async void bDel_Click(object sender, RoutedEventArgs e)
		{
			int id;
			if (!int.TryParse(txtInOut.Text, out id)) id = 1;

			await HandleResponse(await client.DeleteAsync($"{ModelType.SelectedValue.ToString()}/{id}"));

		}
	}
}
