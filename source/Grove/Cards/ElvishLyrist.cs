namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TargetingRules;
  using Ai.TimingRules;
  using Gameplay.Card.Costs;
  using Gameplay.Card.Factory;
  using Gameplay.Effects;
  using Gameplay.Mana;

  public class ElvishLyrist : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Elvish Lyrist")
        .ManaCost("{G}")
        .Type("Creature Elf")
        .Text("{G},{T}, Sacrifice Elvish Lyrist: Destroy target enchantment.")
        .FlavorText(
          "Bring the spear of ancient briar;{EOL}Bring the torch to light the pyre.{EOL}Bring the one who trod our ground;{EOL}Bring the spade to dig his mound.")
        .Power(1)
        .Toughness(1)
        .ActivatedAbility(p =>
          {
            p.Text = "{G},{T}, Sacrifice Elvish Lyrist: Destroy target enchantment.";
            p.Cost = new AggregateCost(
              new PayMana(Mana.Green, ManaUsage.Abilities),
              new Tap());
            p.Effect = () => new DestroyTargetPermanents();
            p.TargetSelector.AddEffect(trg => trg.Is.Enchantment().On.Battlefield());

            p.TargetingRule(new Destroy());
            p.TimingRule(new TargetRemoval());
          }
        );
    }
  }
}