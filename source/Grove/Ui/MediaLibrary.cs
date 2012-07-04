namespace Grove.Ui
{
  using System;
  using System.IO;
  using System.Windows.Media;
  using System.Windows.Media.Imaging;

  public static class MediaLibrary
  {
    private const string Images = @"images\";
    private const string Cards = @"cards\";
    private const string Decks = @"decks\";

#if DEBUG
    private static readonly string BasePath = Path.GetFullPath(@"..\..\..\..\media\");
#else 
    private static readonly string BasePath = Path.GetFullPath(@".\media");
#endif

    public static string ImagesFolder { get { return Path.Combine(BasePath, Images); } }
    public static string DecksFolder {get { return Path.Combine(BasePath, Decks); }}
    

    public static ImageSource GetCardImage(string name)
    {
      return File.Exists(GetImagePath(Cards, name))
        ? GetImage(name, Cards)
        : GetImage("missing-card-image.jpg");
    }

    public static ImageSource GetImage(string name, string folder = null)
    {
      folder = folder ?? Images;
      var path = GetImagePath(folder, name);
      var uri = new Uri(path);
      return new BitmapImage(uri);
    }

    public static ImageSource GetImageWithPath(string path)
    {
      var uri = new Uri(path);
      return new BitmapImage(uri);
    }

    private static string GetImagePath(string folder, string name)
    {
      return Path.Combine(BasePath, folder, name);
    }

    public static Stream GetDeck(string name)
    {
      var path = Path.Combine(BasePath, Decks, name + ".dec");
      return File.OpenRead(path);
    }
  }
}