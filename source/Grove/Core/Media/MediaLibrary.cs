namespace Grove.Media
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using System.Text;
  using System.Windows.Media;
  using System.Windows.Media.Imaging;
  using Infrastructure;

  public static class MediaLibrary
  {
    public delegate void ProgressIndicator(long current, long total);

#if DEBUG
    public static readonly string BasePath = Path.GetFullPath(@"..\..\..\..\media");
#else 
    public static readonly string BasePath = Path.GetFullPath(@".\media");
#endif

    public static class Folders
    {
      public static readonly ResourceFolder Clipart = "images";
      public static readonly ResourceFolder Cards = "cards";
      public static readonly ResourceFolder Sets = "sets";
      public static readonly ResourceFolder Avatars = "avatars";      

      public static long GetSize()
      {
        return Clipart.GetSize() + Cards.GetSize() + Sets.GetSize() + Avatars.GetSize();
      }
    }

    private static readonly Dictionary<string, ImageSource> Clipart = new Dictionary<string, ImageSource>();
    private static readonly Dictionary<string, ImageSource> CardImages = new Dictionary<string, ImageSource>();    
    private static readonly Dictionary<string, MagicSet> Sets = new Dictionary<string, MagicSet>();
    private static readonly List<ImageSource> Avatars = new List<ImageSource>();
    private static readonly List<string> PlayerNames = new List<string>();

    private static ImageSource MissingImage
    {
      get { return Clipart["missing.png"]; }
    }

    public static void LoadAll(ProgressIndicator showProgress = null)
    {
      showProgress = showProgress ?? delegate { };
      var totalBytes = Folders.GetSize();
      var updateProgress = Progress(showProgress, totalBytes);

      LoadPlayerNames();
      LoadClipart(updateProgress);
      LoadCardImages(updateProgress);
      LoadSets(updateProgress);
      LoadAvatars(updateProgress);      
    }

    private static Action<long> Progress(ProgressIndicator showProgress, long totalBytes)
    {
      long loaded = 0;

      return chunkSize =>
        {
          loaded += chunkSize;
          showProgress(loaded, totalBytes);
        };
    }

    private static void LoadPlayerNames()
    {
      var rows = File.ReadAllLines(Path.Combine(BasePath, "player-names.txt"));

      foreach (var row in rows)
      {
        var trimmed = row.Trim();

        if (trimmed.StartsWith("#"))
          continue;

        if (String.IsNullOrEmpty(trimmed))
          continue;

        PlayerNames.Add(trimmed);
      }
    }

    private static void LoadResources(ResourceFolder folder, Action<Resource> loadAction)
    {
      var resources = folder.ReadAll();

      foreach (var resource in resources)
      {
        loadAction(resource);
      }
    }

    public static void LoadSets(Action<long> showProgress = null)
    {
      showProgress = showProgress ?? delegate { };

      LoadResources(Folders.Sets, r =>
        {
          var setName = Path.GetFileNameWithoutExtension(r.Name);
          var set = new MagicSet(setName, Encoding.UTF8.GetString(r.Content));
          Sets[setName.ToLowerInvariant()] = set;
          showProgress(r.Content.Length);
        });
    }

    private static void LoadAvatars(Action<long> showProgress)
    {
      LoadResources(Folders.Avatars, r =>
        {
          Avatars.Add(CreateBitmap(r.Content));
          showProgress(r.Content.Length);
        });
    }   

    private static void LoadClipart(Action<long> showProgress)
    {
      LoadResources(Folders.Clipart, r =>
        {
          Clipart.Add(r.Name.ToLowerInvariant(), CreateBitmap(r.Content));
          showProgress(r.Content.Length);
        });
    }

    private static void LoadCardImages(Action<long> showProgress)
    {
      LoadResources(Folders.Cards, r =>
        {
          CardImages.Add(r.Name.ToLowerInvariant(), CreateBitmap(r.Content));
          showProgress(r.Content.Length);
        });
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

    public static List<string> GetPlayerUnitNames()
    {
      return PlayerNames;
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

      if (id < 0)
      {
        id = -id;
        id = id%Avatars.Count;
        return Avatars[Avatars.Count - id - 1];
      }

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
  }
}