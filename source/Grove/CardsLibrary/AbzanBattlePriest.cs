namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Modifiers;

  public class AbzanBattlePriest : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Abzan Battle Priest")
        .ManaCost("{3}{W}")
        .Type("Creature - Human Cleric")
        .Text("Outlast {W}{I}({W}, {T}: Put a +1/+1 counter on this creature. Outlast only as a sorcery.){/I}{EOL}Each creature you control with a +1/+1 counter on it has lifelink.")
        .FlavorText("\"Wherever I walk, the ancestors walk too.\"")
        .Power(3)
        .Toughness(2)
        .Outlast("{W}")
        .ContinuousEffect(p =>
        {
          p.Selector = (card, ctx) =>
            card.Is().Creature &&
            card.CountersCount(CounterType.PowerToughness) > 0 &&
            card.Controller == ctx.You;
          p.Modifier = () => new AddSimpleAbility(Static.Lifelink);
        });
    }
  }
}
