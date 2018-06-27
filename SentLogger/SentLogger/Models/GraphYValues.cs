using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SentLogger.Models
{
   public class GraphYValues
    {
        public Point StartPoint { get; set; }
        public Label LabelObject { get; set; }
        public BoxView Line { get; set; }

        public double Value { get; set; }

        public GraphYValues(Point startPoint, Label label, double val, BoxView line)
        {
            this.StartPoint = startPoint;
            this.LabelObject = label;
            this.Value = val;
            this.Line = line;
        }
    }
}
