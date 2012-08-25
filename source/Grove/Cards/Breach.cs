namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards;
  using Core.Details.Cards.Effects;
  using Core.Details.Cards.Modifiers;
  using Core.Dsl;
  using Core.Targeting;

  public class Breach : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Breach")
        .ManaCost("{2}{B}")
        .Type("Instant")
        .Text("Target creature gets +2/+0 and gains fear until end of turn. (It can't be blocked except by artifact creatures and/or black creatures.)")
        .Effect<ApplyModifiersToTargets>(p => p.Effect.Modifiers(
          p.Builder.Modifier<AddStaticAbility>((m, _) => m.StaticAbility = Static.Fear, untilEndOfTurn: true),
          p.Builder.Modifier<AddPowerAndToughness>((m, _) =>
            {
              m.Power = 2;              
            }, untilEndOfTurn: true)))
        .Timing(Timings.NoRestrictions())
        .Targets(
          selectorAi: TargetSelectorAi.IncreasePowerAddEvasion(2),
          effectValidators: C.Validator(Validators.Creature()));
    }
  }
}