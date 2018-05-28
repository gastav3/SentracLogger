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
using SkiaSharp;
using SkiaSharp.Views.Forms;

namespace SentLogger.Views
{
    /// <summary>
    /// View for the GraphTab.
    /// </summary>
    public partial class GraphView : ContentPage
    {
        private GraphViewModel graphViewModel; // the viewmodel class
        private Label dotToolTip;

        public GraphView()
        {
            InitializeComponent();

            graphViewModel = new GraphViewModel();
            this.BindingContext = graphViewModel;

            //   SetScrollPositonRatio();

            // EVENTS??
            this.SizeChanged += graphViewModel.UpdateUiElement;
            SliderZoom.ValueChanged += KeepScrollPosistion;
            SliderZoom.ValueChanged += graphViewModel.UpdateUiElement;
            AcceptedLineValueEntry.TextChanged += graphViewModel.UpdateAcceptedValueLine;
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

                    if (dotToolTip != null)
                    {
                        GraphDrawArea.Children.Remove(dotToolTip);
                    }

                    dotToolTip = new Label { Text = dot.Positon.Y.ToString(), LineBreakMode = LineBreakMode.WordWrap };
                    // dotToolTip.BackgroundColor = Color.GhostWhite;
                    AbsoluteLayout.SetLayoutBounds(dotToolTip, new Rectangle(dot.Positon.X, dot.Positon.Y, 100, 20));
                    AbsoluteLayout.SetLayoutFlags(dotToolTip, AbsoluteLayoutFlags.None);

                    GraphDrawArea.Children.Add(dotToolTip);
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