namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.AI.TimingRules;

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
            p.Effect = () => new DestroyAllPermanents((c, ctx) => c.Is().Creature);
          });
    }
  }
}