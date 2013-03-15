namespace Grove.Ui
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using System.Windows.Markup;
  using System.Windows.Media;

  public class BackgroundExtension : MarkupExtension
  {
    private static readonly List<ImageSource> Backgrounds;
    private static readonly Random Rnd = new Random();

    static BackgroundExtension()
    {
      Backgrounds = Directory.EnumerateFiles(MediaLibrary.ImagesFolder, "background*.*")
        .Select(x => MediaLibrary.GetImageWithPath(x))
        .ToList();
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
      var index = Rnd.Next(0, Backgrounds.Count);
      return Backgrounds[index];
    }
  }
}