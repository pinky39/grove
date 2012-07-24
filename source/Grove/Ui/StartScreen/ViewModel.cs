namespace Grove.Ui.StartScreen
{
  using System;
  using System.Diagnostics;
  using System.IO;
  using System.Linq;
  using System.Reflection;
  using System.Windows;
  using Core;
  using SelectDeck;
  using Shell;

  public class ViewModel : IIsDialogHost
  {
    private static readonly Random Rnd = new Random();
    private readonly CardDatabase _cardDatabase;
    private readonly Match _match;
    private readonly SelectDeck.ViewModel.IFactory _selectDeckScreenFactory;
    private readonly IShell _shell;

    public ViewModel(IShell shell, CardDatabase cardDatabase, SelectDeck.ViewModel.IFactory selectDeckScreenFactory,
      Match match)
    {
      _shell = shell;
      _cardDatabase = cardDatabase;
      _selectDeckScreenFactory = selectDeckScreenFactory;
      _match = match;
    }

    public string DatabaseInfo
    {
      get
      {
        return string.Format("Release {0} ({1} cards)",
          FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion,
          _cardDatabase.CardCount);
      }
    }

    public void AddDialog(object dialog, DialogType dialogType) {}

    public bool HasFocus(object dialog)
    {
      return false;
    }

    public void CloseAllDialogs() {}

    public void RemoveDialog(object dialog) {}

    public void Exit()
    {
      Application.Current.Shutdown();
    }

    public void Play()
    {
      var deckScreen = _selectDeckScreenFactory.Create(ScreenType.YourDeck, this);
      _shell.ChangeScreen(deckScreen);
    }

    public void PlayRandom()
    {
      Deck firstDeck;
      Deck secondDeck;

      ChooseRandomDecks(out firstDeck, out secondDeck);

      _match.Start(firstDeck, secondDeck);
    }

    private void ChooseRandomDecks(out Deck firstDeck, out Deck secondDeck)
    {
      Deck first;
      Deck second;

      var deckFiles = Directory.EnumerateFiles(MediaLibrary.DecksFolder, "*.dec");
      var decks = deckFiles.Select(fileName => new Deck(fileName, _cardDatabase)).ToList();
      first = decks[Rnd.Next(0, decks.Count)];
      var decksWithSameRating = decks.Where(x => x.Rating == first.Rating).ToList();
      second = decksWithSameRating[Rnd.Next(0, decksWithSameRating.Count)];

      firstDeck = first;
      secondDeck = second;
    }

    public interface IFactory
    {
      ViewModel Create();
    }
  }
}