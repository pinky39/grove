namespace Grove.Ui.DeckScreen
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using Core;
  using Infrastructure;
  using Shell;

  public class ViewModel : IIsDialogHost
  {
    private const string RandomDeck = "Random";
    private static readonly Random Rnd = new Random();

    private static readonly string[] AvailableDecks = Directory.EnumerateFiles(MediaLibrary.DecksFolder, "*.dec")
      .Select(Path.GetFileNameWithoutExtension).ToArray();

    private readonly Match _match;
    private readonly IShell _shell;
    private readonly StartScreen.ViewModel.IFactory _startScreenFactory;

    public ViewModel(Match match, StartScreen.ViewModel.IFactory startScreenFactory, IShell shell)
    {
      _match = match;
      _startScreenFactory = startScreenFactory;
      _shell = shell;

      Player1 = new Player(Match.Player1Name, "player1.png");
      Player2 = new Player(Match.Player2Name, "player2.png");
    }

    public Player Player1 { get; private set; }
    public Player Player2 { get; private set; }

    [Updates("CanStart")]
    public virtual bool IsStarting { get; protected set; }

    public IEnumerable<string> Decks
    {
      get
      {
        yield return RandomDeck;

        foreach (var deck in AvailableDecks)
        {
          yield return deck;
        }
      }
    }

    public bool CanStart
    {
      get { return !IsStarting; }
    }

    public void AddDialog(object dialog, DialogType dialogType) {}

    public void RemoveDialog(object dialog) {}

    public bool HasFocus(object dialog)
    {
      return false;
    }

    private static string GetRandomDeck()
    {
      return AvailableDecks[Rnd.Next(0, AvailableDecks.Length)];
    }

    public void Start()
    {
      IsStarting = true;
      _match.Start(Player1.GetDeck(), Player2.GetDeck());
    }

    public void ReturnToMainMenu()
    {
      var startScreen = _startScreenFactory.Create();
      _shell.ChangeScreen(startScreen);
    }

    public interface IFactory
    {
      ViewModel Create();
    }

    public class Player
    {
      public Player(string name, string avatar)
      {
        Name = name;
        Avatar = avatar;
        Deck = RandomDeck;
      }

      public string Name { get; private set; }
      public string Avatar { get; private set; }
      public string Deck { get; set; }

      public string GetDeck()
      {
        return Deck == RandomDeck ? GetRandomDeck() : Deck;
      }
    }
  }
}