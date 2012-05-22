namespace Grove.Ui
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using System.Windows.Markup;

  public class BackgroundExtension : MarkupExtension
  {
    private static readonly List<string> Backgrounds;
    private static readonly Random Rnd = new Random();

    static BackgroundExtension()
    {
      Backgrounds = Directory.EnumerateFiles(MediaLibrary.ImagesFolder, "background*.*").ToList();
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
      var index = Rnd.Next(0, Backgrounds.Count);      
      return MediaLibrary.GetImageWithPath(Backgrounds[index]);
    }
  }
}