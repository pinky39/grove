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
    public Func<string, bool> OnAdd = delegate { return true; };
    public Func<string, bool> OnRemove = delegate { return true; };
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
    public int CardCount { get { return _deck.Count(); } }
    public virtual Card SelectedCard { get; protected set; }

    public IManaAmount Colors
    {
      get
      {
        var dictionary = new Dictionary<ManaColor, bool>();

        foreach (var cardName in _deck)
        {
          var card = CardsInfo[cardName];

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

    private IEnumerable<DeckRow> FilterRows(IEnumerable<string> cards, Func<Card, bool> predicate)
    {
      return DeckRow.Group(FilterCards(cards, predicate)).OrderBy(x => x.CardName);
    }

    private IEnumerable<string> FilterCards(IEnumerable<string> cards, Func<Card, bool> predicate)
    {
      return cards.Where(x => predicate(CardsInfo[x]));
    }

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
      
      SelectedCard = CardsInfo[_deck[RandomEx.Next(_deck.CardCount)]];
    }

    public void ChangeSelectedCard(string name)
    {
      SelectedCard = CardsInfo[name];
    }

    [Updates("Creatures", "Spells", "Lands", "CreatureCount", "LandCount", "SpellCount", "CardCount")]
    public virtual void AddCard(string name)
    {
      if (!OnAdd(name))
        return;

      _deck.AddCard(name);
      IsSaved = false;
    }

    [Updates("Creatures", "Spells", "Lands", "CreatureCount", "LandCount", "SpellCount", "CardCount")]
    public virtual bool RemoveCard(string name)
    {
      if (!_deck.Contains(name))
        return false;

      if (!OnRemove(name))
        return false;

      _deck.RemoveCard(name);
      IsSaved = false;
      return true;
    }

    public virtual void Save()
    {
      DeckFile.Write(_deck, MediaLibrary.GetDeckPath(_deck.Name));
      IsSaved = true;
    }

    public virtual void SaveAs(string name)
    {
      _deck.Name = name;
      DeckFile.Write(_deck, MediaLibrary.GetDeckPath(_deck.Name));

      IsSaved = true;
      IsNew = false;
    }

    public interface IFactory
    {
      ViewModel Create(Deck deck);
      ViewModel Create();
    }
  }
}