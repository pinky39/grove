namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards;
  using Core.Details.Cards.Effects;
  using Core.Details.Cards.Triggers;
  using Core.Dsl;
  using Core.Targeting;

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
        .Timing(Timings.Creatures())
        .Abilities(
          Static.Defender,
          Static.Flying,
          C.TriggeredAbility(
            "At the beginning of your end step, you may gain life equal to the power of target creature you control.",
            C.Trigger<AtBegginingOfStep>((t, _) => { t.Step = Step.EndOfTurn; }),
            C.Effect<GainLifeEqualToTargetPower>(),
            C.Validator(Validators.Creature(controller: Controller.SpellOwner)),
            aiSelector: AiTargetSelectors.CreatureWithGreatestPower(),
            triggerOnlyIfOwningCardIsInPlay: true));
    }
  }
}