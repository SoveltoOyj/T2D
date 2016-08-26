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

		//string serviceUrl = "http://localhost:27122/api/test/";
		string serviceUrl = "http://localhost:5000/api/test/";
		
		HttpClient client;

		public MainPage()
		{
			this.InitializeComponent();
			ModelType.Items.Add("Thing");
			ModelType.SelectedIndex = 0;

			this.InitializeComponent();
			ModelType2.Items.Add("Role");
			ModelType2.SelectedIndex = 0;

			client = new HttpClient();
			client.BaseAddress = new Uri(serviceUrl);

		}

		private async Task<bool> HandleResponse(HttpResponseMessage resp)
		{
			string result = null;
			result = await resp.Content.ReadAsStringAsync();
			if (resp.IsSuccessStatusCode)
			{
				txtInOut.Text = result;
			}
			else
			{
				txtInOut.Text = "Error: " + result;
				return false;
			}
			var obj = Newtonsoft.Json.JsonConvert.DeserializeObject(result);
			gridView.ItemsSource = obj;
			return true;
		}

		#region ThingModel

		private async void bGetAll_Click(object sender, RoutedEventArgs e)
		{
			await HandleResponse(await client.GetAsync(ModelType.SelectedValue.ToString()));
		}

		private async void bGetOne_Click(object sender, RoutedEventArgs e)
		{
			await HandleResponse(await client.GetAsync($"{ModelType.SelectedValue.ToString()}/id/{txtInOut.Text}"));
		}

		private void bGetOneInput_Click(object sender, RoutedEventArgs e)
		{
			txtInOut.Text = "?cu=sovelto.fi/inventory&us=ThingNb2";
		}

		private void bPostInp_Click(object sender, RoutedEventArgs e)
		{
			txtInOut.Text =
@"{
id: {
  creatorUri: ""sovelto.fi/inventory"",
  uniqueString: ""ThingNb jokin muu""
},
height: 124.5,
width: 43
}";
		}

		private async void bPost_Click(object sender, RoutedEventArgs e)
		{
			var content = new StringContent(txtInOut.Text, new System.Text.UTF8Encoding(), "application/json");
			await HandleResponse(await client.PostAsync($"{ModelType.SelectedValue.ToString()}", content));
		}

	
		private void bPatchInp_Click(object sender, RoutedEventArgs e)
		{
			txtInOut.Text =
@"?cu=sovelto.fi/inventory&us=ThingNb2  // tämä on päivitettävän thing:n id, päivitetään vain osa propertyistä

[
    {""op"":""replace"",
	  ""path"":""height"",
	  ""value"": ""123""
		},
	{""op"":""replace"",
	  ""path"":""width"",
	  ""value"": 321 
	}
]";
		}

		private async void bPatch_Click(object sender, RoutedEventArgs e)
		{
			string id = txtInOut.Text.Substring(0, txtInOut.Text.IndexOf("  //"));

			string s = txtInOut.Text.Substring(txtInOut.Text.IndexOf("["));
			var msg = new HttpRequestMessage(new HttpMethod("PATCH"), $"{ModelType.SelectedValue.ToString()}/{id}");
			msg.Content = new StringContent(s, new System.Text.UTF8Encoding(), "application/json");

			await HandleResponse(await client.SendAsync(msg));
		}

		private void bPutInp_Click(object sender, RoutedEventArgs e)
		{
			txtInOut.Text =
			@"?cu=sovelto.fi/inventory&us=ThingNb2  // tämä on päivitettävän thing:n id, päivitetään kaikki propertyt)

{
  ""width"":9988,
  ""height"":776
}";
		}

		private async void bPut_Click(object sender, RoutedEventArgs e)
		{
			string id = txtInOut.Text.Substring(0, txtInOut.Text.IndexOf("  //"));

			string s = txtInOut.Text.Substring(txtInOut.Text.IndexOf("{"));
			var content = new StringContent(s, new System.Text.UTF8Encoding(), "application/json");
			await HandleResponse(await client.PutAsync($"{ModelType.SelectedValue.ToString()}/{id}", content));

		}

		private void bDelInp_Click(object sender, RoutedEventArgs e)
		{
			txtInOut.Text = "?cu=sovelto.fi/inventory&us=ThingNb2";
		}

		private async void bDel_Click(object sender, RoutedEventArgs e)
		{
			await HandleResponse(await client.DeleteAsync($"{ModelType.SelectedValue.ToString()}/{txtInOut.Text}"));

		}
		#endregion

		#region id Model

		private async void bGetAll2_Click(object sender, RoutedEventArgs e)
		{
			await HandleResponse(await client.GetAsync(ModelType2.SelectedValue.ToString()));
		}

		private void bGetOneInput2_Click(object sender, RoutedEventArgs e)
		{
			txtInOut.Text = "2";
		}

		private async void bGetOne2_Click(object sender, RoutedEventArgs e)
		{
			await HandleResponse(await client.GetAsync($"{ModelType2.SelectedValue.ToString()}/{txtInOut.Text}"));
		}

		private void bPostInp2_Click(object sender, RoutedEventArgs e)
		{
			txtInOut.Text = "{name: \"uusi\"}";
		}

		private async void bPost2_Click(object sender, RoutedEventArgs e)
		{
			var content = new StringContent(txtInOut.Text, new System.Text.UTF8Encoding(), "application/json");
			await HandleResponse(await client.PostAsync($"{ModelType2.SelectedValue.ToString()}", content));
		}

		private void bPatchInp2_Click(object sender, RoutedEventArgs e)
		{
			txtInOut.Text =
@"2  // tämä on päivitettävän ID, vain halutut kentät päivitetään
[
    {""op"":""replace"",
	  ""path"":""name"",
	  ""value"": ""uusi arvo""
		}
]";
		}

		private async void bPatch2_Click(object sender, RoutedEventArgs e)
		{
			string id = txtInOut.Text.Substring(0, txtInOut.Text.IndexOf("  //"));

			string s = txtInOut.Text.Substring(txtInOut.Text.IndexOf("["));

			var msg = new HttpRequestMessage(new HttpMethod("PATCH"), $"{ModelType2.SelectedValue.ToString()}/{id}");
			msg.Content = new StringContent(s, new System.Text.UTF8Encoding(), "application/json");

			await HandleResponse(await client.SendAsync(msg));
		}

		private void bPutInp2_Click(object sender, RoutedEventArgs e)
		{
			txtInOut.Text =
@"2  // tämä on päivitettävän modelin id, päivitetään kaikki propertyt)

{
  ""name"":""päivitetty arvo""
}";
		}

		private async void bPut2_Click(object sender, RoutedEventArgs e)
		{
			string id = txtInOut.Text.Substring(0, txtInOut.Text.IndexOf("  //"));

			string s = txtInOut.Text.Substring(txtInOut.Text.IndexOf("{"));
			var content = new StringContent(s, new System.Text.UTF8Encoding(), "application/json");
			await HandleResponse(await client.PutAsync($"{ModelType2.SelectedValue.ToString()}/{id}", content));

		}

		private void bDelInp2_Click(object sender, RoutedEventArgs e)
		{
			txtInOut.Text = "2  // tämä on deletoitavan modelin id";
		}

		private async void bDel2_Click(object sender, RoutedEventArgs e)
		{
			string id = txtInOut.Text.Substring(0, txtInOut.Text.IndexOf("  //"));
			await HandleResponse(await client.DeleteAsync($"{ModelType2.SelectedValue.ToString()}/{id}"));
		}
	}
	#endregion
}
