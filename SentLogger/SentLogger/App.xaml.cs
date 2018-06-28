using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using SentLogger.Resources;
using Xamarin.Forms;
using System.IO.Ports;
using Rg.Plugins.Popup.Services;

namespace SentLogger
{
  /// <summary>
  /// Common code behind
  /// </summary>
	public partial class App : Application
	{
    public App ()
		{
			InitializeComponent();
            MainPage = new  SentLogger.Views.GraphView();
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
    /// Buttons handling navigation
    /// </summary>
    private void GoToSettingsButton_Clicked(object sender, EventArgs e)
    {
      MainPage = new Views.SettingsView();
    }

    private void GoToHelpButton_Clicked(object sender, EventArgs e)
    {
      MainPage = new Views.HelpView();
    }

    private void GoToExplorerLoadButton_Clicked(object sender, EventArgs e)
    {
      MainPage = new Views.FileExplorerView();
    }

    /// <summary>
    /// Upper right corner save button (saves current data (in the graph) from any screen in .csv format)
    /// </summary>
    private void SaveActionButton_Clicked(object sender, EventArgs e)
    {
            try
            {
                DependencyService.Get<ICsv>().SaveFile();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

    /// <summary>
    /// Time and date on press button
    /// </summary>
    private void CurTimeAndDateButton_Pressed(object sender, EventArgs e)
    {
      (sender as Button).BackgroundColor = Color.FromHex ("5D74A1");
      (sender as Button).Text = " " + " " + " " + " " + " " + DateTime.Now.ToShortTimeString() + "\n" +
        DateTime.Now.ToShortDateString();
    }
    private void CurTimeAndDateButton_Released(object sender, EventArgs e)
    {
      (sender as Button).BackgroundColor = Color.Transparent;
      (sender as Button).Text = null;
    }

    /// <summary>
    /// Tab buttons
    /// </summary>
    private void FileExplorerTabButton_Clicked(object sender, EventArgs e)
    {
            Debug.WriteLine("FileExplorerTabButton_Clicked");
            MainPage = new Views.FileExplorerView();
    }

    private void GraphTabButton_Clicked(object sender, EventArgs e)
    {
            Debug.WriteLine("GraphTabButton_Clicked");
            MainPage = new Views.GraphView();
    }

    private void HistogramTabButton_Clicked(object sender, EventArgs e)
    {
           Debug.WriteLine("HistogramTabButton_Clicked");
           MainPage = new Views.HistogramView();
    }

    /// <summary>
    /// GraphView Button to navigate to tab views starting at GraphTab
    /// </summary>
    private void GoToGraphButton_Clicked(object sender, EventArgs e)
    {
      Debug.WriteLine("GoToGraphButton_Clicked");
      MainPage = new Views.GraphView();
    }
  }
}
