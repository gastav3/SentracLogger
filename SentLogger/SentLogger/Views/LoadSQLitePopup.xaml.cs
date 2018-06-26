using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LocalDataAccess;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SentLogger.Views
{
  /// <summary>
  /// Code behind for Popup that appears when load button is clicked in FileExplorerView when format is set to SQLite
  /// </summary>
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class LoadSQLitePopup : PopupPage
  {
    private SentracDataAccess sentracDataAccess;

    public LoadSQLitePopup()
    {
      InitializeComponent();
      sentracDataAccess = new SentracDataAccess();
    }

    /// <summary>
    /// Loadbutton in the popup that shows up when loadbutton in FileExplorer is clicked while set in SQLite format
    /// </summary>
    /// <param name="date">In parameter from popup for loadbutton in FileExplorer to be able to locate data to load</param>
    private void LoadSQLiteDataButton_Clicked(DateTime date)
    {
      try
      {
        if (DateTime.TryParse(DateParameter.Text, out DateTime dateValue))
        {
          sentracDataAccess.GetFilteredSentracData(dateValue);
        }
        
      }
      catch (Exception ex)
      {
        Debug.WriteLine(ex.Message);
      }
    }

    /// <summary>
    /// Cancel button in the popup that shows up when loadbutton in FileExplorer is clicked while set in SQLite format
    /// </summary>
    private void CloseLoadSQLitePopupButton_Clicked(object sender, EventArgs e)
    {
      PopupNavigation.Instance.PopAsync(true);
    }
  }
}