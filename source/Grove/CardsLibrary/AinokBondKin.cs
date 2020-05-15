namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Modifiers;

  public class AinokBondKin : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Ainok Bond-Kin")
        .ManaCost("{1}{W}")
        .Type("Creature - Hound Soldier")
        .Text("Outlast {1}{W}{I}({1}{W}, {T}: Put a +1/+1 counter on this creature. Outlast only as a sorcery.){/I}{EOL}Each creature you control with a +1/+1 counter on it has first strike.")
        .FlavorText("\"Hold the line, for family and the fallen!\"")
        .Power(2)
        .Toughness(1)
        .Outlast("{1}{W}")
        .ContinuousEffect(p =>
        {
          p.Selector = (card, ctx) => 
            card.Is().Creature && 
            card.CountersCount(CounterType.PowerToughness) > 0 &&
            card.Controller == ctx.You;
          p.Modifier = () => new AddSimpleAbility(Static.FirstStrike);
        });
    }
  }
}
