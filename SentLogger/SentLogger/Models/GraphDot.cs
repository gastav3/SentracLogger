using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SentLogger.Models
{
    public class GraphDot
    {
        public Point Positon { get; set; }
        public BoxView GraphicDot { get; set; }

        public double Value { get; set; }

        public GraphDot(Point point)
        {
            Positon = point;
        }
    }
}
