namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TimingRules;
  using Core;
  using Gameplay.Card.Costs;
  using Gameplay.Card.Factory;
  using Gameplay.Effects;
  using Gameplay.Mana;

  public class HorseshoeCrab : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Horseshoe Crab")
        .ManaCost("{2}{U}")
        .Type("Creature - Crab")
        .Text("{U}: Untap Horseshoe Crab.")
        .FlavorText(
          "In the final days before the disaster, all the crabs on Tolaria migrated from inlets, streams, and ponds back to the sea. No one took note.")
        .Power(1)
        .Toughness(3)
        .ActivatedAbility(p =>
          {
            p.Text = "{U}: Untap Horseshoe Crab.";
            p.Cost = new PayMana(Mana.Blue, ManaUsage.Abilities);
            p.Effect = () => new UntapOwner();

            p.TimingRule(new Turn(active: true));
            p.TimingRule(new SecondMain());
            p.TimingRule(new OwningCardHas(c => c.IsTapped));
          });
    }
  }
}