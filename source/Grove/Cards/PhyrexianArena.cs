namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.CardDsl;
  using Core.Effects;
  using Core.Triggers;

  public class PhyrexianArena : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Phyrexian Arena")
        .ManaCost("{1}{B}{B}")
        .Type("Enchantment")
        .Text("At the beginning of your upkeep, you draw a card and you lose 1 life.")
        .FlavorText("An audience of one with the malice of thousands.")
        .Timing(Timings.Steps(Step.SecondMain))
        .Abilities(
          C.TriggeredAbility(
            "At the beginning of your upkeep, you draw a card and you lose 1 life.",
            C.Trigger<AtBegginingOfStep>((t, _) => {
              t.Step = Step.Upkeep;
            }),
            C.Effect<DrawCards>((e, c) => {
              e.DrawCount = 1;
              e.Lifeloss = 1;
            })));
    }
  }
}