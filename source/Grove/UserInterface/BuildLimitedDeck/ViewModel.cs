namespace Grove.UserInterface.BuildLimitedDeck
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Gameplay;
  using Infrastructure;
  using Messages;

  public class ViewModel : ViewModelBase , IReceive<DeckGenerated>
  {
    private readonly List<string> _library;
    private Dictionary<Card, CardsLeft> _availability;
    private UserInterface.Deck.ViewModel _deck;

    public ViewModel(IEnumerable<string> library)
    {
      _library = library.ToList();
    }

    public virtual Card SelectedCard { get; protected set; }
    public LibraryFilter.ViewModel LibraryFilter { get; private set; }
    public virtual string Status { get; protected set; }
    public virtual bool CanStartTournament { get; protected set; }

    public virtual UserInterface.Deck.ViewModel Deck
    {
      get { return _deck; }
      set
      {
        _deck = value;
        _deck.Property(x => x.SelectedCard).Changes(this).Property<ViewModel, Card>(x => x.SelectedCard);
      }
    }

    public void ChangeSelectedCard(Card card)
    {
      SelectedCard = card;
    }

    public void AddCard(Card card)
    {      
      Deck.AddCard(card.Name);      
    }

    public void RemoveCard(Card card)
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
      ViewModel Create(IEnumerable<string> library);
    }

    public void Receive(DeckGenerated message)
    {
      Status = String.Format("Building player decks {0} of {1}...", message.Count, message.TotalCount);

      if (message.Count == message.TotalCount)
        CanStartTournament = true;
    }
  }
}