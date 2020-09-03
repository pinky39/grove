namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TimingRules;
  using Grove.Costs;
  using Grove.Effects;
  using Grove.AI;
  using Grove.AI.TargetingRules;
  using Grove.Modifiers;
  using Grove.Triggers;

  public class FrilledSandwalla : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Frilled Sandwalla")
        .ManaCost("{G}")
        .Type("Creature Lizard")
        .Text("{1}{G}: Frilled Sandwalla gets +2/+2 until end of turn. Activate this ability only once each turn.")
        .FlavorText("'Even the smallest creatures are fierce in defense of their own territory.'—Vivien Reid")
        .Power(1)
        .Toughness(1)        
        .Pump(
          cost: "{1}{G}".Parse(),
          text: "{1}{G}: Frilled Sandwalla gets +2/+2 until end of turn.",
          powerIncrease: 2,
          toughnessIncrease: 2,
          onlyOnceEachTurn: true);
    }
  }
}