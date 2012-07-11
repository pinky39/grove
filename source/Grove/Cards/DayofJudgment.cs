namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Effects;
  using Core.Dsl;

  public class DayOfJudgment : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Day of Judgment")
        .ManaCost("{2}{W}{W}")
        .Type("Sorcery")
        .Text("Destroy all creatures.")
        .Timing(Timings.MainPhases())
        .Category(EffectCategories.Destruction)
        .Effect<DestroyPermanents>((e, _) => e.Filter = (card) => card.Is().Creature);
    }
  }
}