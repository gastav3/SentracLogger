using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SentLogger.Models
{
    public class GraphDot
    {
        public Point StartPoint { get; set; }
        public Point Positon { get; set; }
        public Point ScreenSizeCreated { get; set; }

        public Point Size { get; set; }
        public BoxView GraphicDot { get; set; }

        public double Value { get; set; }

        public GraphDot(Point point)
        {
            Positon = point;
            StartPoint = point;
        }
    }
}
