namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Effects;
  using Core.Dsl;

  public class DayOfJudgment : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Day of Judgment")
        .ManaCost("{2}{W}{W}")
        .Type("Sorcery")
        .Text("Destroy all creatures.")
        .Cast(p =>
          {
            p.Timing = Timings.SecondMain();
            p.Category = EffectCategories.Destruction;
            p.Effect = Effect<DestroyAllPermanents>(e => e.Filter = (self, card) => card.Is().Creature);
          });
    }
  }
}