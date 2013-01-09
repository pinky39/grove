namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Effects;
  using Core.Cards.Triggers;
  using Core.Dsl;

  public class PhyrexianArena : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Phyrexian Arena")
        .ManaCost("{1}{B}{B}")
        .Type("Enchantment")
        .Text("At the beginning of your upkeep, you draw a card and you lose 1 life.")
        .FlavorText("An audience of one with the malice of thousands.")
        .Cast(p => p.Timing = Timings.SecondMain())
        .Abilities(
          TriggeredAbility(
            "At the beginning of your upkeep, you draw a card and you lose 1 life.",
            Trigger<OnStepStart>(t => { t.Step = Step.Upkeep; }),
            Effect<DrawCards>(e =>
              {
                e.DrawCount = 1;
                e.Lifeloss = 1;
              }), triggerOnlyIfOwningCardIsInPlay: true));
    }
  }
}