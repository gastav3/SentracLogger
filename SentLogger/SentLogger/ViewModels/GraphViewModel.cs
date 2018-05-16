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
        private ObservableCollection<GraphDot> graphDots = new ObservableCollection<GraphDot>();

        private string _helloVar;
        private float zoomAmount;

        private double graphFrameSizeOffsetX;
        private double graphFrameSizeOffsetY;

        private double graphFrameSizeWidth;
        private double graphFrameSizeHeight;

        //----------------UI-------------------

        public void UpdateUiElement(object sender, EventArgs e)
        {
            foreach (GraphDot dot in GetGraphDotsList())
            {
                double newPosX = (dot.StartPoint.X * (Application.Current.MainPage.Width / dot.ScreenSizeCreated.X));
                double newPosY = (dot.StartPoint.Y * (Application.Current.MainPage.Height) / dot.ScreenSizeCreated.Y);
                dot.Positon = new Point(newPosX, newPosY);
                AbsoluteLayout.SetLayoutBounds(dot.GraphicDot, new Rectangle(dot.Positon.X, dot.Positon.Y, dot.Size.X, dot.Size.Y));
            }

            GraphFrameSizeWidth = Application.Current.MainPage.Width + graphFrameSizeOffsetX;
            GraphFrameSizeHeight = Application.Current.MainPage.Height + graphFrameSizeOffsetY;
        }

        //------ PROPERTIES -----

        public double GraphFrameSizeHeight
        {
            get => this.graphFrameSizeHeight;
            set
            {
                graphFrameSizeHeight = value;
                OnPropertyChanged();
            }
        }

        public double GraphFrameSizeWidth
        {
            get => this.graphFrameSizeWidth;
            set
            {
                graphFrameSizeWidth = value;
                OnPropertyChanged();
            }
        }

        public float ZoomAmount
        {
            get => this.zoomAmount;
            set
            {
                zoomAmount = value;
                OnPropertyChanged();
            }
        }

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
                    GraphDot tempDot = new GraphDot(new Point(rnd.Next(1,1000), rnd.Next(1,600)));
                    tempDot.Size = new Point(10, 10);

                    tempDot.ScreenSizeCreated = new Point(Application.Current.MainPage.Width, Application.Current.MainPage.Height);
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


        public Command GraphZoomInCommand
        {
            get
            {
                return new Command(() =>
                {
                    
                });
            }
        }

        public Command GraphZoomOutCommand
        {
            get
            {
                return new Command(() =>
                {

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
