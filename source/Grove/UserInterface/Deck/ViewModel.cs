namespace Grove.UserInterface.Deck
{
  using System.Collections.Generic;
  using System.Linq;
  using Gameplay;
  using Infrastructure;
  using Persistance;

  public class ViewModel : ViewModelBase
  {
    private const string NewDeckName = "new deck";
    private readonly bool _isReadOnly;
    private Deck _deck;

    public ViewModel() {}


    public ViewModel(Deck deck, bool isReadOnly)
    {
      _deck = deck;
      _isReadOnly = isReadOnly;
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

    public IEnumerable<DeckRow> Creatures { get { return DeckRow.Group(_deck.Creatures).OrderBy(x => x.CardName); } }
    public IEnumerable<DeckRow> Spells { get { return DeckRow.Group(_deck.Spells).OrderBy(x => x.CardName); } }
    public IEnumerable<DeckRow> Lands { get { return DeckRow.Group(_deck.Lands).OrderBy(x => x.CardName); } }

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

    public override void Initialize()
    {
      IsSaved = true;

      if (_deck == null)
      {
        _deck = new Deck(CardsInfo);
        _deck.Name = NewDeckName;
        SelectedCard = null;
        IsNew = true;
        return;
      }

      SelectedCard = CardsInfo[_deck.Random()];
    }

    public void ChangeSelectedCard(string name)
    {
      SelectedCard = CardsInfo[name];
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
      DeckIo.Write(_deck, MediaLibrary.GetDeckPath(_deck.Name));
      IsSaved = true;
    }

    public virtual void SaveAs(string name)
    {
      _deck.Name = name;
      DeckIo.Write(_deck, MediaLibrary.GetDeckPath(_deck.Name));

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