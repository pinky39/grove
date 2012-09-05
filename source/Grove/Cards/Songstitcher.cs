namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Costs;
  using Core.Details.Cards.Effects;
  using Core.Details.Cards.Modifiers;
  using Core.Details.Cards.Preventions;
  using Core.Details.Mana;
  using Core.Dsl;
  using Core.Targeting;

  public class Songstitcher : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Songstitcher")
        .ManaCost("{W}")
        .Type("Creature Human Cleric")
        .Text(
          "{1}{W}: Prevent all combat damage that would be dealt this turn by target attacking creature with flying.")
        .FlavorText("The true names of birds are songs woven into their souls.")
        .Power(1)
        .Toughness(2)
        .Timing(Timings.FirstMain())
        .Abilities(
          C.ActivatedAbility(
            "{1}{W}: Prevent all combat damage that would be dealt this turn by target attacking creature with flying.",
            C.Cost<TapOwnerPayMana>((cost, _) => cost.Amount = "{1}{W}".ParseManaAmount()),
            C.Effect<ApplyModifiersToTargets>(p => p.Effect.Modifiers(
              p.Builder.Modifier<AddDamagePrevention>((m, c0) => m.Prevention = c0.Prevention<PreventDealtDamage>(),
                untilEndOfTurn: true))),
            effectValidator: C.Validator(Validators.Creature(card => card.IsAttacker && card.Has().Flying)),
            selectorAi: TargetSelectorAi.PreventAttackerDamage(),
            timing: All(Timings.DeclareAttackers(), Timings.PassiveTurn())
            ));
    }
  }
}