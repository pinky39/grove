namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Artifical.TimingRules;
  using Gameplay.Effects;
  using Gameplay.Misc;

  public class DayOfJudgment : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Day of Judgment")
        .ManaCost("{2}{W}{W}")
        .Type("Sorcery")
        .Text("Destroy all creatures.")
        .Cast(p =>
          {
            p.TimingRule(new OnSecondMain());
            p.Effect = () => new DestroyAllPermanents((e, c) => c.Is().Creature);
          });
    }
  }
}