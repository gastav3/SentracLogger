using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SkiaSharp;
using SkiaSharp.Views.Forms;

namespace SentLogger.Views
{
  /// <summary>
  /// View for the File Explorer.
  /// </summary>
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FileExplorerView : ContentPage
	{
        SKPaint blackFillPaint = new SKPaint
        {
            Style = SKPaintStyle.Fill,
            Color = SKColors.Black
        };

		public FileExplorerView ()
		{
			InitializeComponent ();
		}

        private void CanvasView_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear(SKColors.CornflowerBlue);

            int width = e.Info.Width;
            int height = e.Info.Height;

            canvas.Translate(width/2, height/2);
            canvas.Scale(width / 200f);

            canvas.DrawCircle(0, 0, 100, blackFillPaint);
        }
	}
}