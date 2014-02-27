namespace Grove.UserInterface.SelectDeck
{
  using System.Collections.Generic;
  using System.Linq;
  using Castle.Core;
  using Infrastructure;
  using Deck = Grove.Deck;

  public class ViewModel : ViewModelBase
  {
    private readonly Configuration _configuration;
    private readonly List<UserInterface.Deck.ViewModel> _decks = new List<UserInterface.Deck.ViewModel>();

    private UserInterface.Deck.ViewModel _selected;

    public ViewModel(Configuration configuration)
    {
      _configuration = configuration;
    }

    [DoNotWire]
    public virtual UserInterface.Deck.ViewModel Selected
    {
      get { return _selected; }
      set
      {
        _selected = value;
        _selected.SelectedCardChanged += delegate { SelectedCard = Cards.All[_selected.SelectedCard.Name]; };
        SelectedCard = Cards.All[_selected.SelectedCard.Name];
      }
    }

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
      var decks = DeckLibrary.ReadDecks();

      foreach (var deck in decks)
      {
        var deckVm = CreateReadonlyDeckVm(deck);
        _decks.Add(deckVm);
      }

      Selected = _decks.FirstOrDefault();
    }

    private UserInterface.Deck.ViewModel CreateReadonlyDeckVm(Deck deck)
    {
      var deckVm = ViewModels.Deck.Create(deck);

      deckVm.OnAdd = delegate { return false; };
      deckVm.OnRemove = delegate { return false; };
      return deckVm;
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