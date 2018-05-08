using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SentLogger.Resources;
using Xamarin.Forms;

namespace SentLogger
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
		    AppResource.Culture = new CultureInfo("sv"); // Change Language
		    InitializeComponent(); // Run this again after chaning language

            // label.Text = AppResource.Title;
        }
	}
}
