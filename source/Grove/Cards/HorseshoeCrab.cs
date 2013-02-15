namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai.TimingRules;
  using Core.Costs;
  using Core.Dsl;
  using Core.Effects;
  using Core.Mana;

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
            p.Cost = new PayMana(ManaAmount.Blue, ManaUsage.Abilities);
            p.Effect = () => new UntapOwner();

            p.TimingRule(new Turn(active: true));
            p.TimingRule(new MainSteps());
            p.TimingRule(new OwningCardHas(c => c.IsTapped));
          });
    }
  }
}