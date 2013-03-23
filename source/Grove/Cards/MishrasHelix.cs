namespace Grove.Cards
{
  using System.Collections.Generic;
  using System.Linq;
  using Core;
  using Core.Ai.CostRules;
  using Core.Ai.TimingRules;
  using Core.Costs;
  using Core.Dsl;
  using Core.Effects;
  using Core.Mana;
  using Core.Modifiers;

  public class MishrasHelix : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
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
              new PayMana(ManaAmount.Zero, ManaUsage.Abilities, hasX: true),
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
            p.CostRule(new OpponentProperty(opponent => opponent.Battlefield.Lands.Count()));
            p.TargetingRule(new Core.Ai.TargetingRules.TapLands());
          });
    }
  }
}