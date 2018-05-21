using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SentLogger.Resources
{
  public class MyBoxView : BoxView
  {   /// <summary>
      /// Create a Bindable Property For CornerRadius
      /// </summary>
    public static readonly BindableProperty CornerRadiusProperty =
        BindableProperty.Create(nameof(CornerRadius), typeof(double), typeof(MyBoxView), 0.0);

    public double CornerRadius
    {
      get { return (double)GetValue(CornerRadiusProperty); }
      set { SetValue(CornerRadiusProperty, value); }
    }
  }
}