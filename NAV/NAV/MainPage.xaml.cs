using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

using System.Xml;
using Xamarin.Essentials;
using System.Diagnostics;

namespace NAV
{
	public partial class MainPage : ContentPage
	{

		public MainPage()
		{
			InitializeComponent();
			DisplayXML(CollectFeed());


		}

		private XmlDocument CollectFeed(string rssfeed = @"https://tjenester.nav.no/stillinger/rss?p=0&rpp=5&rv=al&sort=akt")
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

		private void DisplayXML(XmlDocument input)
		{
			OutputLabel.Text = input.InnerXml;
		}
	}
}
