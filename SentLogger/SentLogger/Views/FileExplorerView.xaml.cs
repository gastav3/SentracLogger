using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SentLogger.ViewModels;
using Plugin.FilePicker;
using System.Diagnostics;
using SentLogger.Resources;
using SentLogger.Models.Extra;
using SentLogger.Models;
using LocalDataAccess;

namespace SentLogger.Views
{
    /// <summary>
    /// View for the File Explorer.
    /// </summary>
    public partial class FileExplorerView : ContentPage
    {
        private FileExplorerViewModel fileExplorerViewModel;
        private SentracDataAccess sentracDataAccess;

        public FileExplorerView()
        {
            InitializeComponent();
            fileExplorerViewModel = new FileExplorerViewModel();
            sentracDataAccess = new SentracDataAccess();
        }

        private async void LoadButton_Clicked(object sender, EventArgs e)
        {
            if (FormatPicker.SelectedIndex == 0)
            {
                try
                {
                    StaticValues.dotList.AddRange(await DependencyService.Get<ICsv>().LoadFile());
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
            else if (FormatPicker.SelectedIndex == 1)
            {
// TODO - Make a actionsheet for choosing date to load from (or fix browser plus entry)....change from DateTime.Now
// to binding from action sheet.
                try
                {
                    sentracDataAccess.GetFilteredSentracData(DateTime.Now);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }
        
        private void SaveButton_Clicked(object sender, EventArgs e)
        {
            if (FormatPicker.SelectedIndex == 0)
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
            else if (FormatPicker.SelectedIndex == 1)
            {
                try
                {
                    sentracDataAccess.AddNewSentracSQLiteData();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }
    }
}