namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TimingRules;
  using Gameplay.Abilities;
  using Gameplay.Characteristics;
  using Gameplay.Misc;

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