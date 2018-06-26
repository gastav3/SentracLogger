using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using SentLogger.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SentLogger.Views
{
  /// <summary>
  /// Code behind for the popup for the save button in the upper right corner
  /// </summary>
  [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SPopup : PopupPage
	{
		public SPopup()
		{
			InitializeComponent ();
		}

    /// <summary>
    /// SPopup save button
    /// For now the popup save button saves the current data from the graph in .csv
    /// </summary>
    private void SaveDataFromCornerButton_Clicked()
    {
      try
      {
        DependencyService.Get<ICsv>().SaveFile(); // Saves to .csv
        PopupNavigation.Instance.PopAsync(true); // Closes the Popup
      }
      catch (Exception ex)
      {
        Debug.WriteLine(ex.Message);
      }
    }

    /// <summary>
    /// No button on SPopup, closes the popup
    /// </summary>
    private void CloseSavingPopupButton_Clicked(object sender, EventArgs e)
    {
      PopupNavigation.Instance.PopAsync(true);
    }
  }
}