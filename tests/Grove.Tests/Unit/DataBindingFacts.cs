namespace Grove.Tests.Unit
{
  using System.Collections;
  using System.Collections.Generic;
  using System.Collections.Specialized;
  using System.ComponentModel;
  using System.Linq;
  using Grove.Infrastructure;
  using Xunit;

  public class DataBindingFacts
  {
    [Fact]
    public void Add()
    {
      var deckOfCards = Bindable.Create<DeckOfCards>();

      var notifyCollectionChanged = deckOfCards as INotifyCollectionChanged;
      var notifyPropertyChanged = deckOfCards as INotifyPropertyChanged;

      Card actual = null;
      notifyCollectionChanged.CollectionChanged += (s, a) => { actual = (Card) a.NewItems[0]; };

      var propertiesThatChaned = new List<string>();
      notifyPropertyChanged.PropertyChanged += (s, a) => propertiesThatChaned.Add(a.PropertyName);

      var expected = new Card();
      deckOfCards.Add(expected);

      Assert.Equal(expected, actual);
      Assert.Equal(new[] {"Count", "IsEmpty"},
        propertiesThatChaned.OrderBy(x => x).ToArray());
    }

    [Fact]
    public void Clear()
    {
      var deckOfCards = Bindable.Create<DeckOfCards>();
      deckOfCards.Add(new Card());

      var notify = deckOfCards as INotifyCollectionChanged;
      var wasReset = false;
      notify.CollectionChanged += (s, a) => { wasReset = a.Action == NotifyCollectionChangedAction.Reset; };

      deckOfCards.Clear();
      Assert.True(wasReset);
    }

    [Fact]
    public void DependantProperties()
    {
      var deckOfCards = Bindable.Create<DeckOfCards>();

      var dependantHasChanged = false;

      var notifier = deckOfCards as INotifyPropertyChanged;
      notifier.PropertyChanged += (s, e) => { if (e.PropertyName == "TopCardName") dependantHasChanged = true; };

      deckOfCards.TopCard.Name = "Some cards";
      Assert.True(dependantHasChanged);
    }

    [Fact]
    public void Remove()
    {
      var expected = new Card();
      var deckOfCards = Bindable.Create<DeckOfCards>();
      deckOfCards.Add(new Card());
      deckOfCards.Add(expected);
      deckOfCards.Add(new Card());

      var notify = deckOfCards as INotifyCollectionChanged;

      Card actual = null;
      var index = 0;

      notify.CollectionChanged += (s, a) =>
        {
          actual = (Card) a.OldItems[0];
          index = a.OldStartingIndex;
        };

      deckOfCards.Remove(expected);

      Assert.Equal(expected, actual);
      Assert.Equal(1, index);
    }

    public class Card
    {
      public virtual string Name { get; set; }
    }

    public class DeckOfCards : IEnumerable<Card>
    {
      private readonly List<Card> _cards = new List<Card>();

      public DeckOfCards()
      {
        TopCard = Bindable.Create<Card>();

        TopCard.Property(x => x.Name)
          .Changes(this).Property<DeckOfCards, string>(x => x.TopCardName);
      }

      public int Count { get { return _cards.Count; } }

      public Card TopCard { get; private set; }

      public string TopCardName { get { return TopCard.Name; } }

      public IEnumerator<Card> GetEnumerator()
      {
        return _cards.GetEnumerator();
      }

      IEnumerator IEnumerable.GetEnumerator()
      {
        return GetEnumerator();
      }

      public virtual void Add(Card card)
      {
        _cards.Add(card);
      }

      public virtual void Clear()
      {
        _cards.Clear();
      }

      public virtual void Remove(Card card)
      {
        _cards.Remove(card);
      }
    }
  }
}