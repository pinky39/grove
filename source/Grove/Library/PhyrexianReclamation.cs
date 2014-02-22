namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay.AI.TargetingRules;
  using Gameplay.AI.TimingRules;
  using Grove.Gameplay;
  using Grove.Gameplay.Costs;

  public class PhyrexianReclamation : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Phyrexian Reclamation")
        .ManaCost("{B}")
        .Type("Enchantment")
        .Text("{1}{B}, Pay 2 life: Return target creature card from your graveyard to your hand.")
        .FlavorText("Death is no excuse to stop working.")
        .Cast(p => p.TimingRule(new OnFirstMain()))
        .ActivatedAbility(p =>
          {
            p.Text = "{1}{B}, Pay 2 life: Return target creature card from your graveyard to your hand.";

            p.Cost = new AggregateCost(
              new PayMana("{1}{B}".Parse(), ManaUsage.Abilities),
              new PayLife(2));

            p.Effect = () => new Gameplay.Effects.ReturnToHand();

            p.TargetSelector.AddEffect(trg =>
              {
                trg.Is.Creature().In.YourGraveyard();
                trg.Message = "Select a creature from your graveyard.";
              });

            p.TimingRule(new OnFirstMain());
            p.TargetingRule(new EffectRankBy(c => -c.Score));
          });
    }
  }
}