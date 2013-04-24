namespace Grove.Ui
{
  using System;
  using System.IO;
  using System.Linq;
  using System.Windows.Media;
  using System.Windows.Media.Imaging;
  using Core;
  using Gameplay.Card.Factory;
  using Gameplay.Deck;

  public static class MediaLibrary
  {
    private static readonly Random Rnd = new Random();
    private const string Images = @"images\";
    private const string Cards = @"cards\";
    private const string Decks = @"decks\";
    private const string Sets = @"sets\";

#if DEBUG
    private static readonly string BasePath = Path.GetFullPath(@"..\..\..\..\media\");
#else 
    private static readonly string BasePath = Path.GetFullPath(@".\media");
#endif

    public static string ImagesFolder { get { return Path.Combine(BasePath, Images); } }
    public static string DecksFolder { get { return Path.Combine(BasePath, Decks); } }
    public static string SetsFolder { get { return Path.Combine(BasePath, Sets); } }

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

    public static string GetDeckPath(string name)
    {
      return Path.Combine(DecksFolder, name + ".dec");
    }

    public static string GetSetPath(string name)
    {
      return Path.Combine(SetsFolder, name + ".txt");
    }

    public static Gameplay.Deck.Deck GetDeck(string name, CardDatabase cardDatabase)
    {
      name = name.EndsWith(".dec") ? name : name + ".dec";

      var path = Path.Combine(BasePath, Decks, name);
      return new DeckReaderWriter().Read(path, cardDatabase);
    }

    public static Gameplay.Deck.Deck GetRandomDeck(CardDatabase cardDatabase)
    {
      var decks = Directory.EnumerateFiles(DecksFolder, "*.dec").ToList();

      return decks.Count > 0 ? GetDeck(decks[Rnd.Next(0, decks.Count)], cardDatabase)
        : null;
    }
  }
}