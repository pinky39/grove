namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.AI;
  using Grove.AI.TargetingRules;
  using Grove.AI.TimingRules;

  public class Parch : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Parch")
        .ManaCost("{1}{R}")
        .Type("Instant")
        .Text(
          "Choose one — Parch deals 2 damage to target creature or player; or Parch deals 4 damage to target blue creature.")
        .FlavorText("Your porous flesh betrays you.")
        .Cast(p =>
          {
            p.Text = "Parch deals 2 damage to target creature or player";
            p.Effect = () => new DealDamageToTargets(2);
            p.TargetSelector.AddEffect(trg => trg.Is.CreatureOrPlayer().On.Battlefield());
            p.TargetingRule(new EffectDealDamage(2));
            p.TimingRule(new TargetRemovalTimingRule(EffectTag.DealDamage));
          })
        .Cast(p =>
          {
            p.Text = "Parch deals 4 damage to target blue creature.";
            p.Effect = () => new DealDamageToTargets(4);
            p.TargetSelector.AddEffect(
              trg => trg.Is.Card(c => c.Is().Creature && c.HasColor(CardColor.Blue)).On.Battlefield());
            p.TargetingRule(new EffectDealDamage(4));
            p.TimingRule(new TargetRemovalTimingRule(EffectTag.DealDamage));
          });
    }
  }
}