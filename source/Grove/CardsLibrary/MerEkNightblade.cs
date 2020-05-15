namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Modifiers;

  public class MerEkNightblade : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Mer-Ek Nightblade")
        .ManaCost("{3}{B}")
        .Type("Creature - Orc Assassin")
        .Text("Outlast {B}{I}({B}, {T}: Put a +1/+1 counter on this creature. Outlast only as a sorcery.){/I}{EOL}Each creature you control with a +1/+1 counter on it has deathtouch.")
        .Power(2)
        .Toughness(3)
        .Outlast("{B}")
        .ContinuousEffect(p =>
        {
          p.Selector = (card, ctx) =>
            card.Is().Creature &&
            card.CountersCount(CounterType.PowerToughness) > 0 &&
            card.Controller == ctx.You;
          p.Modifier = () => new AddSimpleAbility(Static.Deathtouch);
        });
    }
  }
}
