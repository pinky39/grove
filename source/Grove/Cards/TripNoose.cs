namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TargetingRules;
  using Ai.TimingRules;
  using Core;
  using Gameplay.Card.Costs;
  using Gameplay.Card.Factory;
  using Gameplay.Effects;
  using Gameplay.Mana;
  using Gameplay.States;

  public class TripNoose : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Trip Noose")
        .ManaCost("{2}")
        .Type("Artifact")
        .Text("{2},{T}: Tap target creature.")
        .FlavorText("A taut slipknot trigger is the only thing standing between you and standing.")
        .Cast(p => p.TimingRule(new FirstMain()))
        .ActivatedAbility(p =>
          {
            p.Text = "{2},{T}: Tap target creature.";
            p.Cost = new AggregateCost(
              new PayMana(2.Colorless(), ManaUsage.Abilities),
              new Tap());
            p.Effect = () => new TapTargets();
            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            p.TimingRule(new Steps(Step.BeginningOfCombat));
            p.TargetingRule(new TapCreature());
          });
    }
  }
}