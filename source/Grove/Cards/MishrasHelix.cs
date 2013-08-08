namespace Grove.Cards
{
  using System.Collections.Generic;
  using System.Linq;
  using Artifical.CostRules;
  using Artifical.TargetingRules;
  using Artifical.TimingRules;
  using Gameplay;
  using Gameplay.Costs;
  using Gameplay.Effects;
  using Gameplay.ManaHandling;
  using Gameplay.Misc;
  using Gameplay.Modifiers;
  using Gameplay.States;

  public class MishrasHelix : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Mishra's Helix")
        .ManaCost("{5}")
        .Type("Artifact")
        .Text("{X},{T}: Tap X target lands.")
        .FlavorText(
          "The helix was the finest example of Mishra's campaign strategy: if he couldn't have Argoth, no one could.")        
        .ActivatedAbility(p =>
          {
            p.Text = "{X},{T}: Tap X target lands.";

            p.Cost = new AggregateCost(
              new PayMana(Mana.Zero, ManaUsage.Abilities, hasX: true),
              new Tap());

            p.Effect = () => new TapTargets();

            p.TargetSelector.AddEffect(trg =>
              {
                trg.MinCount = Value.PlusX;
                trg.MaxCount = Value.PlusX;
                trg.Is.Card(c => c.Is().Land).On.Battlefield();
              });

            p.TimingRule(new Steps(steps: Step.Upkeep, activeTurn: false, passiveTurn: true));
            p.TimingRule(new OpponentHasPermanents(x => x.Is().Land));
            p.CostRule(new ControllersProperty(ctrl => ctrl.Opponent.Battlefield.Lands.Count()));
            p.TargetingRule(new TapLands());
          });
    }
  }
}