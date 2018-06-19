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
using System.Threading;
using System.Threading.Tasks;
using System.IO.Ports;
using Plugin.DeviceInfo;
using SentLogger.Models.Extra;

namespace SentLogger.ViewModels
{
    /// <summary>
    /// The GraphViewModel controls most of the funconality of the graph
    /// </summary>
    public class GraphViewModel : INotifyPropertyChanged
    {

        private Point dotSize = new Point(5.0, 5.0); // the dot size

        public int dotSelected; // used to get value from a speceific dot in the graph
        private double dotIntervalX = 10.0; // The amount of space between every dot

        private string toolTipValue; // The text of the tooltip above the dot
        private Rectangle toolTipPos; // the posistion of the tooltip on the graph

        private float zoomAmount = 0f; // the amount that should be zoomed in

        private double graphFrameSizeWidth; // Binded to the graph width
        private double graphFrameSizeHeight; // Binded to the graph height

        private double minimumGraphFrameSizeWidth; // Binded to the minimum graph width
        private double minimumGraphFrameSizeHeight; // Binded to the minimum graph height

        private double graphFrameSizeOffsetX; // How much should the graph width increase 2140000000
        private double graphFrameSizeOffsetY; // How much should the graph height increase

        private double windowStartSizeX = 0.0; // Gets the start window size X when the first dot is created
        private double windowStartSizeY = 0.0; // Gets the start window size Y when the first dot is created

        private Task<int> updateUITask; // The task that updates the ui
        private double frozenZoomUpdate; // Used when the foreach loop is running so the user dosent change zoom midway through

        private bool hasCreatedDot = false;
        private double maxAcceptedLineValue = 10.0;
        private Rectangle acceptedLineValuePos;

        private int numberOfTests = 0;
        private int numberOfRejects = 0;
        private double leakPrecentage = 0.0;
        private double maxValue = 0.0;


        //----------------UI-------------------
        /// <summary>
        /// Called by events when the graph and its elements needs to be updated
        /// </summary>
        public void UpdateUiElement(object sender, EventArgs e)
        {
            if (!Extras.IsTaskRunning(updateUITask))
            {
              RunUpdateUITask();
            }

            UpdateAcceptedValueLine(this, EventArgs.Empty);
        }

        private async Task RunUpdateUITask()
        {
            frozenZoomUpdate = GetZoomAmount();
            updateUITask = DoUpdateUIElements();
            int result = await updateUITask;

            NumberOfTests = StaticValues.dotList.Count;
            NumberOfRejects = 0;
            LeakPrecentage = 0;

        }

        /// <summary>
        /// Called when the graph and its elements needs to be updated
        /// </summary>
        private async Task<int> DoUpdateUIElements()
        {
            GraphFrameSizeWidth = (Application.Current.MainPage.Width * frozenZoomUpdate) + graphFrameSizeOffsetX;
            GraphFrameSizeHeight = ((Application.Current.MainPage.Height / 2f)) * (frozenZoomUpdate*2f) + graphFrameSizeOffsetY;

            int i = 0;
            foreach (GraphDot dot in GetGraphDotsList()) // loop through all dots
            {
                double newPosX = (dot.StartPoint.X * (((GraphFrameSizeWidth - graphFrameSizeOffsetX) / windowStartSizeX) * frozenZoomUpdate));
                double newPosY = (dot.StartPoint.Y * (((GraphFrameSizeHeight - graphFrameSizeOffsetY) / windowStartSizeY) * frozenZoomUpdate)) + (GraphFrameSizeHeight - ((DotSize.Y*4f) * frozenZoomUpdate));

                ShouldDotChangeColor(dot, MaxAcceptedLineValue);
                AbsoluteLayout.SetLayoutBounds(dot.GraphicDot, new Rectangle(newPosX, newPosY, DotSize.X * frozenZoomUpdate, DotSize.Y * frozenZoomUpdate));

                ExtendGraphLength(newPosX);
                ExtendGraphHeight(newPosY);

                i++;
                await Task.Delay(GetDynamicUpdateDelay()); // a bit of delay so we dont crash
            }
            return i;
        }

        /// <summary>
        /// Adds a new dot to the dot list, which is an ObservableCollection that will trigger DrawChangedDots in GraphView
        /// </summary>
        public void AddNewDotToGraphList(Point pos, double value)
        {
            SetWindowStartSize();

            GraphFrameSizeWidth = (Application.Current.MainPage.Width * GetZoomAmount()) + graphFrameSizeOffsetX;
            GraphFrameSizeHeight = ((Application.Current.MainPage.Height / 2f)) * (GetZoomAmount()*2f) + graphFrameSizeOffsetY;

            GraphDot tempDot = new GraphDot(new Point(pos.X, (pos.Y * 100f)), value);
            tempDot.Index = GetGraphDotsList().Count;

            tempDot.ScreenSizeCreated = new Point(GraphFrameSizeWidth, GraphFrameSizeHeight);

            double newPosX = (tempDot.StartPoint.X * (((GraphFrameSizeWidth - graphFrameSizeOffsetX) / windowStartSizeX) * GetZoomAmount()));
            double newPosY = (tempDot.StartPoint.Y * (((GraphFrameSizeHeight - graphFrameSizeOffsetY) / windowStartSizeY) * GetZoomAmount())) + (GraphFrameSizeHeight - ((DotSize.Y*4f) * GetZoomAmount()));

            AbsoluteLayout.SetLayoutBounds(tempDot.GraphicDot, new Rectangle(newPosX, newPosY, DotSize.X * GetZoomAmount(), DotSize.Y * GetZoomAmount()));
            AbsoluteLayout.SetLayoutFlags(tempDot.GraphicDot, AbsoluteLayoutFlags.None);

            ShouldDotChangeColor(tempDot, MaxAcceptedLineValue);
            GetGraphDotsList().Add(tempDot); // add to the dot list

            if (value > MaxValue)
            {
                MaxValue = value;
            }

            ExtendGraphLength(newPosX);
            ExtendGraphHeight(newPosY);
            hasCreatedDot = true;
        }


        /// <summary>
        /// The method called by the update graph button
        /// Updates where the red line should be on the graph.
        /// </summary>
        public void UpdateAcceptedValueLine(object sender, EventArgs e)
        {
            if (hasCreatedDot)
            {
                double newPosY = ((-MaxAcceptedLineValue*100f) * (((GraphFrameSizeHeight - graphFrameSizeOffsetY) / windowStartSizeY) * GetZoomAmount())) + (GraphFrameSizeHeight - ((DotSize.Y*4f) * GetZoomAmount()));
                AcceptedLineValuePos = new Rectangle(
                    AcceptedLineValuePos.X-1000,
                    newPosY,
                    GraphFrameSizeWidth*100.0,
                    1.0
                    );
            }
        }

        /// <summary>
        /// Extend the length of the graph draw area
        /// </summary>
        public void ExtendGraphLength(double posX)
        {
            if (GraphFrameSizeWidth <= (posX + DotSize.X))
            {
                graphFrameSizeOffsetX = (posX + DotSize.X);
            }
        }

        public void ExtendGraphHeight(double posY)
        {
            if (GraphFrameSizeHeight <= (-posY + DotSize.Y))
            {
                graphFrameSizeOffsetY = (-posY + DotSize.Y);
            }
        }

        //------ PROPERTIES -----

        public Point DotSize
        {
            get => this.dotSize;
            set
            {
                this.dotSize = value;
                OnPropertyChanged();
            }
        }

        public Rectangle AcceptedLineValuePos
        {
            get => this.acceptedLineValuePos;
            set
            {
                this.acceptedLineValuePos = value;
                OnPropertyChanged();
            }
        }

        public double MaxAcceptedLineValue
        {
            get => Extras.Clamp(this.maxAcceptedLineValue, 0.0, GraphFrameSizeHeight);
            set
            {
                this.maxAcceptedLineValue = value;
                OnPropertyChanged();
            }
        }

        public double GetDotIntervalX
        {
            get => this.dotIntervalX;
            set
            {
                this.dotIntervalX = value;
                OnPropertyChanged();
            }
        }

        public double GraphFrameSizeHeight
        {
            get => Extras.Clamp(this.graphFrameSizeHeight, 0.0, 2140000000); // dont go higher than 2140000000
            set
            {
                this.graphFrameSizeHeight = value;
                OnPropertyChanged();
            }
        }

        public double GraphFrameSizeWidth
        {
            get => Extras.Clamp(this.graphFrameSizeWidth, 0.0, 2140000000);
            set
            {
                this.graphFrameSizeWidth = value;
                OnPropertyChanged();
            }
        }

        public double MinimumGraphFrameSizeHeight
        {
            get => this.minimumGraphFrameSizeHeight;
            set
            {
                this.minimumGraphFrameSizeHeight = value;
                OnPropertyChanged();
            }
        }

        public double MinimumGraphFrameSizeWidth
        {
            get => this.minimumGraphFrameSizeWidth;
            set
            {
                this.minimumGraphFrameSizeWidth = value;
                OnPropertyChanged();
            }
        }

        public float ZoomAmount
        {
            get => this.zoomAmount;
            set
            {
                this.zoomAmount = value;
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

        /// <summary>
        /// The value over the selected dot
        /// </summary>
        public string ToolTipValue
        {
            get => this.toolTipValue;
            set
            {
                toolTipValue = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The position of the text over the selected dot
        /// </summary>
        public Rectangle ToolTipPos
        {
            get => this.toolTipPos;
            set
            {
                toolTipPos = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The list that contain all the dots
        /// </summary>
        public ObservableCollection<GraphDot> GetGraphDotsList()
        {
            return StaticValues.graphDots;
        }

        public int NumberOfTests
        {
            get => this.numberOfTests;
            set
            {
                this.numberOfTests = GetGraphDotsList().Count;
                OnPropertyChanged();
            }
        }

        public int NumberOfRejects
        {
            get => this.numberOfRejects;
            set
            {
                int rejected = 0;
                foreach (GraphDot dot in GetGraphDotsList())
                {
                    if (dot.Value > MaxAcceptedLineValue)
                    {
                        rejected += 1;
                    }
                }
                this.numberOfRejects = rejected;
                OnPropertyChanged();
            }
        }

        public double LeakPrecentage
        {
            get => this.leakPrecentage;
            set
            {
                double rejected = 0;
                foreach (GraphDot dot in GetGraphDotsList())
                {
                    if (dot.Value > MaxAcceptedLineValue)
                    {
                        rejected += 1;
                    }
                }
                try
                {
                    this.leakPrecentage = Math.Round((rejected/GetGraphDotsList().Count)*100.0, 3);
                }
                catch(DivideByZeroException ex)
                {
                    this.leakPrecentage = 0.0;
                    Debug.WriteLine(ex.Message);
                }

                OnPropertyChanged();
            }
        }

        public double MaxValue
        {
            get => this.maxValue;
            set
            {
                this.maxValue = value;
                OnPropertyChanged();
            }
        }


        //---------------------MISC-----------------
        /// <summary>
        /// Returns a delay depending on the amount of dots created.
        /// For faster updates when there are few dots.
        /// </summary>
        private int GetDynamicUpdateDelay()
        {
           int delay = Extras.Clamp((GetGraphDotsList().Count / 150), 0, 2);

              return delay;
        }

        /// <summary>
        /// Saves the window size when the first dot is created
        /// </summary>
        private void SetWindowStartSize()
        {
            if (windowStartSizeX == 0.0 || windowStartSizeY == 0.0) // change this maybe
            {
                windowStartSizeX = (Application.Current.MainPage.Width + graphFrameSizeOffsetX) * GetZoomAmount();
                windowStartSizeY = ((Application.Current.MainPage.Height / 2f) + graphFrameSizeOffsetY) * GetZoomAmount();

                UpdateAcceptedValueLine(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Create a new dot reversed the y pos
        /// </summary>
        public void AddNewDot(double y, double value)
        {
            AddNewDotToGraphList(new Point(((1 + GetGraphDotsList().Count) * dotIntervalX), -y), value); // -pos.Y to reverse to fit graph
        }

        /// <summary>
        /// If the dots are below the line make them green
        /// if they are over make them red
        /// </summary>
        private void ShouldDotChangeColor(GraphDot dot, double value)
        {
            if (dot.Value <= value)
            {
                dot.GraphicDot.Color = Color.Green;
            }
            else
            {
                dot.GraphicDot.Color = Color.Red;
            }
        }

        /// <summary>
        /// Selects the dot
        /// </summary>
        public void SelectDot(int newSelected, int oldSelected)
        {
            try
            {
                ShouldDotChangeColor(GetGraphDotsList()[oldSelected], MaxAcceptedLineValue);

                GraphDot tempDot = GetGraphDotsList()[newSelected];
                tempDot.GraphicDot.Color = Color.Blue;

                ToolTipValue = tempDot.Value.ToString();

                ToolTipPos = new Rectangle(
                  tempDot.GraphicDot.X - (tempDot.Value.ToString().Length * 3.5f),
                  tempDot.GraphicDot.Y - (DotSize.Y * 4f),
                  50.0,
                  25.0
                  );
            }
            catch (ArgumentOutOfRangeException e)
            {
                Debug.WriteLine(e);
            }
            catch (ArgumentNullException e)
            {
                Debug.WriteLine(e);
            }
        }

        //-----------UPDATE WHEN SWTCHING TO GRAPH VIEW------
        public void SwitchToThisView()
        {
            GetGraphDotsList().Clear();
            foreach (DataDotObject dot in StaticValues.dotList)
            {
                if (dot != null) {
                    AddNewDot(dot.Value, dot.Value);
                }
            }
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
                        double yValue = rnd.Next(200);
                        AddNewDot(yValue, yValue);
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
                    if (GetGraphDotsList().Count > 0)
                    {
                        GetGraphDotsList().RemoveAt(GetGraphDotsList().Count - 1);
                        UpdateUiElement(this, EventArgs.Empty);
                    }
                });
            }
        }


        public Command UpdateUiCommand
        {
            get
            {
                return new Command(() =>
                {
                    UpdateUiElement(this, EventArgs.Empty);
                });
            }
        }

        public Command NextDotCommand
        {
            get
            {
                return new Command(() =>
                {
                    if ((GetGraphDotsList().Count - 1 ) > dotSelected)
                    {
                        SelectDot(dotSelected + 1, dotSelected);
                        dotSelected += 1;
                    }
                    else
                    {
                        SelectDot(0, dotSelected);
                        dotSelected = 0;
                    }
                });
            }
        }

        public Command PreviousDotCommand
        {
            get
            {
                return new Command(() =>
                {
                    if ((dotSelected - 1) >= 0)
                    {
                        SelectDot(dotSelected - 1, dotSelected);
                        dotSelected -= 1;
                    }
                    else
                    {
                        SelectDot(GetGraphDotsList().Count - 1, dotSelected);
                        dotSelected = GetGraphDotsList().Count - 1;
                    }
                });
            }
        }

        private bool StreamingPlay = false;

        public Command PlayStopButtonCommand
        {
            get
            {
                return new Command(() =>
                {
                    if (StreamingPlay == false)
                    {
                        StreamingPlay = true;
                    }
                    else
                    {
                        StreamingPlay = false;
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
