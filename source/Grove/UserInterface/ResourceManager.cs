namespace Grove.UserInterface
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using System.Text;
  using System.Windows.Media;
  using System.Windows.Media.Imaging;
  using Gameplay.Sets;
  using Infrastructure;
  using Ionic.Zip;
  using Persistance;

  public static class ResourceManager
  {
#if DEBUG
    private static readonly string BasePath = Path.GetFullPath(@"..\..\..\..\media");
#else 
    private static readonly string BasePath = Path.GetFullPath(@".\media");
#endif

    private class ResourceLibrary
    {
      private readonly string _directory;
      private readonly string _zipFile;

      public ResourceLibrary(string name)
      {
        _directory = Path.Combine(BasePath, name);
        _zipFile = Path.Combine(BasePath, name + ".zip");
      }

      public bool HasDirectory { get { return Directory.Exists(_directory); } }
      public bool HasZip { get { return File.Exists(_zipFile); } }

      private string GetFullPath(string filename)
      {
        return Path.Combine(_directory, filename);
      }

      private IEnumerable<Resource> GetFromDirectory()
      {
        return Directory.EnumerateFiles(_directory)
          .Where(file => !Path.GetFileName(file).StartsWith("."))
          .Select(file => new Resource(
            name: Path.GetFileName(file).ToLowerInvariant(),
            content: File.ReadAllBytes(file),
            modifiedAt: new FileInfo(file).LastWriteTime));
      }

      private IEnumerable<Resource> GetFromZip()
      {
        using (var file = ZipFile.Read(_zipFile))
        {
          foreach (var entry in file.Entries)
          {
            using (var stream = new MemoryStream())
            {
              entry.Extract(stream);

              yield return new Resource(
                name: entry.FileName.ToLowerInvariant(),
                content: stream.ToArray(),
                modifiedAt: entry.LastModified);
            }
          }
        }
      }

      public void Add(string name, byte[] data)
      {
        if (HasZip)
        {
          AddToZip(name, data);
          return;
        }

        AddToDirectory(name, data);
      }

      private void AddToDirectory(string name, byte[] data)
      {
        File.WriteAllBytes(GetFullPath(name), data);
      }

      private void AddToZip(string name, byte[] data)
      {
        using (var file = new ZipFile(_zipFile))
        {
          file.AddEntry(name, data);
          file.Save();
        }
      }

      public List<Resource> Get()
      {
        var library = new Dictionary<string, Resource>();

        if (HasDirectory)
        {
          foreach (var resource in GetFromDirectory())
          {
            library[resource.Name] = resource;
          }
        }

        if (HasZip)
        {
          foreach (var resource in GetFromZip())
          {
            library[resource.Name] = resource;
          }
        }

        return library.Values.ToList();
      }

      public static implicit operator ResourceLibrary(string name)
      {
        return new ResourceLibrary(name);
      }
    }

    private static readonly ResourceLibrary ImagesLibrary = "images";
    private static readonly ResourceLibrary CardLibrary = "cards";
    private static readonly ResourceLibrary DeckLibrary = "decks";
    private static readonly ResourceLibrary SetLibrary = "sets";
    private static readonly ResourceLibrary SavedGames = "saved";
    private static readonly ResourceLibrary Tournament = "tournament";
    private static readonly ResourceLibrary AvatarLibrary = "avatars";

    private static readonly Dictionary<string, ImageSource> Clipart = new Dictionary<string, ImageSource>();
    private static readonly Dictionary<string, ImageSource> CardImages = new Dictionary<string, ImageSource>();
    private static readonly Dictionary<string, MagicSet> Sets = new Dictionary<string, MagicSet>();
    private static readonly List<ImageSource> Avatars = new List<ImageSource>();

    private static readonly Dictionary<int, List<Gameplay.Deck>> LimitedDeckDatabase =
      new Dictionary<int, List<Gameplay.Deck>>();

    public static NameGenerator NameGenerator { get; private set; }
    public static ImageSource MissingImage { get { return Clipart["missing.png"]; } }

    public static void LoadResources()
    {
      var elapsed = new[]
        {
          Profiler.Benchmark(() => LoadClipart()),
          Profiler.Benchmark(() => LoadCardImages()),
          Profiler.Benchmark(() => LoadSets()),
          Profiler.Benchmark(() => LoadLimitedDecks()),
          Profiler.Benchmark(() => LoadPlayerNames()),
          Profiler.Benchmark(() => LoadAvatars())
        };

      LogFile.Info(
        "Startup benchmark:\nImages:{0:n}\nCards:{1:n}\nSets:{2:n}\nSealed decks:{3:n}\nPlayer names:{4:n}\nAvatars{5:n}",
        elapsed[0], elapsed[1], elapsed[2], elapsed[3], elapsed[4], elapsed[5]);
    }

    public static void LoadPlayerNames()
    {
      NameGenerator = new NameGenerator(Path.Combine(BasePath, "player-names.txt"));
    }

    public static void LoadLimitedDecks()
    {
      var library = Tournament.Get();

      foreach (var resource in library)
      {
        var deck = DeckFile.Read(resource.Name, resource.Content);

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
      var library = SetLibrary.Get();

      foreach (var resource in library)
      {
        var setName = Path.GetFileNameWithoutExtension(resource.Name);
        
        Sets.Add(
          setName,
          new MagicSet(setName, Encoding.UTF8.GetString(resource.Content)));
      }
    }

    private class Resource
    {
      public readonly byte[] Content;
      public readonly DateTime ModifiedAt;
      public readonly string Name;

      public Resource(string name, byte[] content, DateTime modifiedAt)
      {
        Content = content;
        Name = name;
        ModifiedAt = modifiedAt;
      }
    }


    public static void LoadAvatars()
    {
      var library = AvatarLibrary.Get();

      foreach (var resource in library)
      {
        Avatars.Add(
          CreateBitmap(resource.Content));
      }
    }

    public static void LoadClipart()
    {
      var library = ImagesLibrary.Get();

      foreach (var resource in library)
      {
        Clipart.Add(
          resource.Name,
          CreateBitmap(resource.Content));
      }
    }

    public static void LoadCardImages()
    {
      var library = CardLibrary.Get();

      foreach (var resource in library)
      {
        CardImages.Add(
          resource.Name,
          CreateBitmap(resource.Content));
      }
    }

    private static BitmapImage CreateBitmap(byte[] content)
    {
      var image = new BitmapImage();
      image.BeginInit();
      image.StreamSource = new MemoryStream(content);
      image.CacheOption = BitmapCacheOption.OnLoad;
      image.EndInit();
      image.Freeze();
      return image;
    }

    public static ImageSource GetCardImage(string name)
    {
      var filename = name.ToLowerInvariant() + ".jpg";

      if (CardImages.ContainsKey(filename))
        return CardImages[filename];

      return MissingImage;
    }

    public static ImageSource GetAvatar(int id)
    {
      if (Avatars.Count == 0)
        return MissingImage;

      return Avatars[id%Avatars.Count];
    }

    public static List<string> GetSetsNames()
    {
      return Sets.Keys.Select(x => x.CapitalizeEachWord()).ToList();
    }

    public static MagicSet GetSet(string name)
    {
      return Sets[name.ToLowerInvariant()];
    }

    public static IEnumerable<Gameplay.Deck> GetDecks(int limitedCode)
    {
      if (LimitedDeckDatabase.ContainsKey(limitedCode))
      {
        return LimitedDeckDatabase[limitedCode];
      }

      return Enumerable.Empty<Gameplay.Deck>();
    }

    public static ImageSource GetImage(string filename)
    {
      filename = filename.ToLowerInvariant();

      if (Clipart.ContainsKey(filename))
        return Clipart[filename];

      return MissingImage;
    }

    public static IEnumerable<ImageSource> GetImages(Func<string, bool> filter)
    {
      foreach (var keyValuePair in Clipart)
      {
        if (filter(keyValuePair.Key))
          yield return keyValuePair.Value;
      }
    }

    public static IEnumerable<Gameplay.Deck> ReadDecks()
    {
      var library = DeckLibrary.Get();

      foreach (var resource in library)
      {
        yield return DeckFile.Read(resource.Name, resource.Content);
      }
    }

    public static IEnumerable<SaveGameFile> ReadSavedGames()
    {
      var savedGames = SavedGames.Get();

      return savedGames.Select(x =>
        SaveLoadHelper.ReadFile(x.Name, new MemoryStream(x.Content), x.ModifiedAt));
    }

    public static void SaveGame(SaveFileHeader header, object data)
    {
      SavedGames.Add(String.Format("{0}.savegame", Guid.NewGuid()),
        SaveLoadHelper.Serialize(header, data));
    }

    public static void SaveDeck(Gameplay.Deck deck)
    {
      DeckLibrary.Add(deck.Name, DeckFile.Write(deck));
    }

    public static void SaveGeneratedDeck(Gameplay.Deck deck)
    {
      Tournament.Add(Guid.NewGuid() + ".dec", DeckFile.Write(deck));
    }

    public static object GetSetImage(string set, Rarity? rarity)
    {
      if (String.IsNullOrEmpty(set) || rarity == null)
      {
        return MissingImage;
      }

      return GetImage(String.Format("{0}-{1}.png", set, rarity));
    }

    public static MagicSet RandomSet()
    {
      var sets = Sets.Values.ToList();
      return sets[RandomEx.Next(sets.Count)];
    }

    public static void SaveDebugReport(SaveFileHeader header, SavedGame savedGame)
    {
      var filename = String.Format("debug-report-{0}.report", Guid.NewGuid());
      File.WriteAllBytes(filename, SaveLoadHelper.Serialize(header, filename));
    }
  }
}