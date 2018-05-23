using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace SentLogger
{
	public partial class App : Application
	{
		public App ()
		{
			InitializeComponent();

            MainPage = new NavigationPage(new SentLogger.Views.GraphView());
        }

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}

    /// <summary>
    /// Buttons handling navigation.
    /// </summary>
    /*
    private void GoToSettingsButton_Pressed(object sender, EventArgs e)
    {
      (sender as Button).Text = "You pressed me!";
    }
    */

    private void GoToSettingsButton_Clicked(object sender, EventArgs e)
    {
      MainPage = new Views.SettingsView();
    }

    private void GoToExplorerSaveButton_Clicked(object sender, EventArgs e)
    {
      MainPage = new Views.FileExplorerView();
    }

    private void GoToHelpButton_Clicked(object sender, EventArgs e)
    {
      MainPage = new Views.HelpView();
    }

    private void GoToExplorerLoadButton_Clicked(object sender, EventArgs e)
    {
      MainPage = new Views.FileExplorerView();
    }
  }
}
