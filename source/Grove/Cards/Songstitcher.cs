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
        .Abilities(
          ActivatedAbility(
            "{1}{W}: Prevent all combat damage that would be dealt this turn by target attacking creature with flying.",
            Cost<PayMana>(cost => cost.Amount = "{1}{W}".ParseMana()),
            Effect<ApplyModifiersToTargets>(e => e.Modifiers(
              Modifier<AddDamagePrevention>(m => m.Prevention = Prevention<PreventDealt>(),
                untilEndOfTurn: true))),
            Target(Validators.Card(card => card.IsAttacker && card.Has().Flying), Zones.Battlefield()),
            targetSelectorAi: TargetSelectorAi.PreventAttackerDamage(),
            timing: All(Timings.DeclareAttackers(), Timings.Turn(passive: true))
            ));
    }
  }
}