namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay;
  using Gameplay.AI.TimingRules;

  public class SimianGrunts : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Simian Grunts")
        .ManaCost("{2}{G}")
        .Type("Creature Ape")
        .Text(
          "{Flash}{EOL}{Echo} {2}{G}(At the beginning of your upkeep, if this came under your control since the beginning of your last upkeep, sacrifice it unless you pay its echo cost.)")
        .FlavorText("These monkeys mean business.")
        .Power(3)
        .Toughness(4)
        .SimpleAbilities(Static.Flash)
        .Cast(p => p.TimingRule(new Any(new AfterOpponentDeclaresAttackers(), new OnEndOfOpponentsTurn())))
        .Echo("{2}{G}");
    }
  }
}