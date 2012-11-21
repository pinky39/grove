namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Costs;
  using Core.Cards.Effects;
  using Core.Dsl;
  using Core.Targeting;

  public class SanctumGuardian : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Sanctum Guardian")
        .ManaCost("{1}{W}{W}")
        .Type("Creature Human Cleric")
        .Text(
          "Sacrifice Sanctum Guardian: The next time a source of your choice would deal damage to target creature or player this turn, prevent that damage.")
        .FlavorText("'Protect our mother in her womb.'")
        .Power(1)
        .Toughness(4)
        .Timing(Timings.Creatures())
        .Abilities(
          ActivatedAbility(
            "Sacrifice Sanctum Guardian: The next time a source of your choice would deal damage to target creature or player this turn, prevent that damage.",
            Cost<SacrificeOwner>(),
            Effect<PreventDamageFromSourceToTarget>(e => e.OnlyOnce = true),
            effectValidators: new[]
              {
                TargetValidator(TargetIs.EffectOrPermanent(), text: "Select damage source."),
                TargetValidator(TargetIs.CreatureOrPlayer(), text:  "Select a creature or player.")
              },
            targetSelectorAi: TargetSelectorAi.PreventAllDamageFromSourceToTarget(),
            timing: Timings.NoRestrictions()
            )
        );
    }
  }
}