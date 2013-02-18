namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai.TargetingRules;
  using Core.Ai.TimingRules;
  using Core.Dsl;
  using Core.Effects;

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
                  trg.Text = "Select creature.";
                })
              .AddEffect(trg =>
                {
                  trg.Is.Player();
                  trg.Text = "Select player.";
                });

            p.TargetingRule(new DealDamage(1));
            p.TimingRule(new TargetRemoval());
          });
    }
  }
}