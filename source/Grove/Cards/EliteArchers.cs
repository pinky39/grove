namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TargetingRules;
  using Ai.TimingRules;
  using Gameplay.Card.Costs;
  using Gameplay.Card.Factory;
  using Gameplay.Effects;

  public class EliteArchers : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Elite Archers")
        .ManaCost("{5}{W}")
        .Type("Creature Human Soldier Archer")
        .Text("{T}: Elite Archers deals 3 damage to target attacking or blocking creature.")
        .FlavorText("Arrows fletched with the feathers of angels seldom miss their mark.")
        .Power(3)
        .Toughness(3)
        .ActivatedAbility(p =>
          {
            p.Text = "{T}: Elite Archers deals 3 damage to target attacking or blocking creature.";
            p.Cost = new Tap();
            p.Effect = () => new DealDamageToTargets(3);
            p.TargetSelector.AddEffect(trg => trg.Is.AttackerOrBlocker().On.Battlefield());

            p.TargetingRule(new DealDamage(3));
            p.TimingRule(new DeclareBlockers());
          }
        );
    }
  }
}