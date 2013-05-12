namespace Grove.UserInterface.StartScreen
{
  using System;
  using System.Diagnostics;
  using System.IO;
  using System.Linq;
  using System.Reflection;
  using System.Windows;
  using Gameplay;
  using Persistance;
  using SelectDeck;

  public class ViewModel : ViewModelBase, IIsDialogHost
  {
    private static readonly Random Rnd = new Random();

    public string DatabaseInfo
    {
      get
      {
        return string.Format("Release {0} ({1} cards)",
          FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion,
          CardDatabase.CardCount);
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

    public void NewTournament()
    {
      var newTournament = ViewModels.NewTournament.Create(this);
      Shell.ChangeScreen(newTournament);
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
                selectDeck2 = ViewModels.SelectDeck.Create(new Configuration
                  {
                    ScreenTitle = "Select your opponent deck",
                    ForwardText = "Start the game",
                    PreviousScreen = selectDeck1,
                    Forward = (deck2) => Match.Start(deck1, deck2)
                  });
              }

              Shell.ChangeScreen(selectDeck2);
            }
        };

      selectDeck1 = ViewModels.SelectDeck.Create(configuration1);
      Shell.ChangeScreen(selectDeck1);
    }

    public void PlayRandom()
    {
      Deck firstDeck;
      Deck secondDeck;

      ChooseRandomDecks(out firstDeck, out secondDeck);
      Match.Start(firstDeck, secondDeck);
    }

    public void DeckEditor()
    {
      var deckEditorScreen = ViewModels.DeckEditor.Create(this);
      Shell.ChangeScreen(deckEditorScreen);
    }

    private void ChooseRandomDecks(out Deck firstDeck, out Deck secondDeck)
    {
      var reader = new DeckReaderWriter();

      var deckFiles = Directory
        .EnumerateFiles(MediaLibrary.DecksFolder, "*.dec");

      var decks = deckFiles
        .Select(x => reader.Read(x, CardDatabase))
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