namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TargetingRules;
  using Ai.TimingRules;
  using Core;
  using Gameplay.Card.Costs;
  using Gameplay.Card.Factory;
  using Gameplay.Mana;
  using Gameplay.Player;

  public class BarrinMasterWizard : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Barrin, Master Wizard")
        .ManaCost("{1}{U}{U}")
        .Type("Legendary Creature Human Wizard")
        .Text("{2}, Sacrifice a permanent: Return target creature to its owner's hand.")
        .FlavorText(
          "Knowledge is no more expensive than ignorance, and at least as satisfying.")
        .Power(1)
        .Toughness(1)
        .ActivatedAbility(p =>
          {
            p.Text = "{2}, Sacrifice a permanent: Return target creature to its owner's hand.";
            p.Cost = new AggregateCost(
              new PayMana(2.Colorless(), ManaUsage.Abilities),
              new Sacrifice());
            p.Effect = () => new Gameplay.Effects.ReturnToHand();
            p.TargetSelector
              .AddCost(trg =>
                {
                  trg.Is.Card(controlledBy: ControlledBy.SpellOwner).On.Battlefield();
                  trg.Message = "Select a permanent to sacrifice.";
                })
              .AddEffect(trg =>
                {
                  trg.Is.Card(c => c.Is().Creature).On.Battlefield();
                  trg.Message = "Select a creature to bounce.";
                });

            p.TargetingRule(new SacrificeToBounce());
            p.TimingRule(new TargetRemoval());
          });
    }
  }
}