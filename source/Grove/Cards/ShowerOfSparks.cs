namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TargetingRules;
  using Artifical.TimingRules;
  using Gameplay.Effects;
  using Gameplay.Misc;

  public class ShowerOfSparks : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
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

            p.TargetingRule(new EffectDealDamage(1));
            p.TimingRule(new TargetRemovalTimingRule());
          });
    }
  }
}