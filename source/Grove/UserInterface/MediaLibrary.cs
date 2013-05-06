namespace Grove.UserInterface
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using System.Windows.Media;
  using System.Windows.Media.Imaging;
  using Gameplay;
  using Persistance;

  public static class MediaLibrary
  {
    private static readonly Random Rnd = new Random();
    private const string Images = @"images\";
    private const string Cards = @"cards\";
    private const string Decks = @"decks\";
    private const string Sets = @"sets\";

    private static readonly Dictionary<string, ImageSource> ImageCache = new Dictionary<string, ImageSource>();

#if DEBUG
    private static readonly string BasePath = Path.GetFullPath(@"..\..\..\..\media\");
#else 
    private static readonly string BasePath = Path.GetFullPath(@".\media");
#endif

    public static string ImagesFolder { get { return Path.Combine(BasePath, Images); } }
    public static string DecksFolder { get { return Path.Combine(BasePath, Decks); } }
    public static string SetsFolder { get { return Path.Combine(BasePath, Sets); } }

    public static void LoadImages()
    {
      LoadImageFolder(Images);
      LoadImageFolder(Cards);
    }

    private static void LoadImageFolder(string path)
    {
      var fullPath = Path.Combine(BasePath, path);

      if (!Directory.Exists(fullPath))
        return;

      var files = Directory.EnumerateFiles(fullPath);

      foreach (var file in files)
      {
        var uri = new Uri(file);
        var bitmapImage = new BitmapImage(uri);
        bitmapImage.Freeze();
        ImageCache.Add(file, bitmapImage);
      }
    }


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
      return GetImageWithPath(path);
    }

    public static ImageSource GetImageWithPath(string path)
    {
      if (ImageCache.ContainsKey(path))
        return ImageCache[path];

      var uri = new Uri(path);
      return new BitmapImage(uri);
    }

    private static string GetImagePath(string folder, string name)
    {
      return Path.Combine(BasePath, folder, name);
    }

    public static string GetDeckPath(string name)
    {
      return Path.Combine(DecksFolder, name + ".dec");
    }

    public static string GetSetPath(string name)
    {
      return Path.Combine(SetsFolder, name + ".txt");
    }

    public static Gameplay.Deck GetDeck(string name, CardDatabase cardDatabase)
    {
      name = name.EndsWith(".dec") ? name : name + ".dec";

      var path = Path.Combine(BasePath, Decks, name);
      return new DeckReaderWriter().Read(path, cardDatabase);
    }

    public static Gameplay.Deck GetRandomDeck(CardDatabase cardDatabase)
    {
      var decks = Directory.EnumerateFiles(DecksFolder, "*.dec").ToList();

      return decks.Count > 0 ? GetDeck(decks[Rnd.Next(0, decks.Count)], cardDatabase)
        : null;
    }
  }
}