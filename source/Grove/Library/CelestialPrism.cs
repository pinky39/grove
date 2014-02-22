namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay.AI.TimingRules;
  using Grove.Gameplay;
  using Grove.Gameplay.Costs;
  using Grove.Gameplay.Effects;

  public class CelestialPrism : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
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
            p.TimingRule(new ConvertManaTimingRule(relativeCost: 1));
          });
    }
  }
}