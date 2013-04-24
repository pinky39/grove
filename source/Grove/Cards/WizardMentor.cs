namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TargetingRules;
  using Core;
  using Gameplay.Card.Costs;
  using Gameplay.Card.Factory;
  using Gameplay.Player;

  public class WizardMentor : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Wizard Mentor")
        .ManaCost("{2}{U}")
        .Type("Creature Human Wizard")
        .Text("{T}: Return Wizard Mentor and target creature you control to their owner's hand.")
        .FlavorText(
          "Although some of the students quickly grasped the concept, the others could summon only blackboards.")
        .Power(2)
        .Toughness(2)
        .ActivatedAbility(p =>
          {
            p.Text = "{T}: Return Wizard Mentor and target creature you control to their owner's hand.";
            p.Cost = new Tap();
            p.Effect = () => new Gameplay.Effects.ReturnToHand(returnOwningCard: true);
            p.TargetSelector.AddEffect(trg => trg.Is.Creature(ControlledBy.SpellOwner).On.Battlefield());
            p.TargetingRule(new BounceSelfAndTargets());
          }
        );
    }
  }
}