using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SentLogger.Views
{
  /// <summary>
  /// View for the histogram with function.
  /// </summary>
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HistogramView : ContentPage
	{
    public HistogramView ()
		{
			InitializeComponent ();
		}
  }
}