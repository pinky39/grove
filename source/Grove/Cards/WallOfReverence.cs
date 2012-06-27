namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.CardDsl;
  using Core.Effects;
  using Core.Triggers;

  public class WallOfReverence : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Wall of Reverence")
        .ManaCost("{3}{W}")
        .Type("Creature Spirit Wall")
        .Text(
          "{Defender}, {Flying}{EOL}At the beginning of your end step, you may gain life equal to the power of target creature you control.")
        .FlavorText(
          "The lives of elves are long, but their memories are longer. Even after death, they do not desert their homes.")
        .Power(1)
        .Toughness(6)
        .Abilities(
          StaticAbility.Defender,
          StaticAbility.Flying,
          C.TriggeredAbility(
            "At the beginning of your end step, you may gain life equal to the power of target creature you control.",
            C.Trigger<AtBegginingOfStep>((t, _) => { t.Step = Step.EndOfTurn; }),
            C.Effect<GainLifeEqualToTargetPower>(),
            C.Selector(
              validator: (target, card) => target.Is().Creature && target.Card().Controller == card.Controller,
              scorer: TargetScores.YourCreatureWithGreatestPowerOnly()),
            triggerOnlyIfOwningCardIsInPlay: true));
    }
  }
}