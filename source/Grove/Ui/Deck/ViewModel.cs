namespace Grove.Ui.Deck
{
  using System.Collections.Generic;
  using System.Linq;
  using Core;
  using Infrastructure;

  public class ViewModel
  {
    private const string NewDeckName = "new deck";
    private readonly CardDatabase _cardDatabase;
    private readonly bool _isReadOnly;
    private readonly Deck _deck;

    public ViewModel(CardDatabase cardDatabase)
    {
      _cardDatabase = cardDatabase;
      _deck = new Deck(cardDatabase);
      _deck.Name = NewDeckName;
      IsSaved = true;
      IsNew = true;
    }

    public ViewModel(Deck deck, CardDatabase cardDatabase, bool isReadOnly)
    {
      _deck = deck;
      _cardDatabase = cardDatabase;
      _isReadOnly = isReadOnly;
      SelectedCard = _deck.Random();
      IsSaved = true;
    }

    [Updates("Name")]
    public virtual bool IsSaved { get; protected set; }

    public virtual bool IsNew { get; protected set; }


    public virtual int Rating
    {
      get { return _deck.Rating; }
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

    public IEnumerable<DeckRow> Creatures { get { return _deck.Creatures.AsRows().OrderBy(x => x.CardName); } }
    public IEnumerable<DeckRow> Spells { get { return _deck.Spells.AsRows().OrderBy(x => x.CardName); } }
    public IEnumerable<DeckRow> Lands { get { return _deck.Lands.AsRows().OrderBy(x => x.CardName); } }

    public int CreatureCount { get { return _deck.Creatures.Count(); } }
    public int LandCount { get { return _deck.Lands.Count(); } }
    public int SpellCount { get { return _deck.Spells.Count(); } }
    public int CardCount { get { return _deck.Count(); } }

    public virtual Card SelectedCard { get; protected set; }

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

    public void ChangeSelectedCard(string name)
    {
      SelectedCard = _cardDatabase[name];
    }

    [Updates("Creatures", "Spells", "Lands", "CreatureCount", "LandCount", "SpellCount", "CardCount")]
    public virtual void AddCard(string name)
    {
      if (_isReadOnly)
        return;
      
      _deck.AddCard(name);
      IsSaved = false;
    }

    [Updates("Creatures", "Spells", "Lands", "CreatureCount", "LandCount", "SpellCount", "CardCount")]
    public virtual void RemoveCard(string name)
    {
      if (_isReadOnly)
        return;
      
      _deck.RemoveCard(name);
      IsSaved = false;
    }

    public virtual void Save()
    {
      _deck.Save();
      IsSaved = true;
    }

    public virtual void SaveAs(string name)
    {
      _deck.Name = name;
      _deck.Save();

      IsSaved = true;
      IsNew = false;
    }

    public interface IFactory
    {
      ViewModel Create(Deck deck, bool isReadOnly = false);
      ViewModel Create();
    }
  }
}