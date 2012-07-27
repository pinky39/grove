namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards;
  using Core.Details.Cards.Effects;
  using Core.Details.Cards.Modifiers;
  using Core.Dsl;
  using Core.Targeting;

  public class VinesOfVastwood : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Vines of Vastwood")
        .ManaCost("{G}")
        .KickerCost("{G}")
        .Type("Instant")
        .Category(EffectCategories.Protector | EffectCategories.ToughnessIncrease)
        .Timing(Timings.NoRestrictions())
        .Text(
          "{Kicker} {G}{EOL}Target creature can't be the target of spells or abilities your opponents control this turn. If Vines of Vastwood was kicked, that creature gets +4/+4 until end of turn.")
        .Effect<ApplyModifiersToTargets>(p => p.Effect.Modifiers(
          p.Builder.Modifier<AddStaticAbility>((m, _) => m.StaticAbility = Static.Hexproof, untilEndOfTurn: true)))
        .Targets(
          aiTargetSelector: AiTargetSelectors.ShieldHexproof(),
          effectValidator: C.Validator(Validators.Creature()))
        .KickerEffect<ApplyModifiersToTargets>(p => p.Effect.Modifiers(
          p.Builder.Modifier<AddStaticAbility>((m, _) => m.StaticAbility = Static.Hexproof, untilEndOfTurn: true),
          p.Builder.Modifier<AddPowerAndToughness>((m, _) =>
            {
              m.Power = 4;
              m.Toughness = 4;
            }, untilEndOfTurn: true)))
        .KickerTargets(
          aiTargetSelector: Any(AiTargetSelectors.ShieldHexproof(), AiTargetSelectors.IncreasePowerAndToughness(4, 4)),
          effectValidators: C.Validator(Validators.Creature()));
    }
  }
}