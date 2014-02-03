namespace Grove.UserInterface
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Windows.Markup;
  using System.Windows.Media;
  using Infrastructure;
  using Persistance;

  public class BackgroundExtension : MarkupExtension
  {
    private static readonly List<ImageSource> Backgrounds;

    static BackgroundExtension()
    {
      Backgrounds = MediaLibrary.GetImages(filename => filename.StartsWith("background"))
        .ToList();
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
      var index = RandomEx.Next(0, Backgrounds.Count);
      return Backgrounds[index];
    }
  }
}