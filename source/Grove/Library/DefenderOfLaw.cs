namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay;
  using Gameplay.AI.TimingRules;

  public class DefenderOfLaw : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Defender of Law")
        .ManaCost("{2}{W}")
        .Type("Creature Human Knight")
        .Text("{Protection from red}, {Flash}")
        .FlavorText("It is not my place to question Radiant's rule. I exist to enforce her will.")
        .Power(2)
        .Toughness(1)
        .SimpleAbilities(Static.Flash)
        .Protections(CardColor.Red)
        .Cast(p => p.TimingRule(new AfterOpponentDeclaresAttackers()));
    }
  }
}