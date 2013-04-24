namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TimingRules;
  using Core;
  using Gameplay.Card.Costs;
  using Gameplay.Card.Factory;
  using Gameplay.Effects;
  using Gameplay.Mana;

  public class CelestialPrism : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Celestial Prism")
        .ManaCost("{3}")
        .Type("Artifact")
        .Text("{2},{T}: Add one mana of any color to your mana pool.")
        .ActivatedAbility(p =>
          {
            p.Text = "{2},{T}: Add one mana of any color to your mana pool.";

            p.Cost = new AggregateCost(
              new PayMana(2.Colorless(), ManaUsage.Abilities),
              new Tap());

            p.Effect = () => new AddManaToPool(Mana.Any);
            p.TimingRule(new ManaConverter(relativeCost: 1));
          });
    }
  }
}