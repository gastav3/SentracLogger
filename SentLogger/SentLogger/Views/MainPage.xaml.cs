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
    /// <summary>
    /// MainPage for the shared code of the project.
    /// </summary>
    public MainPage()
		{
			InitializeComponent();
         /// <summary>
         /// Changes the language in the app and initializes it again.
        /// </summary>
        // AppResource.Culture = new CultureInfo("sv");
		// InitializeComponent();

		}
    }
}
