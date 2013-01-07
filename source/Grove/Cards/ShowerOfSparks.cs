namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Effects;
  using Core.Dsl;
  using Core.Targeting;

  public class ShowerOfSparks : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Shower of Sparks")
        .ManaCost("{R}")
        .Type("Instant")
        .Timing(Timings.InstantRemovalTarget())
        .Text("Shower of Sparks deals 1 damage to target creature and 1 damage to target player.")
        .FlavorText("The viashino had learned how to operate the rig through trial and error—mostly error.")
        .Effect<DealDamageToTargets>(e => e.Amount = 1)
        .Targets(
          TargetSelectorAi.DealDamageMultipleSelectors(amount: 1),
          new[]
            {
              Target(
                Validators.Card(x => x.Is().Creature),
                Zones.Battlefield(), text: "Select target creature."),
              Target(Validators.Player(), Zones.None(), "Select target player.")
            }
        );
    }
  }
}