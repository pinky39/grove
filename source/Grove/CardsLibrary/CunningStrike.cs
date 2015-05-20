namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;

  public class CunningStrike : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Cunning Strike")
        .ManaCost("{3}{U}{R}")
        .Type("Instant")
        .Text("Cunning Strike deals 2 damage to target creature and 2 damage to target player.{EOL}Draw a card.")
        .FlavorText("\"The opponent who blocks the path, becomes the path.\"{EOL}—Shu Yun, the Silent Tempest")
        .Cast(p =>
          {
            p.Effect = () => new CompoundEffect(
              new DealDamageToTargets(2),
              new DrawCards(1));

            p.TargetSelector
              .AddEffect(
                trg => trg.Is.Creature().On.Battlefield(),
                trg => trg.Message = "Select a creature.")
              .AddEffect(
                trg => trg.Is.Player(),
                trg => trg.Message = "Select a player.");

            p.TargetingRule(new EffectDealDamage(2));
            p.TimingRule(new TargetRemovalTimingRule(removalTag: EffectTag.DealDamage));
          });
    }
  }
}