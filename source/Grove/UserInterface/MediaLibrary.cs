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
    private const string Avatars = @"avatars\";

    private static readonly Dictionary<string, ImageSource> ImageDatabase = new Dictionary<string, ImageSource>();
    private static readonly Dictionary<string, MagicSet> SetsDatabase = new Dictionary<string, MagicSet>();
    private static readonly List<ImageSource> AvatarsDatabase = new List<ImageSource>();

    private static readonly Dictionary<int, List<Gameplay.Deck>> LimitedDeckDatabase =
      new Dictionary<int, List<Gameplay.Deck>>();

    public static NameGenerator NameGenerator { get; private set; }

#if DEBUG
    private static readonly string BasePath = Path.GetFullPath(@"..\..\..\..\media\");
#else 
    private static readonly string BasePath = Path.GetFullPath(@".\media");
#endif

    public static string ImagesFolder { get { return Path.Combine(BasePath, Images); } }
    public static string AvatarsFolder { get { return Path.Combine(BasePath, Avatars); } }
    public static string DecksFolder { get { return Path.Combine(BasePath, Decks); } }
    public static string SetsFolder { get { return Path.Combine(BasePath, Sets); } }
    public static string TournamentFolder { get { return Path.Combine(BasePath, Tournament); } }
    public static string SavedGamesFolder { get { return Path.Combine(BasePath, SavedGames); } }

    public static ImageSource MissingImage
    {
      get
      {
        var filename = Path.Combine(ImagesFolder, "missing.png");

        ImageSource missingImage = null;
        if (ImageDatabase.TryGetValue(filename, out missingImage))
        {
          return missingImage;
        }

        return new BitmapImage(new Uri(filename));
      }
    }

    public static string GetSaveGameFilename()
    {
      var filename = String.Format("{0}.savegame", Guid.NewGuid());
      return Path.Combine(SavedGamesFolder, filename);
    }

    public static void LoadResources()
    {
      var elapsed = new[]
        {
          Profiler.Benchmark(() => LoadImageFolder(Images)),            
          Profiler.Benchmark(() => LoadImageFolder(Cards)), 
          Profiler.Benchmark(() => LoadSets()), 
          Profiler.Benchmark(() => LoadLimitedDecks()),
          Profiler.Benchmark(() => LoadPlayerNames()),
          Profiler.Benchmark(() => LoadAvatars())
        };           
      
      LogFile.Info("Startup benchmark:\nImages:{0:n}\nCards:{1:n}\nSets:{2:n}\nSealed decks:{3:n}\nPlayer names:{4:n}\nAvatars{5:n}",
        elapsed[0], elapsed[1], elapsed[2], elapsed[3], elapsed[4], elapsed[5]);
    }

    public static void LoadPlayerNames()
    {
      NameGenerator = new NameGenerator(Path.Combine(BasePath, "player-names.txt"));
    }

    public static void LoadLimitedDecks()
    {
      var files = Directory.EnumerateFiles(TournamentFolder, "*.dec");

      foreach (var file in files)
      {
        var deck = DeckFile.Read(file);

        if (deck.LimitedCode.HasValue)
        {
          List<Gameplay.Deck> groupedByCode;
          if (!LimitedDeckDatabase.TryGetValue(deck.LimitedCode.Value, out groupedByCode))
          {
            groupedByCode = new List<Gameplay.Deck>();
            LimitedDeckDatabase.Add(deck.LimitedCode.Value, groupedByCode);
          }

          groupedByCode.Add(deck);
        }
      }
    }

    public static void LoadSets()
    {
      var setsFilenames = Directory.EnumerateFiles(SetsFolder, "*.txt");

      foreach (var filename in setsFilenames)
      {
        var name = Path.GetFileNameWithoutExtension(filename);
        SetsDatabase.Add(name, new MagicSet(filename));
      }
    }

    public static void LoadAvatars()
    {
      var files = Directory.EnumerateFiles(AvatarsFolder);

      foreach (var file in files)
      {
        var uri = new Uri(file);
        var bitmapImage = new BitmapImage(uri);
        bitmapImage.Freeze();
        AvatarsDatabase.Add(bitmapImage);
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
        ImageDatabase.Add(file.ToLowerInvariant(), bitmapImage);
      }
    }

    public static ImageSource GetCardImage(string name)
    {
      return GetImage(name, Cards);
    }

    public static ImageSource GetAvatar(int id)
    {
      if (AvatarsDatabase.Count == 0)
        return MissingImage;

      return AvatarsDatabase[id%AvatarsDatabase.Count];
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
      path = path.ToLowerInvariant();

      if (ImageDatabase.ContainsKey(path))
        return ImageDatabase[path];

      return MissingImage;
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

    public static MagicSet RandomSet()
    {
      var sets = SetsDatabase.Values.ToList();
      return sets[RandomEx.Next(sets.Count)];
    }
  }
}