namespace Grove
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;
  using System.Reflection;
  using Infrastructure;

  public static class Cards
  {
    private static readonly List<CardTemplate> Templates;
    private static CachedCards _cached;

    static Cards()
    {
      var sources = Assembly
        .GetExecutingAssembly()
        .GetTypes()
        .Where(x => x.IsSubclassOf(typeof (CardTemplateSource)))
        .Select(x => (CardTemplateSource) x.GetParameterlessCtor()());

      Templates = GetTemplates(sources);
    }

    public static int Count { get { return All.Count; } }

    public static CachedCards All
    {
      get
      {
        return _cached ??
          (_cached = new CachedCards(Templates
            .Select(x => x.CreateCard())
            .Where(x => !x.Is("uncastable"))));
      }
    }

    public static Card Create(string name) { return GetCardTemplate(name).CreateCard(); }

    private static CardTemplate GetCardTemplate(string name)
    {
      var template = Templates
        .FirstOrDefault(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));

      Asrt.True(template != null,
        String.Format("Card with name '{0}' was not found.", name));

      return template;
    }

    private static List<CardTemplate> GetTemplates(IEnumerable<CardTemplateSource> sources)
    {
      var factories = new List<CardTemplate>();
      foreach (var source in sources)
      {
        factories.AddRange(source.GetCards());
      }
      return factories;
    }

    public class CachedCards : IEnumerable<Card>
    {
      private static Dictionary<string, Card> _cards;

      public CachedCards(IEnumerable<Card> cards) { _cards = cards.ToDictionary(x => x.Name.ToLowerInvariant(), x => x); }

      public int Count { get { return _cards.Count; } }
      public IEnumerable<string> Names { get { return _cards.Values.Select(x => x.Name); } }

      public Card this[string name] { get { return _cards[name.ToLowerInvariant()]; } }

      public IEnumerator<Card> GetEnumerator() { return _cards.Values.GetEnumerator(); }

      IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
    }
  }
}