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
    private readonly int _decksToGenerate;
    private readonly List<CardInfo> _library;
    private Dictionary<string, Card> _cardsWithSetAndRarity;
    private UserInterface.Deck.ViewModel _deck;
    private Dictionary<string, LibraryItem> _libraryItems;
    private double _percentageCompleted;
    private int _remainingDecksToGenerate;

    public ViewModel(IEnumerable<CardInfo> library, int decksToGenerate)
    {
      _decksToGenerate = decksToGenerate;
      _remainingDecksToGenerate = decksToGenerate;
      _library = library.ToList();

      AddBasicLands();
    }

    public virtual Card SelectedCard { get; protected set; }
    public LibraryFilter.ViewModel LibraryFilter { get; private set; }

    public string Status
    {
      get
      {
        if (_remainingDecksToGenerate > 0)
          return String.Format("Building decks {0}% completed.", (int) _percentageCompleted);

        if (Deck.CardCount < 40)
          return "Please add at least 40 cards to your deck.";

        return String.Empty;
      }
    }

    public virtual bool CanStartTournament { get { return _remainingDecksToGenerate == 0 && Deck.CardCount >= 40; } }

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
      _remainingDecksToGenerate--;
    }

    private void AddBasicLands()
    {
      for (var i = 0; i < 40; i++)
      {
        _library.Add(new CardInfo("Plains"));
        _library.Add(new CardInfo("Island"));
        _library.Add(new CardInfo("Swamp"));
        _library.Add(new CardInfo("Mountain"));
        _library.Add(new CardInfo("Forest"));
      }
    }

    public void ChangeSelectedCard(LibraryItem libraryItem)
    {
      SelectedCard = libraryItem.Card;
    }

    [Updates("CanStartTournament", "Status")]
    public virtual void AddCard(LibraryItem libraryItem)
    {
      Deck.AddCard(libraryItem.Info);
    }

    [Updates("CanStartTournament", "Status")]
    public virtual void RemoveCard(LibraryItem libraryItem)
    {
      Deck.RemoveCard(libraryItem.Info);
    }

    public void StartTournament()
    {
      this.Close();
    }

    private bool OnAdd(CardInfo cardInfo)
    {
      var availableCards = _libraryItems[cardInfo.Name];

      if (availableCards.Count == 0)
        return false;

      availableCards.Count--;
      return true;
    }

    public override void Initialize()
    {
      Deck = ViewModels.Deck.Create();
      Deck.OnAdd = OnAdd;
      Deck.OnRemove = OnRemove;

      var uniqueCards = _library
        .Distinct()
        .ToList();

      _cardsWithSetAndRarity = uniqueCards
        .Select(x =>
          {
            var card = CardsDatabase.CreateCard(x.Name);
            card.Rarity = x.Rarity;
            card.Set = x.Set;

            return card;
          })
        .ToDictionary(x => x.Name, x => x);

      _libraryItems = _library.GroupBy(x => x.Name)
        .Select(x =>
          {
            var cardsLeft = Bindable.Create<LibraryItem>();
            cardsLeft.Info = x.First();
            cardsLeft.Card = _cardsWithSetAndRarity[x.Key];
            cardsLeft.Count = x.Count();
            return cardsLeft;
          })
        .ToDictionary(x => x.Info.Name, x => x);


      LibraryFilter = ViewModels.LibraryFilter.Create(
      cards: uniqueCards,
      transformResult: card => _libraryItems[card.Name],
      orderBy: card => card.Rarity.HasValue ? -(int)card.Rarity : 0);


      SelectedCard = _cardsWithSetAndRarity[_library[0].Name];
    }    

    private bool OnRemove(CardInfo cardInfo)
    {
      var cardsLeft = _libraryItems[cardInfo.Name];
      cardsLeft.Count++;
      return true;
    }

    public interface IFactory
    {
      ViewModel Create(IEnumerable<CardInfo> library, int decksToGenerate);
    }
  }
}