using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using SentLogger.Views;
using Xamarin.Forms;
using SentLogger.Models;
using SentLogger.Resources;
using System.Diagnostics;

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

        private double dotIntervalX = 15.0; // The amount of space between every dot

        private string _helloVar;
        private float zoomAmount = 0f; // the amount that should be zoomed in

        private double graphFrameSizeWidth; // Binded to the graph width
        private double graphFrameSizeHeight; // Binded to the graph height

        private double minimumGraphFrameSizeWidth; // Binded to the minimum graph width
        private double minimumGraphFrameSizeHeight; // Binded to the minimum graph height

        private double graphFrameSizeOffsetX; // How much should the graph width increase
        private double graphFrameSizeOffsetY; // How much should the graph height increase

        private double windowStartSizeX = 0.0; // Gets the start window size X when the first dot is created
        private double windowStartSizeY = 0.0; // Gets the start window size Y when the first dot is created

        //----------------UI-------------------
        /// <summary>
        /// Called by events when the graph and its elements needs to be updated
        /// </summary>
        public void UpdateUiElement(object sender, EventArgs e)
        {
            DoUpdateUIElements();
        }

        /// <summary>
        /// Called when the graph and its elements needs to be updated
        /// </summary>
        private void DoUpdateUIElements()
        {
            GraphFrameSizeWidth = (Application.Current.MainPage.Width + graphFrameSizeOffsetX) * GetZoomAmount();
            GraphFrameSizeHeight = ((Application.Current.MainPage.Height / 2f) + graphFrameSizeOffsetY) * GetZoomAmount();

            foreach (GraphDot dot in GetGraphDotsList()) // loop through all dots
            {
                double newPosX = (dot.StartPoint.X * (((GraphFrameSizeWidth - graphFrameSizeOffsetX) / windowStartSizeX) * GetZoomAmount()));
                double newPosY = (dot.StartPoint.Y * (((GraphFrameSizeHeight - graphFrameSizeOffsetY) / windowStartSizeY) * GetZoomAmount()));

                AbsoluteLayout.SetLayoutBounds(dot.GraphicDot, new Rectangle(newPosX, newPosY, dot.Size.X * GetZoomAmount(), dot.Size.Y * GetZoomAmount()));
            }
        }

        /// <summary>
        /// Adds a new dot to the dot list, which is an ObservableCollection that will trigger DrawChangedDots in GraphView
        /// </summary>
        public void AddNewDotToGraphList(Point pos)
        {

            if (windowStartSizeX == 0.0 || windowStartSizeY == 0.0)
            {
                windowStartSizeX = (Application.Current.MainPage.Width + graphFrameSizeOffsetX) * GetZoomAmount();
                windowStartSizeY = ((Application.Current.MainPage.Height / 2f) + graphFrameSizeOffsetY) * GetZoomAmount();
            }

            GraphFrameSizeWidth = (Application.Current.MainPage.Width + graphFrameSizeOffsetX) * GetZoomAmount();
            GraphFrameSizeHeight = ((Application.Current.MainPage.Height / 2f) + graphFrameSizeOffsetY) * GetZoomAmount();

            GraphDot tempDot = new GraphDot(new Point(pos.X, pos.Y));
            tempDot.ScreenSizeCreated = new Point(GraphFrameSizeWidth, GraphFrameSizeHeight);

            double newPosX = (tempDot.StartPoint.X * (((GraphFrameSizeWidth - graphFrameSizeOffsetX) / windowStartSizeX) * GetZoomAmount()));
            double newPosY = (tempDot.StartPoint.Y * (((GraphFrameSizeHeight - graphFrameSizeOffsetY) / windowStartSizeY) * GetZoomAmount()));

            AbsoluteLayout.SetLayoutBounds(tempDot.GraphicDot, new Rectangle(newPosX, newPosY, tempDot.Size.X * GetZoomAmount(), tempDot.Size.Y * GetZoomAmount()));
            AbsoluteLayout.SetLayoutFlags(tempDot.GraphicDot, AbsoluteLayoutFlags.None);

            this.graphDots.Add(tempDot);
        }


        /// <summary>
        /// Extend the length of the graph draw area
        /// </summary>
        public void ExtendGraphLength()
        {
            graphFrameSizeOffsetX += dotIntervalX * GetZoomAmount();
        }

        //------ PROPERTIES -----

        public double GetDotIntervalX
        {
            get => this.dotIntervalX;
            set
            {
                dotIntervalX = value;
                OnPropertyChanged();
            }
        }
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

        public double MinimumGraphFrameSizeHeight
        {
            get => this.minimumGraphFrameSizeHeight;
            set
            {
                minimumGraphFrameSizeHeight = value;
                OnPropertyChanged();
            }
        }

        public double MinimumGraphFrameSizeWidth
        {
            get => this.minimumGraphFrameSizeWidth;
            set
            {
                minimumGraphFrameSizeWidth = value;
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

        /// <summary>
        /// Get the calculated zoom amount
        /// </summary>
        public double GetZoomAmount()
        {
            return (1f + (ZoomAmount / 100f));
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

        //-----------------INIT-------------------------
        public GraphViewModel()
        {
        }

        //-----------COMMANDS FOR BUTTONS-------------

        public Command AddNewRandomDotCommand
        {
            get
            {
                return new Command(() =>
                {
                    for (int i = 0; i < 10; i++)
                    {
                        Random rnd = new Random();
                        AddNewDotToGraphList(new Point(((1 + GetGraphDotsList().Count) * dotIntervalX), rnd.Next(2)));
                    }
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
