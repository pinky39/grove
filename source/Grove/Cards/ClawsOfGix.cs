namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai.TargetingRules;
  using Core.Ai.TimingRules;
  using Core.Costs;
  using Core.Dsl;
  using Core.Effects;
  using Core.Mana;
  using Core.Targeting;

  public class ClawsOfGix : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Claws of Gix")
        .ManaCost("{0}")
        .Type("Artifact")
        .Text("{1}, Sacrifice a permanent: You gain 1 life.")
        .FlavorText(
          "When the Brotherhood of Gix dug out the cave of Koilos they found their master's severed hand. They enshrined it, hoping that one day it would point the way to Phyrexia.")
        .Cast(p =>
          {
            p.TimingRule(new FirstMain());
            p.TimingRule(new ThereCanBeOnlyOne());
          })
        .ActivatedAbility(p =>
          {
            p.Text = "{1}, Sacrifice a permanent: You gain 1 life.";
            p.Cost = new AggregateCost(
              new PayMana(1.Colorless(), ManaUsage.Abilities),
              new Sacrifice());
            p.Effect = () => new ControllerGainsLife(1);
            p.TargetSelector.AddCost(trg =>
              {
                trg.Is.Card(p1 => p1.Effect.Controller == p1.Target.Controller())
                  .On.Battlefield();

                trg.Text = "Select a permanent to sacrifice.";                
              });
            p.TargetingRule(new SacrificeToGainLife());
          }
        );
    }
  }
}