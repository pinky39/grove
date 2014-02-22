namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay;
  using Gameplay.AI.TimingRules;
  using Grove.Gameplay.Effects;

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