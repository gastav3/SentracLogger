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

namespace SentLogger.Views
{
  /// <summary>
  /// View for the File Explorer.
  /// </summary>
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class FileExplorerView : ContentPage
  {
        private FileExplorerViewModel fileExplorerViewModel;

        public FileExplorerView()
        {
            InitializeComponent();
            fileExplorerViewModel = new FileExplorerViewModel();
        }

        private async void Browse_Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                fileExplorerViewModel.GetChoosenFile = await CrossFilePicker.Current.PickFile();
                fileExplorerViewModel.GetFileExplorerFiles(fileExplorerViewModel.GetChoosenFile.FilePath);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        /*private void LoadButton_Clicked(object sender, EventArgs e)
        {

        }
        */
        /*
        private void SaveButton_Clicked(object sender, EventArgs e)
        {

        }
        */
        /*
        private void BrowseButton_Clicked(object sender, EventArgs e)
        {

        }
        */
    }
}