using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SentLogger.Models
{
    public class GraphDot
    {
        public int Index { get; set; }
        public double Value { get; set; }

        public Point StartPoint { get; set; }
        public Point Positon { get; set; }
        public Point ScreenSizeCreated { get; set; }

        public BoxView GraphicDot { get; set; }

        public GraphDot(Point point, double val)
        {
            this.Value = val;
            this.StartPoint = point;
            this.Positon = point;

            this.GraphicDot = new BoxView { Color = Color.Black };
        }
    }
}
