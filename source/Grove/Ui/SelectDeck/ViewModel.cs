namespace Grove.Ui.SelectDeck
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
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
    private static readonly Random Rnd = new Random();
    private readonly CardDatabase _cardDatabase;
    private readonly List<DeckList> _decks = new List<DeckList>();
    private readonly Match _match;
    private readonly IIsDialogHost _previousScreen;
    private readonly ScreenType _screenType;
    private readonly IFactory _selectDeckScreenFactory;
    private readonly IShell _shell;
    private int _currentDeckList;
    private ViewModel _nextScreen;

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

    public string CurrentDeckName { get { return IsRandomDeck ? GetRandomDeck() : _decks[_currentDeckList].Name; } }

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
    public bool IsRandomDeck { get; set; }

    [Updates("CanStart")]
    public virtual bool IsStarting { get; protected set; }

    public virtual bool CanStart { get { return !IsStarting; } }

    public DeckList CurrentDeckList { get { return _decks[_currentDeckList]; } }
    public void AddDialog(object dialog, DialogType dialogType) {}

    public void RemoveDialog(object dialog) {}

    public bool HasFocus(object dialog)
    {
      return false;
    }

    private string GetRandomDeck()
    {
      return _decks[Rnd.Next(0, _decks.Count)].Name;
    }

    private void LoadDecks()
    {
      var deckFiles = Directory.EnumerateFiles(MediaLibrary.DecksFolder, "*.dec");

      foreach (var fileName in deckFiles)
      {
        _decks.Add(new DeckList(fileName, _cardDatabase));
      }

      ChangeSelectedCard(_decks.First().Creatures.First().CardName);
    }

    public void ChangeSelectedCard(string name)
    {
      SelectedCard = _cardDatabase.CreateCardPreview(name);
    }

    [Updates("CurrentDeckList")]
    public virtual void NextDeck()
    {
      _currentDeckList++;

      if (_currentDeckList >= _decks.Count)
        _currentDeckList = 0;
    }

    [Updates("CurrentDeckList")]
    public virtual void PreviousDeck()
    {
      _currentDeckList--;

      if (_currentDeckList < 0)
        _currentDeckList = _decks.Count - 1;
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
      var player1Deck = ((ViewModel) _previousScreen).CurrentDeckName;
      var player2Deck = CurrentDeckName;

      _match.Start(player1Deck, player2Deck);
    }

    public void Back()
    {
      _shell.ChangeScreen(_previousScreen);
    }

    public class DeckList
    {
      private readonly List<DeckFileRow> _creatures = new List<DeckFileRow>();
      private readonly List<DeckFileRow> _lands = new List<DeckFileRow>();
      private readonly List<DeckFileRow> _spells = new List<DeckFileRow>();

      public DeckList(string filename, CardDatabase cardDatabase)
      {
        Name = Path.GetFileNameWithoutExtension(filename);

        var all = DeckFileReader.Read(filename);

        foreach (var row in all)
        {
          var preview = cardDatabase.CreateCardPreview(row.CardName);

          if (preview.Is().Creature)
            _creatures.Add(row);
          else if (preview.Is().Land)
            _lands.Add(row);
          else
          {
            _spells.Add(row);
          }
        }
      }

      public IEnumerable<DeckFileRow> Creatures { get { return _creatures; } }
      public int CreaturesCount { get { return Creatures.Sum(x => x.Count); } }
      public int SpellsCount { get { return Spells.Sum(x => x.Count); } }
      public int LandsCount { get { return Lands.Sum(x => x.Count); } }
      public IEnumerable<DeckFileRow> Spells { get { return _spells; } }
      public IEnumerable<DeckFileRow> Lands { get { return _lands; } }

      public string Name { get; private set; }
    }

    public interface IFactory
    {
      ViewModel Create(ScreenType screenType, IIsDialogHost previousScreen);
    }
  }
}