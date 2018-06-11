using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using SentLogger.Resources;
using Xamarin.Forms;
using System.Diagnostics;
using SentLogger.Models.Extra;
using System.Collections.ObjectModel;

namespace SentLogger.ViewModels
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        private string selectedPort;
        private ObservableCollection<ComPort> ports = new ObservableCollection<ComPort>();

        public SettingsViewModel()
        {
            DependencyService.Get<IUsbConnectionSerialPort>().Start();
        }

        public ObservableCollection<ComPort> Ports
        {
            get => this.ports;
            set
            {
                ports = value;
            }
        }

        public string SelectedPort
        {
            get => this.selectedPort;
            set
            {
                selectedPort = value;

                string id = FindCorrectPortIdByName(value);
                if (id != null && !string.Equals(id,"")) {
                    StaticValues.SelectedPort = id;
                    DependencyService.Get<IUsbConnectionSerialPort>().SetPort(id, true);
                }
                OnPropertyChanged();
            }
        }

        private string FindCorrectPortIdByName(string name)
        {
            foreach(ComPort port in Ports)
            {
                if (string.Equals(port.Name, name))
                {
                    return port.Id;
                }
            }
            return null;
        }



        //--------ON PROPERTY CHANGED STUFF-----------

        /// <summary>
        /// A standard PropertyChangedEventHandler
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
