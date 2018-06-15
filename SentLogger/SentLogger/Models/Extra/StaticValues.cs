using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using SentLogger.ViewModels;

namespace SentLogger.Models.Extra
{
   public static class StaticValues
    {
        public static List<DataDotObject> dotList = new List<DataDotObject>();
        public static ObservableCollection<GraphDot> graphDots = new ObservableCollection<GraphDot>();

        public static string SelectedPort { get; set; }
        public static GraphViewModel graphViewModel = new GraphViewModel();

    }
}
