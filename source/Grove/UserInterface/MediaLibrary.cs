namespace Grove.UserInterface
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using System.Windows.Media;
  using System.Windows.Media.Imaging;
  using Gameplay.Sets;
  using Infrastructure;
  using Persistance;

  public static class MediaLibrary
  {
    private const string Images = @"images\";
    private const string Cards = @"cards\";
    private const string Decks = @"decks\";
    private const string Sets = @"sets\";
    private const string SavedGames = @"saved\";
    private const string Tournament = @"tournament\";

    private static readonly Dictionary<string, ImageSource> ImageDatabase = new Dictionary<string, ImageSource>();
    private static readonly Dictionary<string, MagicSet> SetsDatabase = new Dictionary<string, MagicSet>();

    private static readonly Dictionary<int, List<Gameplay.Deck>> LimitedDeckDatabase =
      new Dictionary<int, List<Gameplay.Deck>>();

    private static readonly ImageSource MissingImage = new BitmapImage();


    public static NameGenerator NameGenerator { get; private set; }

#if DEBUG
    private static readonly string BasePath = Path.GetFullPath(@"..\..\..\..\media\");
#else 
    private static readonly string BasePath = Path.GetFullPath(@".\media");
#endif

    public static string ImagesFolder { get { return Path.Combine(BasePath, Images); } }
    public static string DecksFolder { get { return Path.Combine(BasePath, Decks); } }
    public static string SetsFolder { get { return Path.Combine(BasePath, Sets); } }
    public static string TournamentFolder { get { return Path.Combine(BasePath, Tournament); } }
    public static string SavedGamesFolder { get { return Path.Combine(BasePath, SavedGames); } }

    public static string GetSaveGameFilename()
    {
      var filename = String.Format("{0}.savegame", Guid.NewGuid());
      return Path.Combine(SavedGamesFolder, filename);
    }

    public static void LoadResources()
    {
      LoadImageFolder(Images);
      LoadImageFolder(Cards);
      LoadSets();
      LoadLimitedDecks();
      LoadPlayerNames();

      MissingImage.Freeze();
    }

    public static void LoadPlayerNames()
    {
      NameGenerator = new NameGenerator(Path.Combine(BasePath, "player-names.txt"));
    }

    public static void LoadLimitedDecks()
    {
      var group = Directory.GetFiles(TournamentFolder, "*.dec")
        .Select(DeckFile.Read).ToList()
        .Where(x => x.LimitedCode.HasValue)
        .GroupBy(x => x.LimitedCode);

      foreach (var grouping in group)
      {
        LimitedDeckDatabase.Add(grouping.Key.Value, grouping.ToList());
      }
    }

    public static void LoadSets()
    {
      var setsFilenames = Directory.GetFiles(SetsFolder, "*.txt");

      foreach (var filename in setsFilenames)
      {
        var name = Path.GetFileNameWithoutExtension(filename);
        SetsDatabase.Add(name, new MagicSet(filename));
      }
    }

    public static void LoadImageFolder(string path)
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
      return GetImage(name, Cards);        
    }

    public static List<string> GetSetsNames()
    {
      return SetsDatabase.Keys.ToList();
    }

    public static MagicSet GetSet(string name)
    {
      return SetsDatabase[name];
    }

    public static IEnumerable<Gameplay.Deck> GetDecks(int limitedCode)
    {
      if (LimitedDeckDatabase.ContainsKey(limitedCode))
      {
        return LimitedDeckDatabase[limitedCode];
      }

      return Enumerable.Empty<Gameplay.Deck>();
    }

    public static ImageSource GetImage(string name, string folder = null)
    {      
      folder = folder ?? Images;
      var path = GetImagePath(folder, name);
      return GetImageWithPath(path);
    }

    public static ImageSource GetImageWithPath(string path)
    {
      if (!File.Exists(path))
        return MissingImage;

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

    public static string[] GetSavedGamesFilenames()
    {
      return Directory.GetFiles(SavedGamesFolder, "*.savegame");
    }

    public static object GetSetImage(string set, Rarity? rarity)
    {
      if (String.IsNullOrEmpty(set) || rarity == null)
      {
        return MissingImage;
      }

      var filename = Path.Combine(ImagesFolder, String.Format("{0}-{1}.png", set, rarity));
      return GetImageWithPath(filename);
    }
  }
}