using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SentLogger.ViewModels;
using SentLogger.Models;
using System.Collections.Specialized;
using System.Diagnostics;
using SentLogger.Models.Extra;
using static SentLogger.Resources.ImageResourceExtension;

namespace SentLogger.Views
{
    /// <summary>
    /// View for the GraphTab.
    /// </summary>
    public partial class GraphView : ContentPage
    {
        private GraphViewModel graphViewModel; // the viewmodel class

        public GraphView()
        {
            InitializeComponent();

            graphViewModel = StaticValues.graphViewModel;
            graphViewModel.SwitchToThisView();

            this.BindingContext = graphViewModel;

            this.SizeChanged += graphViewModel.UpdateUiElement; // when the window size is changed, move the dots/values to their correct locations
            SliderZoom.ValueChanged += KeepScrollPosistion;
            AcceptedLineValueEntry.TextChanged += graphViewModel.UpdateAcceptedValueLine;
            graphViewModel.GetGraphDotsList().CollectionChanged += DrawChangedDots; // Called when a new dot/value is added to the graph list
            graphViewModel.graphYValues.CollectionChanged += DrawYValueListChanged;
            DrawExistingDots();
        }

        // -----------------------DRAW--------------------------------


        /// <summary>
        /// Draw dots already in the StaticValues.dotlist
        /// </summary>
        private void DrawExistingDots()
        {
            foreach (GraphDot dot in graphViewModel.GetGraphDotsList())
            {
                if (dot.GraphicDot != null)
                {
                    DrawNewDot(dot);
                }
            }
        }

        /// <summary>
        /// Add or remove the dots when the GetGraphDotsList() is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void DrawChangedDots(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (GraphDot dot in e.NewItems)
                {
                    DrawNewDot(dot);
                }
            }
            if (e.OldItems != null)
            {
                foreach (GraphDot dot in e.OldItems)
                {
                    if (dot.GraphicDot != null)
                    {
                        GraphDrawArea.Children.Remove(dot.GraphicDot);
                    }
                }
            }
        }

        /// <summary>
        /// Draw a new dot to the ui graph and add a TapGesture to make the dots/values clickable.
        /// </summary>
        /// <param name="dot"></param>
        public void DrawNewDot(GraphDot dot)
        {
            // Add the click funconality to the dots
            dot.GraphicDot.GestureRecognizers.Add(
            new TapGestureRecognizer()
            {
                Command = new Command(() => {

                    graphViewModel.SelectDot(dot.Index, graphViewModel.dotSelected);
                    graphViewModel.dotSelected = dot.Index;
                })
            });
            GraphDrawArea.Children.Add(dot.GraphicDot);
        }

        /// <summary>
        /// Update the y values when the list is containing the labels are changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void DrawYValueListChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (Label label in e.NewItems)
                {
                    if (label != null) {
                          GraphDrawArea.Children.Add(label);
                    }
                }
            }
            if (e.OldItems != null)
            {
                foreach (Label label in e.OldItems)
                {
                    if (label != null)
                    {
                     //   GraphDrawArea.Children.Remove(label);
                    }
                }
            }
        }

        /// <summary>
        /// Keep the scroll positon event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KeepScrollPosistion(object sender, EventArgs e) // maybe remove
        {
            double graphScrollXRatio = GraphScroller.ScrollX / GraphScroller.Width;
            double graphScrollYRatio = GraphScroller.ScrollY / GraphScroller.Height;

            Point graphScrollRatio = new Point(graphScrollXRatio, graphScrollYRatio);

            GraphScroller.ScrollToAsync(GraphScroller.Width * graphScrollRatio.X, GraphScroller.Height * graphScrollRatio.Y, false);
        }

        /// <summary>
        /// Play stop the streaming to the graph list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayStopButton_Clicked(object sender, EventArgs e)
        {
            var PlayButtonImage = this.FindByName<Image>("PlayButtonImage");
            var StopButtonImage = this.FindByName<Image>("StopButtonImage");

            if (PlayButtonImage.IsVisible == true)
            {
                PlayButtonImage.IsVisible = false;
                StopButtonImage.IsVisible = true;
            }
            else
            {
                PlayButtonImage.IsVisible = true;
                StopButtonImage.IsVisible = false;
            } 
        }
    }
}