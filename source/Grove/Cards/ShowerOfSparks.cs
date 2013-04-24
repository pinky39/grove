namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TargetingRules;
  using Ai.TimingRules;
  using Core;
  using Gameplay.Card.Factory;
  using Gameplay.Effects;

  public class ShowerOfSparks : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Shower of Sparks")
        .ManaCost("{R}")
        .Type("Instant")
        .Text("Shower of Sparks deals 1 damage to target creature and 1 damage to target player.")
        .FlavorText("The viashino had learned how to operate the rig through trial and error—mostly error.")
        .Cast(p =>
          {
            p.Effect = () => new DealDamageToTargets(1);
            p.TargetSelector
              .AddEffect(trg =>
                {
                  trg.Is.Creature().On.Battlefield();
                  trg.Message = "Select creature.";
                })
              .AddEffect(trg =>
                {
                  trg.Is.Player();
                  trg.Message = "Select player.";
                });

            p.TargetingRule(new DealDamage(1));
            p.TimingRule(new TargetRemoval());
          });
    }
  }
}