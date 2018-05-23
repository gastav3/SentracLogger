using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SentLogger.Resources
{
  /// <summary>
  /// Makes it possible to use images in Xaml.
  /// Source: https://docs.microsoft.com/en-us/xamarin/xamarin-forms/user-interface/images?tabs=vswin#Downloading_Images
  /// </summary>
  [ContentProperty(nameof(Source))]
  public class ImageResourceExtension : IMarkupExtension
  {
    public string Source { get; set; }

    public object ProvideValue(IServiceProvider serviceProvider)
    {
      if (Source == null)
      {
        return null;
      }

      var imageSource = ImageSource.FromResource(Source);

      return imageSource;
    }
  }
}