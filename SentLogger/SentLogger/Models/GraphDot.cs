using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SentLogger.Models
{
    public class GraphDot
    {
        public double Value { get; set; }

        public Point StartPoint { get; set; }
        public Point Positon { get; set; }
        public Point ScreenSizeCreated { get; set; }
        public Point Size { get; set; }

        public BoxView GraphicDot { get; set; }
        public Label ValueLabel { get; set; }

        public GraphDot(Point point)
        {
            this.StartPoint = point;
            this.Positon = point;

            this.Size = new Point(10,10);
            this.GraphicDot = new BoxView { Color = Color.Black };
        }
    }
}
