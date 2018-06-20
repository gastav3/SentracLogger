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

namespace SentLogger.Views
{
    /// <summary>
    /// View for the File Explorer.
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

        private void Browse_Button_Clicked(object sender, EventArgs e)
        {

        }

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
// TODO - Make a actionsheet for choosing date to load from....change from DateTime.Now
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


        private async Task<int> LoadDataTextElements()
        {
            int i = 0;
            foreach (DataDotObject dot in StaticValues.dotList)
            {
                if (dot != null) {
                    TextDataList.Add(dot);
                    i++;

                    await Task.Delay(50);
                }
            }
            return i;
        }


    }
}