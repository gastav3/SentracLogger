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

        private void Browse_Button_Clicked(object sender, EventArgs e)
        {

        }

        private async void LoadButton_Clicked(object sender, EventArgs e)
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
        
        private void SaveButton_Clicked(object sender, EventArgs e)
        {
            switch (FormatPicker.SelectedIndex)
            {
                case 0:
                {
                    try
                    {
                        DependencyService.Get<ICsv>().SaveFile();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                    break;
                }
                case 1:
                {
                    sentracDataAccess.SaveSentracSQLiteData
                    break;
                }
            }
        }
    }
}