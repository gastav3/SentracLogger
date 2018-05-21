using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using SentLogger.Resources;
using SentLogger.iOS.Resources;

[assembly: ExportRenderer(typeof(SentLogger.Resources.MyBoxView), typeof(RoundBoxViewRender))]
namespace SentLogger.iOS.Resources
{
    public class RoundBoxViewRender: BoxRenderer
    {   /// <summary>
        /// iOS translation of create a Bindable Property For CornerRadius
        /// </summary>
    protected override void OnElementChanged(ElementChangedEventArgs<BoxView> e)
    {
      base.OnElementChanged(e);
      if (Element == null) return;
      Layer.MasksToBounds = true;
      Layer.CornerRadius = (float)((MyBoxView)this.Element).CornerRadius / 2.0f;
    }
  }
}