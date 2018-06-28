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
using System.Collections.ObjectModel;
using Rg.Plugins.Popup.Services;

namespace SentLogger.Views
{
    /// <summary>
    /// File Explorer Code Behind
    /// </summary>
    public partial class FileExplorerView : ContentPage
    {
        private ObservableCollection<DataDotObject> TextDataList = new ObservableCollection<DataDotObject>();
        private FileExplorerViewModel fileExplorerViewModel;
        private SentracDataAccess sentracDataAccess;

        public FileExplorerView()
        {
            InitializeComponent();
            fileExplorerViewModel = new FileExplorerViewModel();
            sentracDataAccess = new SentracDataAccess();
        }

        /// <summary>
        /// On Clicked for Save and Load Buttons
        /// </summary>
        private async void LoadButton_Clicked(object sender, EventArgs e)
        {
            if (FormatPicker.SelectedIndex == 0)
            {
                try { 
                    StaticValues.dotList.Clear();
                    StaticValues.dotList.AddRange(await DependencyService.Get<ICsv>().LoadFile());
                    SentracDataView.ItemsSource = TextDataList;
                    await LoadDataTextElements();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
            else if (FormatPicker.SelectedIndex == 1)
            {
                    await PopupNavigation.Instance.PushAsync(new LoadSQLitePopup());
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
                    sentracDataAccess.SaveAllSentracData();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }

        /// <summary>
        /// Loads the data from saved data to the display for viewing purposes
        /// </summary>
        private async Task<int> LoadDataTextElements()
        {
            int i = 0;
            foreach (DataDotObject dot in StaticValues.dotList)
            {
                if (dot != null) {
                    TextDataList.Add(dot);
                    i++;

                    await Task.Delay(25);
                }
            }
            return i;
        }
    }
}