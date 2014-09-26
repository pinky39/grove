namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using Effects;

  public class BlastfireBolt : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Blastfire Bolt")
        .ManaCost("{5}{R}")
        .Type("Instant")
        .Text("Blastfire Bolt deals 5 damage to target creature. Destroy all Equipment attached to that creature.")
        .FlavorText(
          "\"Encase yourself in the most elaborate armor, and cower behind the heaviest shield. I would hate for you to feel helpless.\"{EOL}—Korig the Ruiner")
        .Cast(p =>
          {
            p.Effect = () => new CompoundEffect(
              new DealDamageToTargets(5),
              new DestroyAllPermanents((e, c) => c.Is().Equipment && c.AttachedTo == e.Target));

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());

            p.TargetingRule(new EffectDealDamage(5));
          });
    }
  }
}