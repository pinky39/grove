namespace Grove.UserInterface
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using System.Windows.Markup;
  using System.Windows.Media;
  using Infrastructure;

  public class BackgroundExtension : MarkupExtension
  {
    private static readonly List<ImageSource> Backgrounds;    

    static BackgroundExtension()
    {
      Backgrounds = Directory.EnumerateFiles(MediaLibrary.ImagesFolder, "background*.*")
        .Select(x => MediaLibrary.GetImageWithPath(x))
        .ToList();
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
      var index = RandomEx.Next(0, Backgrounds.Count);
      return Backgrounds[index];
    }
  }
}