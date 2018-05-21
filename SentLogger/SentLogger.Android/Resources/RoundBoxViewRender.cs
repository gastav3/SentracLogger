using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Android.Util;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using SentLogger.Resources;
using SentLogger.Droid.Resources;

[assembly: ExportRenderer(typeof(MyBoxView), typeof(RoundBoxViewRender))]
namespace SentLogger.Droid.Resources
{
    public class RoundBoxViewRender: BoxRenderer
    {   /// <summary>
        /// Android translation of create a Bindable Property For CornerRadius
        /// </summary>
    private float _cornerRadius;
        private RectF _bounds;
        private Path _path;
        protected override void OnElementChanged(ElementChangedEventArgs<BoxView> e)
        {
            base.OnElementChanged(e);
            if (Element == null)
            {
                return;
            }
            var element = (MyBoxView)Element;
            _cornerRadius = TypedValue.ApplyDimension(ComplexUnitType.Dip,
            (float)element.CornerRadius, Context.Resources.DisplayMetrics);
        }

        protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
        {
            base.OnSizeChanged(w, h, oldw, oldh);
            if (w != oldw && h != oldh)
            {
                _bounds = new RectF(0, 0, w, h);
            }
            _path = new Path();
            _path.Reset();
            _path.AddRoundRect(_bounds, _cornerRadius, _cornerRadius, Path.Direction.Cw);
            _path.Close();
        }

        public override void Draw(Canvas canvas)
        {
            canvas.Save();
            canvas.ClipPath(_path);
            base.Draw(canvas);
            canvas.Restore();
        }
    }
}