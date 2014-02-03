namespace Grove.UserInterface.Deck
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Gameplay;
  using Gameplay.ManaHandling;
  using Infrastructure;
  using Persistance;

  public class ViewModel : ViewModelBase
  {
    private const string NewDeckName = "new deck";
    public Func<CardInfo, bool> OnAdd = delegate { return true; };
    public Func<CardInfo, bool> OnRemove = delegate { return true; };
    private Deck _deck;

    public ViewModel() {}

    public ViewModel(Deck deck)
    {
      _deck = deck;
    }

    [Updates("Name")]
    public virtual bool IsSaved { get; protected set; }

    public virtual bool IsNew { get; protected set; }

    public virtual int Rating
    {
      get { return _deck.Rating ?? 3; }
      set
      {
        _deck.Rating = value;
        IsSaved = false;
      }
    }

    public virtual string Description
    {
      get { return _deck.Description; }
      set
      {
        _deck.Description = value;
        IsSaved = false;
      }
    }

    public IEnumerable<DeckRow> Creatures { get { return FilterRows(_deck, c => c.Is().Creature).ToList(); } }
    public IEnumerable<DeckRow> Spells { get { return FilterRows(_deck, c => !c.Is().Creature && !c.Is().Land).ToList(); } }
    public IEnumerable<DeckRow> Lands { get { return FilterRows(_deck, c => c.Is().Land).ToList(); } }

    public int CreatureCount { get { return FilterCards(_deck, c => c.Is().Creature).Count(); } }
    public int LandCount { get { return FilterCards(_deck, c => c.Is().Land).Count(); } }
    public int SpellCount { get { return FilterCards(_deck, c => !c.Is().Creature && !c.Is().Land).Count(); } }
    public int CardCount { get { return _deck.CardCount; } }
    public CardInfo SelectedCard { get; private set; }

    public IManaAmount Colors
    {
      get
      {
        var dictionary = new Dictionary<ManaColor, bool>();

        foreach (var cardInfo in _deck)
        {
          var card = CardDatabase[cardInfo.Name];

          if (card.ManaCost == null)
            continue;

          foreach (var singleColorAmount in card.ManaCost)
          {
            dictionary[singleColorAmount.Color] = true;
          }
        }

        return new MultiColorManaAmount(dictionary
          .Where(x => x.Value)
          .Where(x => x.Key != ManaColor.Colorless)
          .ToDictionary(x => x.Key, x => 1));
      }
    }

    public string Name
    {
      get
      {
        var name = _deck.Name;

        if (!IsSaved)
        {
          name = name + "*";
        }
        return name;
      }
    }

    public Deck Deck { get { return _deck; } }

    public override void Initialize()
    {
      IsSaved = true;

      if (_deck == null)
      {
        _deck = new Deck {Name = NewDeckName};
        SelectedCard = null;
        IsNew = true;
        return;
      }

      SelectedCard = _deck[RandomEx.Next(_deck.CardCount)];
    }

    public void ChangeSelectedCard(CardInfo cardInfo)
    {
      SelectedCard = cardInfo;
      SelectedCardChanged(this, EventArgs.Empty);
    }

    [Updates("Creatures", "Spells", "Lands", "CreatureCount", "LandCount", "SpellCount", "CardCount")]
    public virtual void AddCard(CardInfo cardInfo)
    {
      if (!OnAdd(cardInfo))
        return;

      _deck.AddCard(cardInfo);
      IsSaved = false;
    }

    [Updates("Creatures", "Spells", "Lands", "CreatureCount", "LandCount", "SpellCount", "CardCount")]
    public virtual bool RemoveCard(CardInfo cardInfo)
    {
      if (!OnRemove(cardInfo))
        return false;

      _deck.RemoveCard(cardInfo);
      IsSaved = false;
      return true;
    }

    public virtual void Save()
    {
      DeckLibrary.Write(_deck);
      IsSaved = true;
    }

    public virtual void SaveAs(string name)
    {
      _deck.Name = name;
      DeckLibrary.Write(_deck);

      IsSaved = true;
      IsNew = false;
    }

    public event EventHandler SelectedCardChanged = delegate { };

    private IEnumerable<DeckRow> FilterRows(IEnumerable<CardInfo> cards, Func<Card, bool> predicate)
    {
      return DeckRow.Group(FilterCards(cards, predicate)).OrderBy(x => x.Card.Name);
    }

    private IEnumerable<CardInfo> FilterCards(IEnumerable<CardInfo> cards, Func<Card, bool> predicate)
    {
      return cards.Where(x => predicate(CardDatabase[x.Name]));
    }

    public interface IFactory
    {
      ViewModel Create(Deck deck);
      ViewModel Create();
    }
  }
}