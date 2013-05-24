namespace Grove.UserInterface.BuildLimitedDeck
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Gameplay;
  using Infrastructure;
  using Messages;

  public class ViewModel : ViewModelBase, IReceive<DeckGenerated>
  {
    private readonly List<string> _library;
    private Dictionary<Card, CardsLeft> _availability;
    private UserInterface.Deck.ViewModel _deck;
    private int _decksToGenerate;
    private double _percentageCompleted;

    public ViewModel(IEnumerable<string> library, int decksToGenerate)
    {
      _decksToGenerate = decksToGenerate;
      _library = library.ToList();

      AddBasicLands();
    }

    public virtual Card SelectedCard { get; protected set; }
    public LibraryFilter.ViewModel LibraryFilter { get; private set; }

    public string Status
    {
      get
      {
        if (_decksToGenerate > 0)
          return String.Format("Building decks {0}% completed.", (int) _percentageCompleted);

        if (Deck.CardCount < 40)
          return "Please add at least 40 cards to your deck.";

        return String.Empty;
      }
    }

    public virtual bool CanStartTournament { get { return _decksToGenerate == 0 && Deck.CardCount >= 40; } }

    public virtual UserInterface.Deck.ViewModel Deck
    {
      get { return _deck; }
      set
      {
        _deck = value;
        _deck.Property(x => x.SelectedCard).Changes(this).Property<ViewModel, Card>(x => x.SelectedCard);
      }
    }

    public Deck Result { get { return Deck.Deck; } }

    [Updates("CanStartTournament", "Status")]
    public virtual void Receive(DeckGenerated message)
    {
      _percentageCompleted = ((double) message.Count/_decksToGenerate)*100;
      _decksToGenerate--;
    }

    private void AddBasicLands()
    {
      for (var i = 0; i < 40; i++)
      {
        _library.Add("Plains");
        _library.Add("Island");
        _library.Add("Swamp");
        _library.Add("Mountain");
        _library.Add("Forest");
      }
    }

    public void ChangeSelectedCard(Card card)
    {
      SelectedCard = card;
    }

    [Updates("CanStartTournament", "Status")]
    public virtual void AddCard(Card card)
    {
      Deck.AddCard(card.Name);
    }

    [Updates("CanStartTournament", "Status")]
    public virtual void RemoveCard(Card card)
    {
      Deck.RemoveCard(card.Name);
    }

    public void StartTournament()
    {
      this.Close();
    }

    private bool OnAdd(string cardName)
    {
      var cardsLeft = _availability[CardsInfo[cardName]];

      if (cardsLeft.Count == 0)
        return false;

      cardsLeft.Count--;
      return true;
    }

    public override void Initialize()
    {
      Deck = ViewModels.Deck.Create();
      Deck.OnAdd = OnAdd;
      Deck.OnRemove = OnRemove;

      _availability = _library.GroupBy(x => x)
        .Select(x =>
          {
            var cardsLeft = Bindable.Create<CardsLeft>();
            cardsLeft.Card = CardsInfo[x.Key];
            cardsLeft.Count = x.Count();
            return cardsLeft;
          })
        .ToDictionary(x => x.Card, x => x);

      SelectedCard = CardsInfo[_library[0]];

      LibraryFilter = ViewModels.LibraryFilter.Create(
        _library.Distinct(),
        card => _availability[card]);
    }

    private bool OnRemove(string cardName)
    {
      var cardsLeft = _availability[CardsInfo[cardName]];
      cardsLeft.Count++;
      return true;
    }

    public interface IFactory
    {
      ViewModel Create(IEnumerable<string> library, int decksToGenerate);
    }
  }
}