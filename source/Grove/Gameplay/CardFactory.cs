namespace Grove.Gameplay
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Misc;

  public class CardFactory
  {
    private readonly List<CardTemplate> _templates;

    public CardFactory(IEnumerable<CardTemplateSource> cardSources)
    {
      _templates = GetTemplates(cardSources);
    }    

    public Card CreateCard(string name)
    {
      var cardFactory = GetCardTemplate(name);
      return cardFactory.CreateCard();
    }     

    public List<Card> CreateAll()
    {
      return _templates
        .Select(x => x.CreateCard())
        .Where(x => !x.Is("uncastable"))
        .ToList();
    }

    private CardTemplate GetCardTemplate(string name)
    {
      var template = _templates
        .Where(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
        .FirstOrDefault();

      if (template == null)
      {
        throw new InvalidOperationException(
          String.Format("Card with name '{0}' was not found in database.", name));
      }

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
  }
}