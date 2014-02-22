namespace Grove.UserInterface.StartScreen
{
  using System;
  using System.Diagnostics;
  using System.Linq;
  using System.Reflection;
  using System.Windows;
  using Gameplay;
  using Infrastructure;
  using SelectDeck;

  public class ViewModel : ViewModelBase
  {
    private const string YourName = "You";

    public string Version
    {
      get
      {
        return String.Format("Version {0}",
          FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion);
      }
    }

    public string CardCount
    {
      get { return string.Format("{0} cards", Cards.Count); }
    }

    public void Exit()
    {
      Application.Current.Shutdown();
    }

    public void NewTournament()
    {
      var newTournament = ViewModels.NewTournament.Create(this);
      Shell.ChangeScreen(newTournament);
    }

    public void LoadSavedGame()
    {
      var loadSavedGame = ViewModels.LoadSavedGame.Create(this);
      Shell.ChangeScreen(loadSavedGame);
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
                    Forward = (deck2) =>
                      {
                        try
                        {
                          var mp = MatchParameters.Default(
                            player1: new PlayerParameters
                              {
                                Name = YourName,
                                AvatarId = RandomEx.Next(),
                                Deck = deck1
                              },
                            player2: new PlayerParameters
                              {
                                Name = NameGenerator.GenerateRandomName(MediaLibrary.GetPlayerUnitNames()),
                                AvatarId = RandomEx.Next(),
                                Deck = deck2
                              },
                            isTournament: false);

                          Ui.Match = new Match(mp);
                          Ui.Match.Start();
                        }
                        catch (Exception ex)
                        {
                          HandleException(ex);
                        }

                        Shell.ChangeScreen(this);
                      }
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
      var decks = ChooseRandomDecks();

      try
      {
        var mp = MatchParameters.Default(
          player1: new PlayerParameters
            {
              Name = YourName,
              AvatarId = RandomEx.Next(),
              Deck = decks[0]
            },
          player2: new PlayerParameters
            {
              Name = NameGenerator.GenerateRandomName(MediaLibrary.GetPlayerUnitNames()),
              AvatarId = RandomEx.Next(),
              Deck = decks[1]
            },
          isTournament: false
          );

        Ui.Match = new Match(mp);
        Ui.Match.Start();        
      }
      catch (Exception ex)
      {
        HandleException(ex);
      }


      Shell.ChangeScreen(this);
    }

    public void DeckEditor()
    {
      var deckEditorScreen = ViewModels.DeckEditor.Create(this);
      Shell.ChangeScreen(deckEditorScreen);
    }

    private Deck[] ChooseRandomDecks()
    {
      var decks = DeckLibrary.ReadDecks().ToList();

      var first = decks[RandomEx.Next(0, decks.Count)];

      var decksWithSameRating = decks
        .Where(x => x.Rating == first.Rating)
        .ToList();

      var second = decksWithSameRating[RandomEx.Next(0, decksWithSameRating.Count)];

      return new[] {first, second};
    }

    public interface IFactory
    {
      ViewModel Create();
    }
  }
}