namespace Grove.UserInterface.BuildLimitedDeck
{
  using System.Collections.Generic;
  using System.Linq;
  using Gameplay;
  using Infrastructure;

  public class ViewModel : ViewModelBase
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
      var cardsLeft = _availability[card];

      if (cardsLeft.Count > 0)
      {
        Deck.AddCard(card.Name);
        cardsLeft.Count--;
      }
    }

    public void RemoveCard(Card card)
    {
      var cardsLeft = _availability[card];
      Deck.RemoveCard(card.Name);
      cardsLeft.Count++;
    }

    public void StartTournament() {}

    public override void Initialize()
    {
      Deck = ViewModels.Deck.Create();

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
  }
}