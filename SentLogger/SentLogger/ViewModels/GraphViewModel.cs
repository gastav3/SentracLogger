using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using SentLogger.Views;
using Xamarin.Forms;
using SentLogger.Models;
using SentLogger.Misc;

namespace SentLogger.ViewModels
{
    /// <summary>
    /// The GraphViewModel controls most of the funconality of the graph
    /// -TODO-
    /// Stay pos where zoomed in
    /// Make graph longer when more dots are added
    /// X Axis
    /// Y Axis
    /// X Axis acceptable value
    /// Dots should be circles
    /// 
    /// </summary>
   public class GraphViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<GraphDot> graphDots = new ObservableCollection<GraphDot>();

        private string _helloVar;
        private float zoomAmount = 0f; // the amount that should be zoomed in

        private double graphFrameSizeWidth; // Binded to the graph width
        private double graphFrameSizeHeight; // Binded to the graph height

        //----------------UI-------------------

        /// <summary>
        /// Called when the graph and its elements needs to be updated
        /// </summary>
        public void UpdateUiElement(object sender, EventArgs e)
        {
            foreach (GraphDot dot in GetGraphDotsList()) // loop through all dots
            {
                double newPosX = (dot.StartPoint.X * (GraphFrameSizeWidth / dot.ScreenSizeCreated.X));
                double newPosY = (dot.StartPoint.Y * (GraphFrameSizeHeight / dot.ScreenSizeCreated.Y));

                dot.Positon = new Point(newPosX, newPosY);
                AbsoluteLayout.SetLayoutBounds(dot.GraphicDot, new Rectangle(dot.Positon.X, dot.Positon.Y, dot.Size.X*(1 + ZoomAmount/100f), dot.Size.Y*(1 + ZoomAmount/100f)));
            }

            GraphFrameSizeWidth = (Application.Current.MainPage.Width/2f) * (1 + (ZoomAmount/100f));
            GraphFrameSizeHeight = (Application.Current.MainPage.Height/2f) * (1 + (ZoomAmount/100f));
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
                        UpdateUiElement(this, EventArgs.Empty);
                    }
                });
            }
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
