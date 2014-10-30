namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Modifiers;

  public class AbzanFalconer : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Abzan Falconer")
        .ManaCost("{2}{W}")
        .Type("Creature - Human Soldier")
        .Text("Outlast {W}{I}({W}, {T}: Put a +1/+1 counter on this creature. Outlast only as a sorcery.){/I}{EOL}Each creature you control with a +1/+1 counter on it has flying.")
        .FlavorText("The fastest way across the dunes is above.")
        .Power(2)
        .Toughness(3)
        .Outlast("{W}")
        .ContinuousEffect(p =>
        {
          p.CardFilter = (card, effect) =>
            card.Is().Creature &&
            card.CountersCount(CounterType.PowerToughness) > 0 &&
            card.Controller == effect.Source.Controller;
          p.Modifier = () => new AddStaticAbility(Static.Flying);
        });
    }
  }
}
