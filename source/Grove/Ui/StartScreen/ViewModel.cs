namespace Grove.Ui.StartScreen
{
  using System;
  using System.Collections.Generic;
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
    private readonly DeckEditor.ViewModel.IFactory _deckEditorScreenFactory;
    private readonly Match _match;
    private readonly SelectDeck.ViewModel.IFactory _selectDeckScreenFactory;
    private readonly IShell _shell;

    public ViewModel(
      IShell shell,
      CardDatabase cardDatabase,
      SelectDeck.ViewModel.IFactory selectDeckScreenFactory,
      DeckEditor.ViewModel.IFactory deckEditorScreenFactory,
      Match match)
    {
      _shell = shell;
      _cardDatabase = cardDatabase;
      _selectDeckScreenFactory = selectDeckScreenFactory;
      _deckEditorScreenFactory = deckEditorScreenFactory;
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
      SelectDeck.ViewModel selectDeck1 = null;
      SelectDeck.ViewModel selectDeck2 = null;

      var configuration1 = new Configuration
        {
          ScreenTitle = "Select your deck",
          ForwardText = "Next",
          PreviousScreen = this,
          Forward = (deck1) =>
            {

              if (selectDeck2 == null)
              {
                selectDeck2 = _selectDeckScreenFactory.Create(new Configuration
                  {
                    ScreenTitle = "Select your opponent deck",
                    ForwardText = "Start the game",
                    PreviousScreen = selectDeck1,
                    Forward = (deck2) => _match.Start(deck1, deck2)
                  });
              }

              _shell.ChangeScreen(selectDeck2);
            }
        };

      selectDeck1 = _selectDeckScreenFactory.Create(configuration1);
      _shell.ChangeScreen(selectDeck1);
    }

    public void PlayRandom()
    {
      List<string> firstDeck;
      List<string> secondDeck;

      ChooseRandomDecks(out firstDeck, out secondDeck);
      _match.Start(firstDeck, secondDeck);
    }

    public void DeckEditor()
    {
      var deckEditorScreen = _deckEditorScreenFactory.Create(this);
      _shell.ChangeScreen(deckEditorScreen);
    }

    private void ChooseRandomDecks(out List<string> firstDeck, out List<string> secondDeck)
    {
      DeckFile first;
      DeckFile second;

      var deckFiles = Directory.EnumerateFiles(MediaLibrary.DecksFolder, "*.dec");
      var decks = deckFiles.Select(DeckFileReader.Read).ToList();
      first = decks[Rnd.Next(0, decks.Count)];
      var decksWithSameRating = decks.Where(x => x.Rating == first.Rating).ToList();
      second = decksWithSameRating[Rnd.Next(0, decksWithSameRating.Count)];

      firstDeck = first.AllCards.ToList();
      secondDeck = second.AllCards.ToList();
    }

    public interface IFactory
    {
      ViewModel Create();
    }
  }
}