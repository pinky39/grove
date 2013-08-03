namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Artifical.TargetingRules;
  using Artifical.TimingRules;
  using Gameplay;
  using Gameplay.Costs;
  using Gameplay.Effects;
  using Gameplay.ManaHandling;
  using Gameplay.Misc;
  using Gameplay.States;

  public class RingOfGix : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Ring of Gix")
        .Type("Artifact")        
        .ManaCost("{3}")
        .Text("{Echo} {3}{EOL}{1},{T}: Tap target artifact, creature, or land.")
        .FlavorText("Not every cage is made of bars.")
        .Echo("{3}")
        .ActivatedAbility(p =>
          {
            p.Text = "{1},{T}: Tap target artifact, creature, or land.";

            p.Cost = new AggregateCost(
              new PayMana(1.Colorless(), ManaUsage.Abilities),
              new Tap());

            p.Effect = () => new TapTargets();

            p.TargetSelector.AddEffect(trg => trg.Is.Card(
              c => c.Is().Creature || c.Is().Land || c.Is().Artifact));

            p.TimingRule(new Steps(Step.BeginningOfCombat));
            p.TargetingRule(new TapCreature());
          });
    }
  }
}