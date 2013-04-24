namespace Grove.Ui.StartScreen
{
  using System;
  using System.Collections.Generic;
  using System.Diagnostics;
  using System.IO;
  using System.Linq;
  using System.Reflection;
  using System.Threading.Tasks;
  using System.Windows;
  using Core;
  using Gameplay;
  using Gameplay.Card.Factory;
  using Gameplay.Deck;
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

      LoadPreviews();
    }

    public virtual bool HasLoaded { get; protected set; }

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

    private void LoadPreviews()
    {
      Task.Factory.StartNew(() =>
        {
          _cardDatabase.LoadPreviews();
          HasLoaded = true;
        });
    }

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
      Deck firstDeck;
      Deck secondDeck;

      ChooseRandomDecks(out firstDeck, out secondDeck);      
      _match.Start(firstDeck, secondDeck);
    }

    public void DeckEditor()
    {
      var deckEditorScreen = _deckEditorScreenFactory.Create(this);
      _shell.ChangeScreen(deckEditorScreen);
    }

    private void ChooseRandomDecks(out Deck firstDeck, out Deck secondDeck)
    {
      var reader = new DeckReaderWriter();
      
      var deckFiles = Directory
        .EnumerateFiles(MediaLibrary.DecksFolder, "*.dec");
      
      var decks = deckFiles
        .Select(x => reader.Read(x, _cardDatabase))
        .ToList();

      var first = decks[Rnd.Next(0, decks.Count)];
      
      var decksWithSameRating = decks
        .Where(x => x.Rating == first.Rating)
        .ToList();
      
      var second = decksWithSameRating[Rnd.Next(0, decksWithSameRating.Count)];

      firstDeck = first;
      secondDeck = second;
    }

    public interface IFactory
    {
      ViewModel Create();
    }
  }
}