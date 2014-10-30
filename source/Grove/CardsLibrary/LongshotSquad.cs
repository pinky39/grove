namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Modifiers;

  public class LongshotSquad : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Longshot Squad")
        .ManaCost("{3}{G}")
        .Type("Creature - Hound Archer")
        .Text("Outlast {1}{G}{I}({1}{G}, {T}: Put a +1/+1 counter on this creature. Outlast only as a sorcery.){/I}{EOL}Each creature you control with a +1/+1 counter on it has reach. {I}(A creature with reach can block creatures with flying.){/I}")
        .Power(3)
        .Toughness(3)
        .Outlast("{1}{G}")
        .ContinuousEffect(p =>
        {
          p.CardFilter = (card, effect) =>
            card.Is().Creature &&
            card.CountersCount(CounterType.PowerToughness) > 0 &&
            card.Controller == effect.Source.Controller;
          p.Modifier = () => new AddStaticAbility(Static.Reach);
        });
    }
  }
}
