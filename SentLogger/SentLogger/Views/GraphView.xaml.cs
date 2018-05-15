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

namespace SentLogger.Views
{
  /// <summary>
  /// View for the GraphTab.
  /// </summary>
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class GraphView : ContentPage
	{

        private GraphViewModel graphViewModel;

		public GraphView ()
		{
			InitializeComponent();
            graphViewModel = new GraphViewModel();
            this.BindingContext = graphViewModel;

            graphViewModel.GetGraphDotsList().CollectionChanged += DrawChangedDots;
        }

        public void DrawChangedDots(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null) {
                foreach (GraphDot dot in e.NewItems)
                {
                    DrawNewDot(dot);
                }
            }

            if (e.OldItems != null)
            {
                foreach (GraphDot dot in e.OldItems)
                {
                    if (dot.GraphicDot != null) {
                        GraphDrawArea.Children.Remove(dot.GraphicDot);
                    }
                }
            }
        }

        public void DrawNewDot(GraphDot dot)
        {
            var newDot = new BoxView { Color = Color.Olive };
            AbsoluteLayout.SetLayoutBounds(newDot, new Rectangle(dot.Positon.X, dot.Positon.Y, 10, 10));
            AbsoluteLayout.SetLayoutFlags(newDot, AbsoluteLayoutFlags.None);
            dot.GraphicDot = newDot;

            newDot.GestureRecognizers.Add(
            new TapGestureRecognizer()
             {
              Command = new Command(() => {
                  graphViewModel.HelloVar = dot.Positon.Y.ToString();
              })
          }
      );


            GraphDrawArea.Children.Add(newDot);
        }
	}
}