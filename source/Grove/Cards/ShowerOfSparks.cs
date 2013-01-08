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
        .Text("Shower of Sparks deals 1 damage to target creature and 1 damage to target player.")
        .FlavorText("The viashino had learned how to operate the rig through trial and error—mostly error.")
        .Cast(p =>
          {
            p.Timing = Timings.InstantRemovalTarget();
            p.Effect = Effect<DealDamageToTargets>(e => e.Amount = 1);
            p.EffectTargets = L(
              Target(Validators.Card(x => x.Is().Creature), Zones.Battlefield(), text: "Select target creature."),
              Target(Validators.Player(), Zones.None(), "Select target player."));
            p.TargetSelectorAi = TargetSelectorAi.DealDamageMultipleSelectors(amount: 1);
          });
    }
  }
}