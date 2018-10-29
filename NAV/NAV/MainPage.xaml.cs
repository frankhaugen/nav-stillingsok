using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

using System.Xml;
using Xamarin.Essentials;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Reflection;

namespace NAV
{
	public partial class MainPage : ContentPage
	{
		// https://tjenester.nav.no/stillinger/stillinger?rpp=20&rv=al&sort=akt&l1=62876 (last number)

		// troms: https://tjenester.nav.no/stillinger/stillinger?rpp=20&rv=al&sort=akt&l=62876&l1=62887
		// tromso: https://tjenester.nav.no/stillinger/stillinger?rpp=20&rv=al&sort=akt&l=62876&l2=62953

		public string someValue;

		public string myValue
		{
			get
			{
				return someValue;
			}
		}

		public MainPage()
		{
			InitializeComponent();
			//DisplayXML(CollectFeed()); // debugging



			//OutputLabel.Text = GetChannelElement().ToString();
		}

		

		private async Task DownloadFilesAsync()
		{
			//var cacheDir = FileSystem.CacheDirectory;
			//var httpClient = new HttpClient();
			//var feed = string.Empty;

			//var responseString = await httpClient.GetStringAsync(feed);
			string csv;

			using (var stream = await FileSystem.OpenAppPackageFileAsync("sources.csv"))
			{
				using (var reader = new StreamReader(stream))
				{
					var fileContents = await reader.ReadToEndAsync();
					csv = fileContents;
				}
			}

			OutputLabel.Text = csv;

		}

		private XmlDocument CollectFeed(string rssfeed = @"https://tjenester.nav.no/stillinger/rss?p=0&rpp=1&rv=al&sort=akt")
		{
			XmlDocument output = new XmlDocument();

			try
			{
				output.Load(rssfeed);
			}
			catch (Exception e)
			{
				Debug.WriteLine(e.Message);
			}
			
			return output;
		}

		private XmlElement GetChannelElement()
		{
			XmlDocument xmlDocument = CollectFeed();

			return xmlDocument.GetElementById("channel");
		}
		

		private void DisplayXML(XmlDocument input)
		{
			OutputLabel.Text = input.InnerXml;
		}

		private void TestButton_Clicked(object sender, EventArgs e)
		{

			var assembly = IntrospectionExtensions.GetTypeInfo(typeof(App)).Assembly;
			foreach (var res in assembly.GetManifestResourceNames())
			{
				System.Diagnostics.Debug.WriteLine("found resource: " + res);
			}

			//OutputLabel.Text = ChangeText().Result;
			GetText();
			// Task.Run(async () => { await DownloadFilesAsync(); });
		}

		private void GetText()
		{

			try
			{
				var assembly = IntrospectionExtensions.GetTypeInfo(typeof(App)).Assembly;
				Stream stream = assembly.GetManifestResourceStream("NAV.sources.csv");
				string text = "";
				using (var reader = new System.IO.StreamReader(stream))
				{
					text = reader.ReadToEnd();
				}

				OutputLabel.Text = text;
			}
			catch (Exception e)
			{
				OutputLabel.Text = e.Message;
			}
		}

		private async Task<string> ChangeText()
		{
			string output;

			try
			{
				using (var stream = await FileSystem.OpenAppPackageFileAsync("NAV.sources.csv"))
				{
					using (var reader = new StreamReader(stream))
					{
						var fileContents = await reader.ReadToEndAsync();
						output = fileContents.ToString();
					}
				}
			}
			catch (Exception e)
			{
				output = e.Message;
			}

			

			return output;
		}
	}
}
