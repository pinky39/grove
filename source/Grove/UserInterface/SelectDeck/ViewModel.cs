namespace Grove.UserInterface.SelectDeck
{
  using System.Collections.Generic;
  using System.Linq;
  using Castle.Core;
  using Gameplay;
  using Infrastructure;
  using Persistance;

  public class ViewModel : ViewModelBase
  {
    private readonly Configuration _configuration;
    private readonly List<UserInterface.Deck.ViewModel> _decks = new List<UserInterface.Deck.ViewModel>();

    public ViewModel(Configuration configuration)
    {
      _configuration = configuration;
    }

    private UserInterface.Deck.ViewModel _selected;

    [DoNotWire]
    public virtual UserInterface.Deck.ViewModel Selected { get { return _selected; } set
    {
      _selected = value;
      _selected.SelectedCardChanged += delegate { SelectedCard = CardsDictionary[_selected.SelectedCard.Name]; };
      SelectedCard = CardsDictionary[_selected.SelectedCard.Name];
    } }

    public string NextCaption { get { return _configuration.ForwardText; } }
    public virtual Card SelectedCard { get; protected set; }

    public string Title { get { return _configuration.ScreenTitle; } }

    public List<UserInterface.Deck.ViewModel> Decks { get { return _decks; } }

    [Updates("CanStart")]
    public virtual bool IsStarting { get; protected set; }

    public virtual bool CanStart { get { return !IsStarting; } }

    public override void Initialize()
    {
      LoadDecks();
    }

    private void LoadDecks()
    {
      var deckFiles = MediaLibrary.GetDeckFilenames();

      foreach (var fileName in deckFiles)
      {
        var deck = CreateReadonlyDeck(fileName);
        _decks.Add(deck);
      }

      Selected = _decks.FirstOrDefault();
    }

    private UserInterface.Deck.ViewModel CreateReadonlyDeck(string fileName)
    {
      var deck = ViewModels.Deck.Create(
        DeckFile.Read(fileName));

      deck.OnAdd = delegate { return false; };
      deck.OnRemove = delegate { return false; };
      return deck;
    }


    public void Forward()
    {
      IsStarting = true;
      _configuration.Forward(Selected.Deck);
      IsStarting = false;
    }

    public void Back()
    {
      Shell.ChangeScreen(_configuration.PreviousScreen);
    }

    public interface IFactory
    {
      ViewModel Create(Configuration configuration);
    }
  }
}