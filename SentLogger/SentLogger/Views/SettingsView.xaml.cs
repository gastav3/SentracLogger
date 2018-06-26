using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SentLogger.ViewModels;
using SentLogger.Resources;
using SentLogger.Models.Extra;
using System.Diagnostics;
using System.Collections.ObjectModel;

namespace SentLogger.Views
{
    /// <summary>
    /// SettingsView Code Behind
    /// </summary>
	  [XamlCompilation(XamlCompilationOptions.Compile)]
	  partial class SettingsView : ContentPage
	  {
        private SettingsViewModel settingsViewModel;

        public SettingsView ()
		    {
			      InitializeComponent ();

            settingsViewModel = new SettingsViewModel();
            this.BindingContext = settingsViewModel;

            portPicker.SelectedIndexChanged += (sender, args) =>
            {
                if (portPicker.SelectedIndex == -1)
                {
                }
                else
                {
                    string name = portPicker.Items[portPicker.SelectedIndex];
                    settingsViewModel.SelectedPort = name;
                }
            };
        }

        private void portPicker_Focused(object sender, FocusEventArgs e)
        {
            foreach (ComPort p in DependencyService.Get<IUsbConnectionSerialPort>().GetPorts())
            {
                if (!CheckListForNameAndId(p.Name, p.Id, settingsViewModel.Ports))
                {
                    settingsViewModel.Ports.Add(p);
                }
            }          
        }

        private void portPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                portPicker.SelectedItem = portPicker.Items[portPicker.SelectedIndex]; //List and string...?
                portPicker.Items.Clear();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private bool CheckListForNameAndId(string name, string id, ObservableCollection<ComPort> ports)
        {
            foreach (ComPort p in ports)
            {
                if (string.Equals(p.Name, name) && string.Equals(p.Id, id))
                {
                    return true;
                }
            }
            return false;
        }

        private void Button_Clicked_Test_Connection(object sender, EventArgs e)
        {

        }
    }
}