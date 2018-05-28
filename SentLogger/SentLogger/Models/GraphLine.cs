using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SentLogger.Models
{
   public class GraphLine
    {
        public Point Position { get; set; }
        public Point Size { get; set; }

        public BoxView GraphicLine { get; set; }

        public GraphLine(Point pos, Point size)
        {
            this.Position = pos;
            this.Size = size;
            this.GraphicLine = new BoxView { Color = Color.Red };
        }
    }
}
