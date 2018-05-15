using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using SentLogger.Views;
using Xamarin.Forms;
using SentLogger.Models;

namespace SentLogger.ViewModels
{
    class GraphViewModel : INotifyPropertyChanged
    {
        private string _helloVar;
        private ObservableCollection<GraphDot> graphDots = new ObservableCollection<GraphDot>();

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

        public ObservableCollection<GraphDot> GetGraphDotsList()
        {
            return this.graphDots;
        }

        public ObservableCollection<GraphDot> GetGraphDots()
        {
            return this.graphDots;
        }

        public void AddGraphDot(GraphDot dot)
        {
            this.graphDots.Add(dot);
        }

        //-----------------INIT-------------------------
        public GraphViewModel()
        {
            HelloVar = "VOLVO";
        }

        //-----------COMMANDS FOR BUTTONS-------------

        public Command AddNewRandomDotCommand
        {
            get
            {
                return new Command(() =>
                {
                    Random rnd = new Random();
                    GraphDot tempDot = new GraphDot(new Point(rnd.Next(1,100), rnd.Next(1,25)));
                    AddGraphDot(tempDot);
                });
            }
        }

        public Command RemoveLastGraphDotCommand
        {
            get
            {
                return new Command(() =>
                {
                    if (graphDots.Count > 0)
                    {
                        this.graphDots.RemoveAt(graphDots.Count - 1);
                        HelloVar = graphDots.Count.ToString() + " St";
                    }
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
