namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Costs;
  using Core.Cards.Effects;
  using Core.Cards.Modifiers;
  using Core.Cards.Preventions;
  using Core.Dsl;
  using Core.Mana;
  using Core.Targeting;

  public class Songstitcher : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
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
          ActivatedAbility(
            "{1}{W}: Prevent all combat damage that would be dealt this turn by target attacking creature with flying.",
            Cost<TapOwnerPayMana>(cost => cost.Amount = "{1}{W}".ParseManaAmount()),
            Effect<ApplyModifiersToTargets>(e => e.Modifiers(
              Modifier<AddDamagePrevention>(m => m.Prevention = Prevention<PreventDealt>(),
                untilEndOfTurn: true))),
            effectValidator: TargetValidator(TargetIs.Creature(card => card.IsAttacker && card.Has().Flying)),
            targetSelectorAi: TargetSelectorAi.PreventAttackerDamage(),
            timing: All(Timings.DeclareAttackers(), Timings.PassiveTurn())
            ));
    }
  }
}