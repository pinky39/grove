namespace Grove.Ui.SelectDeck
{
  using System.Collections.Generic;
  using System.IO;
  using Core;
  using Infrastructure;
  using Shell;

  public enum ScreenType
  {
    YourDeck,
    OpponentDeck
  }

  public class ViewModel : IIsDialogHost
  {
    private readonly CardDatabase _cardDatabase;
    private readonly List<Deck> _decks = new List<Deck>();
    private readonly Match _match;
    private readonly IIsDialogHost _previousScreen;
    private readonly ScreenType _screenType;
    private readonly IFactory _selectDeckScreenFactory;
    private readonly IShell _shell;
    private ViewModel _nextScreen;

    private Deck _selected;

    public ViewModel(CardDatabase cardDatabase, ScreenType screenType, IShell shell,
      IIsDialogHost previousScreen, IFactory selectDeckScreenFactory, Match match)
    {
      _cardDatabase = cardDatabase;
      _screenType = screenType;
      _shell = shell;
      _previousScreen = previousScreen;
      _selectDeckScreenFactory = selectDeckScreenFactory;
      _match = match;

      LoadDecks();
    }

    public virtual Deck Selected
    {
      get { return _selected; }
      set
      {
        _selected = value;
        SelectedCard = _selected.GetPreviewCard();
      }
    }    

    public string NextCaption
    {
      get
      {
        return _screenType == ScreenType.YourDeck
          ? "Next"
          : "Start the game";
      }
    }

    public string Title
    {
      get
      {
        return _screenType == ScreenType.YourDeck
          ? "Select your deck"
          : "Select opponent's deck";
      }
    }

    public virtual Card SelectedCard { get; protected set; }
    public IEnumerable<Deck> Decks { get { return _decks; } }

    [Updates("CanStart")]
    public virtual bool IsStarting { get; protected set; }

    public virtual bool CanStart { get { return !IsStarting; } }
    public void AddDialog(object dialog, DialogType dialogType) {}
    public void RemoveDialog(object dialog) {}

    public bool HasFocus(object dialog)
    {
      return false;
    }

    public void CloseAllDialogs() {}

    private void LoadDecks()
    {
      var deckFiles = Directory.EnumerateFiles(MediaLibrary.DecksFolder, "*.dec");
      var previews = _cardDatabase.CreatePreviewForEveryCard();
      
      foreach (var fileName in deckFiles)
      {
        _decks.Add(new Deck(fileName, previews));
      }

      Selected = _decks[0];
    }

    public void ChangeSelectedCard(string name)
    {
      SelectedCard = Selected.GetCard(name);
    }

    public void Forward()
    {
      if (_screenType == ScreenType.YourDeck)
      {
        _nextScreen = _nextScreen ?? _selectDeckScreenFactory.Create(ScreenType.OpponentDeck, this);
        _shell.ChangeScreen(_nextScreen);
        return;
      }

      IsStarting = true;
      var player1Deck = ((ViewModel) _previousScreen).Selected;
      var player2Deck = Selected;

      _match.Start(player1Deck, player2Deck);
    }

    public void Back()
    {
      _shell.ChangeScreen(_previousScreen);
    }

    public interface IFactory
    {
      ViewModel Create(ScreenType screenType, IIsDialogHost previousScreen);
    }
  }
}