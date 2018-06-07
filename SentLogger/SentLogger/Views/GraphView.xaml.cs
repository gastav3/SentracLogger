﻿using System;
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

            graphViewModel = new GraphViewModel();
            this.BindingContext = graphViewModel;

            // EVENTS??
            this.SizeChanged += graphViewModel.UpdateUiElement;
            SliderZoom.ValueChanged += KeepScrollPosistion;
           // SliderZoom.ValueChanged += graphViewModel.UpdateUiElement;
            AcceptedLineValueEntry.TextChanged += graphViewModel.UpdateAcceptedValueLine;
      //      AcceptedLineValueEntry.TextChanged += graphViewModel.UpdateUiElement;
            graphViewModel.GetGraphDotsList().CollectionChanged += DrawChangedDots;
        }

        // -----------------------DRAW--------------------------------

        /// <summary>
        /// Add or remove the dots when the GetGraphDotsList() is changed
        /// </summary>
        public void DrawChangedDots(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (GraphDot dot in e.NewItems)
                {
                    DrawNewDot(dot);
                    // graphViewModel.HelloVar = dot.Positon.X.ToString();
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
        /// View for the GraphTab.
        /// </summary>
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
            }
      );
            GraphDrawArea.Children.Add(dot.GraphicDot);
        }


        /// <summary>
        /// View for the GraphTab.
        /// </summary>
        private void KeepScrollPosistion(object sender, EventArgs e) // maybe remove
        {
            double graphScrollXRatio = GraphScroller.ScrollX / GraphScroller.Width;
            double graphScrollYRatio = GraphScroller.ScrollY / GraphScroller.Height;

            Point graphScrollRatio = new Point(graphScrollXRatio, graphScrollYRatio);

            GraphScroller.ScrollToAsync(GraphScroller.Width * graphScrollRatio.X, GraphScroller.Height * graphScrollRatio.Y, false);
        }
    }
}