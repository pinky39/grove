namespace Grove.UserInterface
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using System.Windows.Media;
  using System.Windows.Media.Imaging;
  using Gameplay;
  using Gameplay.Sets;

  public static class MediaLibrary
  {
    private static readonly Random Rnd = new Random();
    private const string Images = @"images\";
    private const string Cards = @"cards\";
    private const string Decks = @"decks\";
    private const string Sets = @"sets\";

    private static readonly Dictionary<string, ImageSource> ImageDatabase = new Dictionary<string, ImageSource>();

    private static Dictionary<string, MagicSet> SetsDatabase
    {
      get
      {
        if (_setsDatabase == null)
        {
          LoadSets();
        }

        return _setsDatabase;
      }
    }

    public static NameGenerator NameGenerator { get; private set; }

#if DEBUG
    private static readonly string BasePath = Path.GetFullPath(@"..\..\..\..\media\");
    private static Dictionary<string, MagicSet> _setsDatabase;
#else 
    private static readonly string BasePath = Path.GetFullPath(@".\media");
#endif

    public static string ImagesFolder { get { return Path.Combine(BasePath, Images); } }
    public static string DecksFolder { get { return Path.Combine(BasePath, Decks); } }
    public static string SetsFolder { get { return Path.Combine(BasePath, Sets); } }

    public static void LoadResources()
    {
      LoadImageFolder(Images);
      LoadImageFolder(Cards);
      LoadSets();

      NameGenerator = new NameGenerator(Path.Combine(BasePath, "player-names.txt"));
    }

    private static void LoadSets()
    {
      _setsDatabase = new Dictionary<string, MagicSet>();
      var setsFilenames = Directory.GetFiles(SetsFolder, "*.txt");

      foreach (var filename in setsFilenames)
      {
        var name = Path.GetFileNameWithoutExtension(filename);
        _setsDatabase.Add(name, new MagicSet(filename));
      }
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
        ImageDatabase.Add(file, bitmapImage);
      }
    }

    public static ImageSource GetCardImage(string name)
    {
      return File.Exists(GetImagePath(Cards, name))
        ? GetImage(name, Cards)
        : GetImage("missing-card-image.jpg");
    }

    public static List<string> GetSetsNames()
    {
      return SetsDatabase.Keys.ToList();
    }

    public static MagicSet GetSet(string name)
    {
      return SetsDatabase[name];
    }

    public static ImageSource GetImage(string name, string folder = null)
    {
      folder = folder ?? Images;
      var path = GetImagePath(folder, name);
      return GetImageWithPath(path);
    }

    public static ImageSource GetImageWithPath(string path)
    {
      if (ImageDatabase.ContainsKey(path))
        return ImageDatabase[path];

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

    public static string[] GetDeckFilenames()
    {
      return Directory.GetFiles(DecksFolder, "*.dec");
    }
  }
}