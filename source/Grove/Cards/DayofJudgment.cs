namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TimingRules;
  using Core;
  using Gameplay.Card.Factory;
  using Gameplay.Effects;

  public class DayOfJudgment : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Day of Judgment")
        .ManaCost("{2}{W}{W}")
        .Type("Sorcery")
        .Text("Destroy all creatures.")
        .Cast(p =>
          {
            p.TimingRule(new SecondMain());
            p.Effect = () => new DestroyAllPermanents((e, c) => c.Is().Creature);
          });
    }
  }
}