namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Costs;
  using Effects;
  using Modifiers;

  public class YisanTheWandererBard : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Yisan, the Wanderer Bard")
        .ManaCost("{2}{G}")
        .Type("Legendary Creature — Human Rogue")
        .Text("{2}{G}, {T}, Put a verse counter on Yisan, the Wanderer Bard: Search your library for a creature card with converted mana cost equal to the number of verse counters on Yisan, put it onto the battlefield, then shuffle your library.")
        .Power(2)
        .Toughness(3)
        .ActivatedAbility(p =>
        {
          p.Cost = new AggregateCost(
            new PayMana("{2}{G}".Parse()),
            new Tap());

          p.Effect = () => new CompoundEffect(
            new ApplyModifiersToSelf(() => new AddCounters(
              () => new SimpleCounter(CounterType.Verse), 1)),
            new SearchLibraryPutToZone(
            zone: Zone.Battlefield,
            minCount: 0,
            maxCount: 1,
            validator: (c, ctx) => c.Is().Creature && c.ConvertedCost == ctx.OwningCard.CountersCount(CounterType.Verse),
            text: "Search you library for a creature card."));
        });
    }
  }
}
