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
      yield return Card
        .Named("Breach")
        .ManaCost("{2}{B}")
        .Type("Instant")
        .Text("Target creature gets +2/+0 and gains fear until end of turn. (It can't be blocked except by artifact creatures and/or black creatures.)")
        .Effect<ApplyModifiersToTargets>(e => e.Modifiers(
          Modifier<AddStaticAbility>(m => m.StaticAbility = Static.Fear, untilEndOfTurn: true),
          Modifier<AddPowerAndToughness>(m =>
            {
              m.Power = 2;              
            }, untilEndOfTurn: true)))
        .Timing(Timings.NoRestrictions())
        .Targets(
          selectorAi: TargetSelectorAi.AddEvasion(),
          effectValidators: Validator(Validators.Creature()));
    }
  }
}