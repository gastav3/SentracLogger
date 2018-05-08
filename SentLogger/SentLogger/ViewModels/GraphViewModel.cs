using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace SentLogger.ViewModels
{
    class GraphViewModel : INotifyPropertyChanged
    {
        private string _helloVar;
        private List<string> _testList { get; set; }


        //------ PROPERTIES -----
        public string HelloVar
        {
            get => this._helloVar;
            set
            {
                _helloVar = value;
                OnPropertyChanged();
            }

        }

        public List<string> TestList
        {
            get => _testList;
            set
            {
                _testList = value;
                OnPropertyChanged();
            }
        }

        //-----------------INIT-------------------------
        public GraphViewModel()
        {

        }


        //-----------COMMANDS FOR BUTTONS-------------
        public Command TestButtonCommand
        {
            get
            {
                return new Command(() =>
                {
                    HelloVar = "VOLVO";
                });
            }
        }


        //--------ON PROPERTY CHANGED STUFF-----------
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
