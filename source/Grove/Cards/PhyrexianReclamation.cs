namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TargetingRules;
  using Artifical.TimingRules;
  using Gameplay;
  using Gameplay.Costs;
  using Gameplay.ManaHandling;
  using Gameplay.Misc;
  using Gameplay.States;

  public class PhyrexianReclamation : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Phyrexian Reclamation")
        .ManaCost("{B}")
        .Type("Enchantment")
        .Text("{1}{B}, Pay 2 life: Return target creature card from your graveyard to your hand.")
        .FlavorText("Death is no excuse to stop working.")
        .Cast(p => p.TimingRule(new FirstMain()))
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
            
            p.TimingRule(new Steps(activeTurn: true, passiveTurn: false, steps: Step.FirstMain));
            p.TimingRule(new SingleInstanceOfSpellOnStack());            
            p.TargetingRule(new OrderByRank(c => -c.Score));
          });
    }
  }
}